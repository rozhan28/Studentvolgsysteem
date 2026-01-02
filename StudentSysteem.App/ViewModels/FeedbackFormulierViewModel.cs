using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;
using StudentSysteem.Core.Services;

namespace StudentSysteem.App.ViewModels
{
    [QueryProperty(nameof(IsZelfEvaluatie), "isZelf")]
    public partial class FeedbackFormulierViewModel : BasisViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ICriteriumRepository _criteriumRepository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IPrestatiedoelService _prestatiedoelService;
        private readonly IZelfEvaluatieService _zelfEvaluatieService;
        private readonly IMeldingService _meldingService;
        private readonly IFeedbackFormulierService _feedbackService;
        private readonly IVaardigheidService _vaardigheidService;
        private readonly IToelichtingService _toelichtingService;
        private readonly bool _isDocent;

        private ObservableCollection<BeoordelingItem> _beoordelingen;
        public ObservableCollection<BeoordelingItem> Beoordelingen
        {
            get => _beoordelingen;
            set { _beoordelingen = value; OnPropertyChanged(); }
        }

        private string _statusMelding;
        public string StatusMelding
        {
            get => _statusMelding;
            set { _statusMelding = value; OnPropertyChanged(); }
        }

        // Begin code voor paginatitel in header
        [ObservableProperty]
        bool isZelfEvaluatie;

        public void UpdateTitelGebaseerdOpStatus()
        {
            Titel = IsZelfEvaluatie ? "Zelfevaluatieformulier" : "Feedbackformulier";
        }
        // Einde code voor paginatitel in header

        public ICommand OpslaanCommand { get; }
        public ICommand VoegExtraToelichtingToeCommand { get; }
        public ICommand OptiesCommand { get; }
        public ICommand LeertakenCommand { get; }

        private readonly Dictionary<int, bool> _geselecteerdeCriteria = new();
        private readonly Dictionary<int, string> _toelichtingen = new();

        public FeedbackFormulierViewModel(
            IPrestatiedoelService prestatiedoelService,
            ICriteriumRepository criteriumRepository,
            IFeedbackRepository feedbackRepository,
            IZelfEvaluatieService zelfEvaluatieService,
            IMeldingService meldingService,
            IFeedbackFormulierService feedbackService,
            IVaardigheidService vaardigheidService,
            IToelichtingService toelichtingService,
            GlobaleViewModel globaal)
        {
            _prestatiedoelService = prestatiedoelService;
            _criteriumRepository = criteriumRepository;
            _feedbackRepository = feedbackRepository;
            _zelfEvaluatieService = zelfEvaluatieService;
            _meldingService = meldingService;
            _feedbackService = feedbackService;
            _vaardigheidService = vaardigheidService;
            _toelichtingService = toelichtingService;

            _isDocent = globaal.IngelogdeGebruiker?.Rol == Role.Docent;

            Task.Run(async () => await InitialiseerPaginaAsync());

            OpslaanCommand = new Command(async () => await BewaarEvaluatieAsync());
            LeertakenCommand = new Command<string>(async (url) => await OpenLeertakenUrl(url));
            VoegExtraToelichtingToeCommand = new Command<BeoordelingItem>(item => VoegExtraToelichtingToe(item));
            OptiesCommand = new Command<Toelichting>(async t => await ShowOptiesPicker(t));
        }

        private async Task InitialiseerPaginaAsync()
        {
            try
            {
                LaadPrestatiedoelen();
            }
            catch (Exception ex)
            {
                StatusMelding = "Databasefout: De prestatiedoelen konden niet worden geladen.";
                Debug.WriteLine($"Fout: {ex.Message}");
            }
        }

        private void LaadPrestatiedoelen()
        {
            IEnumerable<Prestatiedoel> doelen = _prestatiedoelService.HaalPrestatiedoelenOp();
            IEnumerable<Vaardigheid> vaardigheden = _vaardigheidService.HaalAlleVaardighedenOp();

            List<BeoordelingItem> items = doelen.Select(d =>
            {
                Vaardigheid gekoppeldeVaardigheid = vaardigheden.FirstOrDefault(v => v.PrestatiedoelId == d.Id);
                return new BeoordelingItem
                {
                    PrestatiedoelId = d.Id,
                    Titel = $"Prestatiedoel {d.Id}",
                    PrestatiedoelBeschrijving = d.Beschrijving,
                    AiAssessmentScale = d.AiAssessmentScale,
                    Vaardigheid = gekoppeldeVaardigheid?.VaardigheidNaam ?? "Geen vaardigheid gekoppeld",
                    LeertakenUrl = gekoppeldeVaardigheid?.LeertakenUrl,
                    HboiActiviteit = gekoppeldeVaardigheid?.HboiActiviteit,
                    Beschrijving = gekoppeldeVaardigheid?.VaardigheidBeschrijving,
                    OpNiveauCriteria = new ObservableCollection<Criterium>(
                        _criteriumRepository.HaalCriteriaOpVoorPrestatiedoel(d.Id, "Op niveau")
                    ),
                                    BovenNiveauCriteria = new ObservableCollection<Criterium>(
                        _criteriumRepository.HaalCriteriaOpVoorPrestatiedoel(d.Id, "Boven niveau")
                    )
                };
            }).ToList();

            Beoordelingen = new ObservableCollection<BeoordelingItem>(items);

            foreach (var item in Beoordelingen)
            {
                if (item.Toelichtingen == null)
                    item.Toelichtingen = new ObservableCollection<Toelichting>();

                item.Toelichtingen.Add(_toelichtingService.MaakNieuweToelichting());
                HookToelichtingen(item);
            }
        }

        private async Task BewaarEvaluatieAsync()
        {
            StatusMelding = string.Empty;

            if (!ValideerBeoordelingen())
            {
                StatusMelding = "Controleer alle velden a.u.b.";
                return;
            }

            try
            {
                int idVorigRecord = _zelfEvaluatieService.VoegToe(new ZelfEvaluatie
                {
                    StudentId = 1,
                    PrestatieNiveau = "Ingevuld"
                });

                foreach (var item in Beoordelingen)
                {
                    if (item.Toelichtingen != null && item.Toelichtingen.Any())
                        _feedbackService.SlaToelichtingenOp(item.Toelichtingen.ToList(), idVorigRecord);
                }

                await _meldingService.ToonMeldingAsync(
                    "Succes",
                    IsZelfEvaluatie ? "Je zelfevaluatie is opgeslagen!" : "De feedback is succesvol opgeslagen!");
            }
            catch (Exception ex)
            {
                StatusMelding = $"Fout bij opslaan: {ex.Message}";
            }
        }

        private bool ValideerBeoordelingen()
        {
            bool allesGeldig = true;

            foreach (var item in Beoordelingen)
            {
                bool niveauGekozen = ValideerPrestatieNiveau(item);
                bool criteriumGekozen =
                    item.BeschikbareCriteria.Any(c => _geselecteerdeCriteria.TryGetValue(c.Id, out var val) && val);

                bool niveauOfCriteriumOk = niveauGekozen || criteriumGekozen;
                item.IsPrestatieNiveauInvalid = !niveauOfCriteriumOk;
                item.IsCriteriumInvalid = !niveauOfCriteriumOk;

                bool toelichtingOk = _isDocent || ZijnAlleToelichtingenOk(item.Toelichtingen);
                item.IsToelichtingInvalid = !toelichtingOk;

                if (!niveauOfCriteriumOk || !toelichtingOk)
                    allesGeldig = false;
            }

            return allesGeldig;
        }

        public bool IsCriteriumInvalid { get; set; } = false;

        private bool _kanExtraToelichtingToevoegen;
        public bool KanExtraToelichtingToevoegen
        {
            get => _kanExtraToelichtingToevoegen;
            set
            {
                if (_kanExtraToelichtingToevoegen == value) return;
                _kanExtraToelichtingToevoegen = value;
                Notify();
            }
        }


        public bool ZijnAlleToelichtingenOk(ObservableCollection<Toelichting> toelichtingen)
        {
            if (_isDocent) return true;
            if (toelichtingen == null || !toelichtingen.Any()) return false;
            return toelichtingen.All(t => IsToelichtingCorrect(t));
        }

        private bool IsToelichtingCorrect(Toelichting toelichting)
        {
            if (_isDocent) return true;
            return !string.IsNullOrWhiteSpace(toelichting.Tekst) &&
                   toelichting.GeselecteerdeOptie != "Toelichting gekoppeld aan...";
        }

        private static bool ValideerPrestatieNiveau(BeoordelingItem item) =>
            item.InOntwikkeling || item.IsOpNiveau || item.IsBovenNiveau;

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

            var opties = _toelichtingService.GetBeschikbareOpties(parent.Toelichtingen, parent.PrestatiedoelId);


            // Converteer naar string array voor DisplayActionSheet
            string[] optieStrings = opties.Select(o => o.ToString()).ToArray();

            string selected = await Application.Current.MainPage.DisplayActionSheet(
                "Toelichting gekoppeld aan...",
                "Annuleren",
                null,
                optieStrings);

            if (!string.IsNullOrEmpty(selected) && selected != "Annuleren")
            {
                int idx = parent.Toelichtingen.IndexOf(toelichting);
                Toelichting nieuwe = new() { Tekst = toelichting.Tekst, GeselecteerdeOptie = selected };
                parent.Toelichtingen[idx] = nieuwe;
                OnPropertyChanged(nameof(Beoordelingen));
            }
        }

        private void HookToelichtingen(BeoordelingItem item)
        {
            item.KanExtraToelichtingToevoegen = item.Toelichtingen.Count < _toelichtingService.TotaleOptiesCount;

            void Handler(object s, NotifyCollectionChangedEventArgs e)
            {
                item.KanExtraToelichtingToevoegen = item.Toelichtingen.Count < _toelichtingService.TotaleOptiesCount;
                MainThread.BeginInvokeOnMainThread(() => OnPropertyChanged(nameof(Beoordelingen)));
            }

            item.Toelichtingen.CollectionChanged += Handler;
        }

        private async Task OpenLeertakenUrl(string url)
        {
            if (!string.IsNullOrWhiteSpace(url) && Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                await Browser.Default.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
            }
        }

        private void Notify([System.Runtime.CompilerServices.CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

