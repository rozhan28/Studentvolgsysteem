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

namespace StudentSysteem.App.ViewModels
{
    [QueryProperty(nameof(IsZelfEvaluatie), "isZelf")]
    public partial class FeedbackFormulierViewModel : BasisViewModel, INotifyPropertyChanged
    {
        private readonly IProcesService _procesService;
        private readonly IProcesstapService _processtapService;
        private readonly IPrestatiedoelService _prestatiedoelService;
        private readonly ICriteriumService _criteriumService;
        private readonly IFeedbackFormulierService _feedbackService;
        private readonly IToelichtingService _toelichtingService;
        private readonly IMeldingService _meldingService;
        private readonly IZelfEvaluatieService _zelfEvaluatieService;
        private readonly IVaardigheidService _vaardigheidService;

        private readonly bool _isDocent;

        public ObservableCollection<BeoordelingItem> Beoordelingen { get; } = new();

        private string _statusMelding;
        public string StatusMelding
        {
            get => _statusMelding;
            set
            {
                if (_statusMelding == value) return;
                _statusMelding = value;
                OnPropertyChanged(nameof(StatusMelding));
            }
        }

        public ICommand OptiesCommand { get; }
        
        [ObservableProperty]
        private bool isZelfEvaluatie;

        public ICommand OpslaanCommand { get; }
        public ICommand VoegExtraToelichtingToeCommand { get; }
        public ICommand LeertakenCommand { get; }

        public FeedbackFormulierViewModel(
            IProcesService procesService,
            IProcesstapService processtapService,
            IVaardigheidService vaardigheidService,
            IPrestatiedoelService prestatiedoelService,
            ICriteriumService criteriumService,
            IFeedbackFormulierService feedbackService,
            IToelichtingService toelichtingService,
            IMeldingService meldingService,
            IZelfEvaluatieService zelfEvaluatieService,
            GlobaleViewModel globaal)
        {
            _procesService = procesService;
            _processtapService = processtapService;
            _vaardigheidService = vaardigheidService;
            _prestatiedoelService = prestatiedoelService;
            _criteriumService = criteriumService;
            _feedbackService = feedbackService;
            _toelichtingService = toelichtingService;
            _meldingService = meldingService;
            _zelfEvaluatieService = zelfEvaluatieService;

            _isDocent = globaal.IngelogdeGebruiker?.Rol == Role.Docent;

            OpslaanCommand = new Command(async () => await BewaarEvaluatieAsync());
            VoegExtraToelichtingToeCommand = new Command<BeoordelingItem>(VoegExtraToelichtingToe);
            LeertakenCommand = new Command<string>(async url => await OpenLeertakenUrl(url));
            OptiesCommand = new Command<Toelichting>(async t => await ToonCriteriaKeuze(t));

            Task.Run(InitialiseerPaginaAsync);
        }

        private async Task InitialiseerPaginaAsync()
        {
            try
            {
                LaadStructuur();
                MainThread.BeginInvokeOnMainThread(() => UpdatePaginaTitel());
            }
            catch (Exception ex)
            {
                StatusMelding = "Fout bij laden formulier.";
                Debug.WriteLine(ex);
            }
        }

        public void UpdatePaginaTitel()
        {
            Titel = IsZelfEvaluatie ? "Zelfevaluatieformulier" : "Feedbackformulier";
        }

        private void LaadStructuur()
        {
            MainThread.BeginInvokeOnMainThread(() => Beoordelingen.Clear());
            
            IEnumerable<Proces> processen = _procesService.HaalOp();
            IEnumerable<Vaardigheid> vaardigheden = _vaardigheidService.HaalAlleVaardighedenOp();
            IEnumerable<Prestatiedoel> prestatiedoelen = _prestatiedoelService.HaalAllePrestatiedoelenOp();
            IEnumerable<Processtap> alleStappen = _processtapService.HaalAlleProcesstappenOp(); // Of per proces ophalen

            // Stappen ophalen die bij het proces horen
            foreach (Proces proces in processen)
            {
                IEnumerable<Processtap> stappenVoorProces = _processtapService.HaalProcesstappenOpVoorProces(proces.Id);

                foreach (Processtap stap in stappenVoorProces)
                {
                    IEnumerable<Vaardigheid> vaardighedenVoorStap = vaardigheden.Where(v => v.ProcesstapId == stap.Id);

                    foreach (Vaardigheid vaardigheid in vaardighedenVoorStap)
                    {
                        Prestatiedoel doel = prestatiedoelen.FirstOrDefault(d => d.Id == vaardigheid.PrestatiedoelId);
                        if (doel == null) continue;

                        // Maak het item aan
                        BeoordelingItem item = new BeoordelingItem
                        {
                            Proces = proces.Naam,
                            Processtap = stap.Naam,
                            Vaardigheid = vaardigheid.Naam,
                            Beschrijving = vaardigheid.Beschrijving,
                            HboiActiviteit = vaardigheid.HboiActiviteit,
                            LeertakenUrl = vaardigheid.LeertakenUrl,
                            PrestatiedoelId = doel.Id,
                            PrestatiedoelBeschrijving = doel.Beschrijving,
                            AiAssessmentScale = doel.AiAssessmentScale,
                            GeselecteerdNiveau = Niveauaanduiding.NietIngeleverd
                        };

                        // Criteria vullen
                        item.OpNiveauCriteria = new ObservableCollection<Criterium>(
                            _criteriumService.HaalCriteriaOpVoorPrestatiedoel(doel.Id, "Op niveau"));

                        item.BovenNiveauCriteria = new ObservableCollection<Criterium>(
                            _criteriumService.HaalCriteriaOpVoorPrestatiedoel(doel.Id, "Boven niveau"));

                        // Toelichtingen initialiseren
                        item.Toelichtingen.Add(_toelichtingService.MaakNieuweToelichting());

                        HookToelichtingen(item);

                        // Geef terug aan UI
                        MainThread.BeginInvokeOnMainThread(() => Beoordelingen.Add(item));
                    }
                }
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
                int evaluatieId = _zelfEvaluatieService.VoegToe(new ZelfEvaluatie
                {
                    StudentId = 1,
                    PrestatieNiveau = "Ingevuld"
                });

                foreach (BeoordelingItem item in Beoordelingen)
                {
                    if (item.Toelichtingen.Any())
                        _feedbackService.SlaToelichtingenOp(
                            item.Toelichtingen.ToList(), evaluatieId);
                }

                await _meldingService.ToonMeldingAsync(
                    "Succes",
                    IsZelfEvaluatie
                        ? "Zelfevaluatie opgeslagen."
                        : "Feedback opgeslagen.");
            }
            catch (Exception ex)
            {
                StatusMelding = $"Opslaan mislukt: {ex.Message}";
            }
        }

        private bool ValideerBeoordelingen()
        {
            bool allesGeldig = true;

            foreach (BeoordelingItem item in Beoordelingen)
            {
                bool niveauOk =
                    item.InOntwikkeling || item.IsOpNiveau || item.IsBovenNiveau;

                item.IsPrestatieNiveauInvalid = !niveauOk;

                bool toelichtingOk =
                    _isDocent || ZijnAlleToelichtingenOk(item.Toelichtingen);

                item.IsToelichtingInvalid = !toelichtingOk;

                if (!niveauOk || !toelichtingOk)
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

        private void VoegExtraToelichtingToe(BeoordelingItem item)
        {
            if (item == null) return;
            if (item.Toelichtingen.Count >= _toelichtingService.TotaleOptiesCount) return;

            item.Toelichtingen.Add(_toelichtingService.MaakNieuweToelichting());
        }

        private void HookToelichtingen(BeoordelingItem item)
        {
            item.KanExtraToelichtingToevoegen =
                item.Toelichtingen.Count < _toelichtingService.TotaleOptiesCount;

            item.Toelichtingen.CollectionChanged += (_, _) =>
            {
                item.KanExtraToelichtingToevoegen =
                    item.Toelichtingen.Count < _toelichtingService.TotaleOptiesCount;
            };
        }

        private async Task OpenLeertakenUrl(string url)
        {
            if (!string.IsNullOrWhiteSpace(url) &&
                Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                await Browser.Default.OpenAsync(url);
            }
        }

        private async Task ToonCriteriaKeuze(Toelichting toelichting)
        {
            if (toelichting == null) return;

            BeoordelingItem parent = Beoordelingen
                .FirstOrDefault(b => b.Toelichtingen.Contains(toelichting));

            if (parent == null) return;

            List<string> opties = parent.OpNiveauCriteria
                .Concat(parent.BovenNiveauCriteria)
                .Select(c => c.DisplayNaam)
                .Distinct()
                .ToList();

            opties.Insert(0, "Algemeen");

            string keuze = await Shell.Current.DisplayActionSheet(
                "Koppel toelichting aan criterium",
                "Annuleren",
                null,
                opties.ToArray());

            if (keuze == "Annuleren" || string.IsNullOrWhiteSpace(keuze))
                return;

            toelichting.GeselecteerdeOptie = keuze;
        }
    }
}