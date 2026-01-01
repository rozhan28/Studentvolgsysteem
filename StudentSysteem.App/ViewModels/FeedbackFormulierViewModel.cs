using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentSysteem.App.ViewModels
{
    [QueryProperty(nameof(IsZelfEvaluatie), "isZelf")]
    public partial class FeedbackFormulierViewModel : BasisViewModel
    {
        private readonly IMeldingService _meldingService;
        private readonly IFeedbackFormulierService _feedbackService;
        private readonly IPrestatiedoelService _prestatiedoelService;
        private readonly IVaardigheidService _vaardigheidService;
        private readonly IToelichtingService _toelichtingService;
        private readonly ZelfEvaluatieViewModel _zelfEvaluatieViewModel;

        private readonly bool _isDocent;

        public ICommand OpslaanCommand { get; }
        public ICommand VoegExtraToelichtingToeCommand { get; }
        public ICommand OptiesCommand { get; }
        public ICommand LeertakenCommand { get; }

        [ObservableProperty]
        private bool isZelfEvaluatie;

        private ObservableCollection<BeoordelingItem> _beoordelingen;
        public ObservableCollection<BeoordelingItem> Beoordelingen
        {
            get => _beoordelingen;
            set
            {
                _beoordelingen = value;
                OnPropertyChanged();
            }
        }

        private string _statusMelding;
        public string StatusMelding
        {
            get => _statusMelding;
            set
            {
                _statusMelding = value;
                OnPropertyChanged();
            }
        }

        public FeedbackFormulierViewModel(
            IZelfEvaluatieService zelfEvaluatieService,
            IMeldingService meldingService,
            IFeedbackFormulierService feedbackService,
            IPrestatiedoelService prestatiedoelService,
            IVaardigheidService vaardigheidService,
            IToelichtingService toelichtingService,
            GlobaleViewModel globaal)
        {
            _zelfEvaluatieViewModel = new ZelfEvaluatieViewModel(zelfEvaluatieService);
            _meldingService = meldingService;
            _feedbackService = feedbackService;
            _prestatiedoelService = prestatiedoelService;
            _vaardigheidService = vaardigheidService;
            _toelichtingService = toelichtingService;

            _isDocent = globaal.IngelogdeGebruiker?.Rol == Role.Docent;

            OpslaanCommand = new Command(async () => await BewaarEvaluatieAsync());
            VoegExtraToelichtingToeCommand = new Command<BeoordelingItem>(VoegExtraToelichtingToe);
            OptiesCommand = new Command<Toelichting>(async t => await ShowOptiesPicker(t));
            LeertakenCommand = new Command<string>(async url => await OpenLeertakenUrl(url));

            Task.Run(async () => await InitialiseerPaginaAsync());
        }

        private async Task InitialiseerPaginaAsync()
        {
            try
            {
                LaadPrestatiedoelen();
            }
            catch (Exception ex)
            {
                StatusMelding = "Fout bij laden van prestatiedoelen.";
                Debug.WriteLine(ex);
            }
        }

        private void LaadPrestatiedoelen()
        {
            var doelen = _prestatiedoelService.HaalPrestatiedoelenOp();
            var vaardigheden = _vaardigheidService.HaalAlleVaardighedenOp();

            var items = doelen.Select(d =>
            {
                var vaardigheid = vaardigheden.FirstOrDefault(v => v.Prestatiedoel_id == d.Id);

                return new BeoordelingItem
                {
                    PrestatiedoelId = d.Id,
                    Titel = $"Prestatiedoel {d.Id}",
                    PrestatiedoelBeschrijving = d.Beschrijving,
                    AiAssessmentScale = d.AiAssessmentScale,
                    Vaardigheid = vaardigheid?.VaardigheidNaam ?? "Geen vaardigheid",
                    LeertakenUrl = vaardigheid?.LeertakenUrl,
                    HboiActiviteit = vaardigheid?.HboiActiviteit,
                    Beschrijving = vaardigheid?.VaardigheidBeschrijving,
                    Toelichtingen = new ObservableCollection<Toelichting>()
                };
            }).ToList();

            Beoordelingen = new ObservableCollection<BeoordelingItem>(items);

            foreach (var item in Beoordelingen)
            {
                item.Toelichtingen.Add(_toelichtingService.MaakNieuweToelichting());
                HookToelichtingen(item);
            }
        }

        private async Task BewaarEvaluatieAsync()
        {
            StatusMelding = string.Empty;

            if (!ValideerBeoordelingen())
            {
                StatusMelding = "Controleer alle velden.";
                return;
            }

            try
            {
                int zelfEvaluatieId = _zelfEvaluatieViewModel.SlaZelfEvaluatieOp(1);

                foreach (var item in Beoordelingen)
                {
                    _feedbackService.SlaToelichtingenOp(
                        item.Toelichtingen.ToList(),
                        zelfEvaluatieId);
                }

                await _meldingService.ToonMeldingAsync(
                    "Succes",
                    IsZelfEvaluatie
                        ? "Zelfevaluatie opgeslagen"
                        : "Feedback opgeslagen");
            }
            catch (Exception ex)
            {
                StatusMelding = $"Opslaan mislukt: {ex.Message}";
            }
        }

        private bool ValideerBeoordelingen()
        {
            bool geldig = true;

            foreach (var item in Beoordelingen)
            {
                bool niveauOk =
                    item.InOntwikkeling ||
                    item.IsOpNiveau ||
                    item.IsBovenNiveau;

                bool toelichtingOk =
                    _isDocent ||
                    ZijnAlleToelichtingenOk(item.Toelichtingen);

                item.IsPrestatieNiveauInvalid = !niveauOk;
                item.IsToelichtingInvalid = !toelichtingOk;

                if (!niveauOk || !toelichtingOk)
                    geldig = false;
            }

            return geldig;
        }

        private bool ZijnAlleToelichtingenOk(ObservableCollection<Toelichting> toelichtingen)
        {
            if (toelichtingen == null || !toelichtingen.Any())
                return false;

            return toelichtingen.All(t =>
                !string.IsNullOrWhiteSpace(t.Tekst) &&
                t.GeselecteerdeOptie != "Toelichting gekoppeld aan...");
        }

        private void VoegExtraToelichtingToe(BeoordelingItem item)
        {
            if (item == null) return;
            if (item.Toelichtingen.Count >= _toelichtingService.TotaleOptiesCount) return;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                item.Toelichtingen.Add(_toelichtingService.MaakNieuweToelichting());
            });
        }

        private async Task ShowOptiesPicker(Toelichting toelichting)
        {
            if (toelichting == null) return;

            var parent = Beoordelingen.FirstOrDefault(b => b.Toelichtingen.Contains(toelichting));
            if (parent == null) return;

            var opties = _toelichtingService.GetBeschikbareOpties(parent.Toelichtingen);

            string selected = await Application.Current.MainPage.DisplayActionSheet(
                "Toelichting gekoppeld aan...",
                "Annuleren",
                null,
                opties.ToArray());

            if (!string.IsNullOrWhiteSpace(selected) && selected != "Annuleren")
            {
                int index = parent.Toelichtingen.IndexOf(toelichting);
                parent.Toelichtingen[index] = new Toelichting
                {
                    Tekst = toelichting.Tekst,
                    GeselecteerdeOptie = selected
                };

                OnPropertyChanged(nameof(Beoordelingen));
            }
        }

        private void HookToelichtingen(BeoordelingItem item)
        {
            item.KanExtraToelichtingToevoegen =
                item.Toelichtingen.Count < _toelichtingService.TotaleOptiesCount;

            item.Toelichtingen.CollectionChanged += (_, _) =>
            {
                item.KanExtraToelichtingToevoegen =
                    item.Toelichtingen.Count < _toelichtingService.TotaleOptiesCount;

                MainThread.BeginInvokeOnMainThread(() =>
                    OnPropertyChanged(nameof(Beoordelingen)));
            };
        }

        private async Task OpenLeertakenUrl(string url)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                await Browser.Default.OpenAsync(url);
        }
    }
}
