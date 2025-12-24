using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace StudentSysteem.App.ViewModels
{
    public class FeedbackFormulierViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //TODO: Clean properties naar models in .core
        public string Titel { get; set; }             
        public string PrestatiedoelBeschrijving { get; set; }
        public string AiAssessmentScale { get; set; }
        
        
        private readonly bool _isDocent;


        private readonly INavigatieService _navigatieService;
        private readonly IMeldingService _meldingService;
        private readonly IFeedbackFormulierService _feedbackService;
        private readonly IPrestatiedoelService _prestatiedoelService;
        private readonly ZelfEvaluatieViewModel _zelfEvaluatieViewModel;
        private readonly IVaardigheidService _vaardigheidService;
        
        public ICommand LeertakenCommand { get; }


        private ObservableCollection<BeoordelingItem> _beoordelingen;
        public ObservableCollection<BeoordelingItem> Beoordelingen
        {
            get => _beoordelingen;
            set
            {
                _beoordelingen = value;
                OnPropertyChanged(nameof(Beoordelingen));
            }
        }

        private string _statusMelding;
        public string StatusMelding
        {
            get => _statusMelding;
            set
            {
                _statusMelding = value;
                OnPropertyChanged(nameof(StatusMelding));
            }
        }

        public ICommand OpslaanCommand { get; }

        public FeedbackFormulierViewModel(
            IZelfEvaluatieService zelfEvaluatieService,
            INavigatieService navigatieService,
            IMeldingService meldingService,
            IFeedbackFormulierService feedbackService,
            IPrestatiedoelService prestatiedoelService,
            IVaardigheidService vaardigheidService,
            bool isDocent = false)
        {
            _zelfEvaluatieViewModel = new ZelfEvaluatieViewModel(zelfEvaluatieService);
            _navigatieService = navigatieService;
            _meldingService = meldingService;
            _feedbackService = feedbackService;
            _prestatiedoelService = prestatiedoelService;
            _vaardigheidService = vaardigheidService;
            _isDocent = isDocent;
            
            Task.Run(async () => await InitialiseerPaginaAsync());
            
            OpslaanCommand = new Command(async () => await BewaarReflectieAsync());
            LeertakenCommand = new Command<string>(async (url) => await OpenLeertakenUrl(url));
        }

        // Toegevoegd voor TC2-01.1 - voorkomt crashen
        // Zorgt ervoor dat indien de database niet beschikbaar is, de applicatie niet crasht
        private async Task InitialiseerPaginaAsync()
        {
            try 
            {
                LaadPrestatiedoelen();
            }
            catch (Exception ex)
            {
                StatusMelding = "Databasefout: De prestatiedoelen konden niet worden geladen.";
                System.Diagnostics.Debug.WriteLine($"Fout: {ex.Message}");
            }
        }
        
        private void LaadPrestatiedoelen()
        {
            IEnumerable<Prestatiedoel> doelen = _prestatiedoelService.HaalPrestatiedoelenOp();
            IEnumerable<Vaardigheid> vaardigheden = _vaardigheidService.HaalAlleVaardighedenOp();
            
            List<BeoordelingItem> items = doelen.Select(delegate(Prestatiedoel d)
            {
                Vaardigheid gekoppeldeVaardigheid = vaardigheden.FirstOrDefault(v => v.Prestatiedoel_id == d.Id);

                return new BeoordelingItem
                {
                    PrestatiedoelId = d.Id,
                    Titel = $"Prestatiedoel {d.Id}",
                    PrestatiedoelBeschrijving = d.Beschrijving,
                    AiAssessmentScale = d.AiAssessmentScale,

                    Vaardigheid = gekoppeldeVaardigheid?.VaardigheidNaam ?? "Geen vaardigheid gekoppeld",
                    LeertakenUrl = gekoppeldeVaardigheid?.LeertakenUrl,
                    HboiActiviteit = gekoppeldeVaardigheid?.HboiActiviteit,
                    Beschrijving = gekoppeldeVaardigheid?.VaardigheidBeschrijving
                };
            }).ToList();

            Beoordelingen = new ObservableCollection<BeoordelingItem>(items);
        }

        private async Task BewaarReflectieAsync()
        {
            StatusMelding = string.Empty;

            if (!ValideerBeoordelingen())
            {
                StatusMelding = "Controleer alle velden a.u.b.";
                return;
            }

            try
            {
                // 1️⃣ Zelfevaluatie opslaan
                int zelfEvaluatieId = _zelfEvaluatieViewModel.SlaZelfEvaluatieOp(1);

                // 2️⃣ Toelichtingen opslaan
                foreach (BeoordelingItem item in Beoordelingen)
                {
                    if (!string.IsNullOrWhiteSpace(item.Toelichting))
                    {
                        _feedbackService.SlaToelichtingOp(
                            item.Toelichting,
                            zelfEvaluatieId
                        );
                    }
                }

                await _meldingService.ToonMeldingAsync(
                    "Succes",
                    "Zelfevaluatie en feedback zijn opgeslagen!");
            }
            catch (Exception ex)
            {
                StatusMelding = $"Fout bij opslaan: {ex.Message}";
            }
        }

        private bool ValideerBeoordelingen()
        {
            bool allesGeldig = true;

            foreach (BeoordelingItem item in Beoordelingen)
            {
                bool prestatieOk = ValideerPrestatieNiveau(item);
                item.IsPrestatieNiveauInvalid = !prestatieOk;

                bool toelichtingOk =
                    !(string.IsNullOrWhiteSpace(item.Toelichting) && !_isDocent);

                item.IsToelichtingInvalid = !toelichtingOk;

                if (!prestatieOk || !toelichtingOk)
                    allesGeldig = false;
            }

            return allesGeldig;
        }

        private static bool ValideerPrestatieNiveau(BeoordelingItem item)
        {
            return item.InOntwikkeling
                || item.OpNiveauSyntaxCorrect
                || item.OpNiveauVastgelegd
                || item.OpNiveauDomeinWeerspiegelt
                || item.BovenNiveauVolledig;
        }

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        
        private async Task OpenLeertakenUrl(string url)
        {
            if (!string.IsNullOrWhiteSpace(url) && Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                await Browser.Default.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
            }
        }
    }
}
