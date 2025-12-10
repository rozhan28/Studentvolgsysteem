using StudentSysteem.Core.Models;
using StudentSysteem.Core.Interfaces.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using StudentSysteem.Core.Interfaces.Repository;

namespace StudentSysteem.App.ViewModels
{
    public class FeedbackFormulierViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly INavigatieService _navigatieService;
        private readonly IMeldingService _meldingService;
        private readonly IFeedbackFormulierService _feedbackService;
        private readonly bool _isDocent;

        public ObservableCollection<BeoordelingItem> Beoordelingen { get; }

        public List<string> Opties { get; } = new()
        {
            "Algemeen",
            "Criteria 1",
            "Criteria 2",
            "Criteria 3"
        };

        private string _statusMelding;
        public string StatusMelding
        {
            get => _statusMelding;
            set { _statusMelding = value; OnPropertyChanged(nameof(StatusMelding)); }
        }

        public ICommand OpslaanCommand { get; }

        public FeedbackFormulierViewModel(
            IZelfEvaluatieService zelfevaluatieService,
            INavigatieService navigatieService,
            IMeldingService meldingService,
            IFeedbackFormulierService feedbackService,
            bool isDocent = false)
        {
            _navigatieService = navigatieService;
            _meldingService = meldingService;
            _feedbackService = feedbackService;
            _isDocent = isDocent;

            OpslaanCommand = new Command(async () => await BewaarReflectieAsync());

            // Startdata voor beoordelingitems
            Beoordelingen = new ObservableCollection<BeoordelingItem>
{
                new BeoordelingItem {
                    Titel = "Maken domeinmodel | Definiëren probleemdomein | Requirementsanalyseproces | Analyseren",
                    Vaardigheid = "Maken domeinmodel",
                    Beschrijving = "Het maken van een domeinmodel volgens een UML klassendiagram"
                },
                new BeoordelingItem {
                    Titel = "Bestuderen probleemstelling | Definiëren probleemdomein | Requirementsanalyseproces | Analyseren",
                    Vaardigheid = "Bestuderen probleemstelling",
                    Beschrijving = "Het probleem achterhalen"
                },
                new BeoordelingItem {
                    Titel = "Beschrijven stakeholders | Verzamelen requirement | Requirementsanalyseproces | Analyseren",
                    Vaardigheid = "Beschrijven stakeholders",
                    Beschrijving = "Het maken van een stakeholderanalyse"
                }
            };

        
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
                foreach (var item in Beoordelingen)
                {
                    if (!string.IsNullOrWhiteSpace(item.Toelichting))
                        _feedbackService.SlaToelichtingOp(item.Toelichting, 1);
                }

                await _meldingService.ToonMeldingAsync("Succes", "Toelichting is opgeslagen!");
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

                bool toelichtingOk = !(string.IsNullOrWhiteSpace(item.Toelichting) && !_isDocent);
                item.IsToelichtingInvalid = !toelichtingOk;

                if (!prestatieOk || !toelichtingOk)
                    allesGeldig = false;
            }

            return allesGeldig;
        }

        private static bool ValideerPrestatieNiveau(BeoordelingItem item)
        {
            return item.InOntwikkeling ||
                   item.OpNiveauSyntaxCorrect ||
                   item.OpNiveauVastgelegd ||
                   item.OpNiveauDomeinWeerspiegelt ||
                   item.BovenNiveauVolledig;
        }

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}