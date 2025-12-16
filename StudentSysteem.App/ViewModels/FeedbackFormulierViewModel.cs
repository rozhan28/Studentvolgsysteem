using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;
using StudentSysteem.Core.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace StudentSysteem.App.ViewModels
{
    public class FeedbackFormulierViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Titel { get; set; }             
        public string PrestatiedoelBeschrijving { get; set; } 


        private readonly INavigatieService _navigatieService;
        private readonly IMeldingService _meldingService;
        private readonly IFeedbackFormulierService _feedbackService;
        private readonly IPrestatiedoelService _prestatiedoelService;
        private readonly ZelfEvaluatieViewModel _zelfEvaluatieViewModel;
        private readonly ICriteriumService _criteriumService;
        private readonly bool _isDocent;

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
            ICriteriumService criteriumService,
            bool isDocent = false)
        {
            _zelfEvaluatieViewModel = new ZelfEvaluatieViewModel(zelfEvaluatieService);
            _navigatieService = navigatieService;
            _meldingService = meldingService;
            _feedbackService = feedbackService;
            _prestatiedoelService = prestatiedoelService;
            _criteriumService = criteriumService;
            _isDocent = isDocent;

            OpslaanCommand = new Command(async () => await BewaarReflectieAsync());

            LaadPrestatiedoelen();
        }

        private void LaadPrestatiedoelen()
        {
            var doelen = _prestatiedoelService.HaalPrestatiedoelenOp();

            var lijst = new ObservableCollection<BeoordelingItem>();

            foreach (var d in doelen)
            {
                var item = new BeoordelingItem
                {
                    PrestatiedoelId = d.Id,
                    Titel = $"Prestatiedoel {d.Id}",
                    PrestatiedoelBeschrijving = d.Beschrijving
                };

                foreach (var criterium in _criteriumService.HaalOpNiveauCriteriaOp())
                {
                    item.OpNiveauCriteria.Add(criterium);
                }

                foreach (var criterium in _criteriumService.HaalBovenNiveauCriteriaOp())
                {
                    item.BovenNiveauCriteria.Add(criterium);
                }

                lijst.Add(item);
            }

            Beoordelingen = lijst;
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
                // Zelfevaluatie opslaan
                int zelfEvaluatieId = _zelfEvaluatieViewModel.SlaZelfEvaluatieOp(1);

                // Toelichtingen opslaan
                foreach (var item in Beoordelingen)
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

            foreach (var item in Beoordelingen)
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
                || item.IsOpNiveau
                || item.IsBovenNiveau;
        }


        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
