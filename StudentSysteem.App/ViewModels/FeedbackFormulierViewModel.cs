using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

namespace StudentSysteem.App.ViewModels
{
    public class FeedbackFormulierViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly INavigatieService _navigatieService;
        private readonly IMeldingService _meldingService;
        private readonly IFeedbackFormulierService _feedbackService;
        private readonly IPrestatiedoelService _prestatiedoelService;
        private readonly IVaardigheidService _vaardigheidService;
        private readonly IToelichtingService _toelichtingService;
        private readonly ZelfEvaluatieViewModel _zelfEvaluatieViewModel;

        private readonly bool _isDocent;

        public ObservableCollection<BeoordelingItem> Beoordelingen
        {
            get => _beoordelingen;
            set
            {
                _beoordelingen = value;
                Notify(nameof(Beoordelingen));
            }
        }
        private ObservableCollection<BeoordelingItem> _beoordelingen = new();

        private string _statusMelding;
        public string StatusMelding
        {
            get => _statusMelding;
            set
            {
                _statusMelding = value;
                Notify(nameof(StatusMelding));
            }
        }

        public ICommand OpslaanCommand { get; }
        public ICommand LeertakenCommand { get; }
        public ICommand VoegExtraToelichtingToeCommand { get; }
        public ICommand OptiesCommand { get; }

        public FeedbackFormulierViewModel(
            INavigatieService navigatieService,
            IMeldingService meldingService,
            IFeedbackFormulierService feedbackService,
            IPrestatiedoelService prestatiedoelService,
            IVaardigheidService vaardigheidService,
            IToelichtingService toelichtingService,
            IZelfEvaluatieService zelfEvaluatieService,
            bool isDocent = false)
        {
            _navigatieService = navigatieService;
            _meldingService = meldingService;
            _feedbackService = feedbackService;
            _prestatiedoelService = prestatiedoelService;
            _vaardigheidService = vaardigheidService;
            _toelichtingService = toelichtingService;
            _zelfEvaluatieViewModel = new ZelfEvaluatieViewModel(zelfEvaluatieService);

            _isDocent = isDocent;

            OpslaanCommand = new Command(async () => await BewaarEvaluatieAsync());
            LeertakenCommand = new Command<string>(async url => await OpenLeertakenUrl(url));
            VoegExtraToelichtingToeCommand = new Command<BeoordelingItem>(VoegExtraToelichtingToe);
            OptiesCommand = new Command<Toelichting>(async t => await ShowOptiesPicker(t));

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
                StatusMelding = "Databasefout: Prestatiedoelen konden niet worden geladen.";
                System.Diagnostics.Debug.WriteLine(ex);
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

                    Vaardigheid = vaardigheid?.VaardigheidNaam ?? "Geen vaardigheid gekoppeld",
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
                StatusMelding = "Controleer alle velden a.u.b.";
                return;
            }

            try
            {
                int zelfEvaluatieId = _zelfEvaluatieViewModel.SlaZelfEvaluatieOp(1);

                foreach (var item in Beoordelingen)
                {
                    if (item.Toelichtingen != null && item.Toelichtingen.Any())
                    {
                        _feedbackService.SlaToelichtingenOp(
                            item.Toelichtingen.ToList(),
                            zelfEvaluatieId);
                    }
                }

                await _meldingService.ToonMeldingAsync(
                    "Succes",
                    "Evaluatie is succesvol opgeslagen.");
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
                item.IsPrestatieNiveauInvalid = !niveauGekozen;

                bool toelichtingOk = _isDocent || ZijnAlleToelichtingenOk(item.Toelichtingen);
                item.IsToelichtingInvalid = !toelichtingOk;

                if (!niveauGekozen || !toelichtingOk)
                    allesGeldig = false;
            }

            return allesGeldig;
        }

        private bool ZijnAlleToelichtingenOk(ObservableCollection<Toelichting> toelichtingen)
        {
            if (_isDocent) return true;
            if (toelichtingen == null || !toelichtingen.Any()) return false;

            return toelichtingen.All(t =>
                !string.IsNullOrWhiteSpace(t.Tekst) &&
                t.GeselecteerdeOptie != "Toelichting gekoppeld aan...");
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

            var opties = _toelichtingService.GetBeschikbareOpties(parent.Toelichtingen);

            var gekozen = await Application.Current.MainPage.DisplayActionSheet(
                "Toelichting gekoppeld aan...",
                "Annuleren",
                null,
                opties.ToArray());

            if (!string.IsNullOrEmpty(gekozen) && gekozen != "Annuleren")
            {
                toelichting.GeselecteerdeOptie = gekozen;
                Notify(nameof(Beoordelingen));
            }
        }

        private void HookToelichtingen(BeoordelingItem item)
        {
            void Handler(object s, NotifyCollectionChangedEventArgs e)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                    Notify(nameof(Beoordelingen)));
            }

            item.Toelichtingen.CollectionChanged += Handler;
        }

        private async Task OpenLeertakenUrl(string url)
        {
            if (!string.IsNullOrWhiteSpace(url) &&
                Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                await Browser.Default.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
            }
        }

        private void Notify(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

