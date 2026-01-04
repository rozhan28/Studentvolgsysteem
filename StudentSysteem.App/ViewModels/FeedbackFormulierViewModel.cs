using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
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

        [ObservableProperty] private bool isZelfEvaluatie;

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

            IEnumerable<Proces> processen = _procesService.HaalAlleProcessenOp();
            IEnumerable<Vaardigheid> vaardigheden = _vaardigheidService.HaalAlleVaardighedenOp();
            IEnumerable<Prestatiedoel> prestatiedoelen = _prestatiedoelService.HaalAllePrestatiedoelenOp();

            // Stappen ophalen die bij het proces horen
            foreach (Proces proces in processen)
            {
                IEnumerable<Processtap> stappenVoorProces = _processtapService.HaalProcesstappenOpVoorProces(proces.Id);

                foreach (Processtap stap in stappenVoorProces)
                {
                    IEnumerable<Vaardigheid> vaardighedenVoorStap = vaardigheden.Where(v => v.ProcesstapId == stap.Id);

                    foreach (Vaardigheid vaardigheid in vaardighedenVoorStap)
                    {
                        Prestatiedoel prestatiedoel = prestatiedoelen.FirstOrDefault(p => p.Id == vaardigheid.PrestatiedoelId);
                        if (prestatiedoel == null) continue;

                        // Maak het item aan
                        BeoordelingItem item = new BeoordelingItem
                        {
                            ProcesNaam = proces.Naam,
                            ProcesstapNaam = stap.Naam,
                            VaardigheidNaam = vaardigheid.VaardigheidNaam,
                            VaardigheidBeschrijving = vaardigheid.VaardigheidBeschrijving,
                            HboiActiviteit = vaardigheid.HboiActiviteit,
                            LeertakenUrl = vaardigheid.LeertakenUrl,
                            PrestatiedoelId = prestatiedoel.Id,
                            PrestatiedoelBeschrijving = prestatiedoel.Beschrijving,
                            AiAssessmentScale = prestatiedoel.AiAssessmentScale,
                        };

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
                bool niveauOk = true;
                    //item.InOntwikkeling || item.IsOpNiveau || item.IsBovenNiveau;

                //item.IsPrestatieNiveauInvalid = !niveauOk;

                bool toelichtingOk =
                    _isDocent || ZijnAlleToelichtingenOk(item.Toelichtingen);

                item.IsToelichtingInvalid = !toelichtingOk;

                if (!niveauOk || !toelichtingOk)
                    allesGeldig = false;
            }

            return allesGeldig;
        }
        
        // Leertaken Url
        private async Task OpenLeertakenUrl(string url)
        {
            if (!string.IsNullOrWhiteSpace(url) &&
                Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                await Browser.Default.OpenAsync(url);
            }
        }

        // Toelichtingen
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
            if (item.Toelichtingen.Count >= _toelichtingService.BerekenMaxToelichtingen(item.PrestatiedoelId)) return;

            item.Toelichtingen.Add(_toelichtingService.MaakNieuweToelichting());
        }

        private void HookToelichtingen(BeoordelingItem item)
        {
            item.KanExtraToelichtingToevoegen =
                item.Toelichtingen.Count < _toelichtingService.BerekenMaxToelichtingen(item.PrestatiedoelId);

            item.Toelichtingen.CollectionChanged += (_, _) =>
            {
                item.KanExtraToelichtingToevoegen =
                    item.Toelichtingen.Count < _toelichtingService.BerekenMaxToelichtingen(item.PrestatiedoelId);
            };
        }

        private async Task ToonCriteriaKeuze(Toelichting toelichting)
        {
            if (toelichting == null) return;

            // Zoek eerst de parent (het BeoordelingItem) waar deze toelichting bij hoort
            BeoordelingItem parent = Beoordelingen.FirstOrDefault(delegate(BeoordelingItem b)
            {
                return b.Toelichtingen.Contains(toelichting);
            });
            if (parent == null) return;

            // Titels ophalen via de service
            List<string> beschikbareOpties =
                _toelichtingService.GetBeschikbareCriteria(parent.Toelichtingen, parent.PrestatiedoelId);
            string[] titels = beschikbareOpties.ToArray();

            string keuze = await Shell.Current.DisplayActionSheet("Koppel aan...", "Annuleren", null, titels);

            if (keuze != "Annuleren" && !string.IsNullOrWhiteSpace(keuze))
            {
                _toelichtingService.KoppelGekozenOptie(toelichting, keuze, parent.PrestatiedoelId);

                // UI-refresh trick
                int index = parent.Toelichtingen.IndexOf(toelichting);
                if (index != -1)
                {
                    parent.Toelichtingen[index] = new Toelichting
                    {
                        Tekst = toelichting.Tekst,
                        GeselecteerdeOptie = keuze,
                        Niveau = toelichting.Niveau
                    };
                }
            }
        }
    }
}