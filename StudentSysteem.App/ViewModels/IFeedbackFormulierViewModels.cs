using StudentSysteem.App.Models;
using StudentSysteem.Core.Interfaces.Services;
using StudentVolgSysteem.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentSysteem.App.ViewModels
{
    public class FeedbackFormulierViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IZelfEvaluatieService _zelfreflectieService;
        private readonly INavigatieService _navigatieService;
        private readonly IMeldingService _meldingService;
        private readonly IFeedbackFormulierService _feedbackFormulierService;
        private readonly bool _isDocent;

        public List<string> Opties { get; } = new List<string>
        {
            "Algemeen",
            "Criteria 1",
            "Criteria 2",
            "Criteria 3"
        };

        public FeedbackFormulierViewModel(
            IZelfEvaluatieService zelfreflectieService,
            INavigatieService navigatieService,
            IMeldingService meldingService,
            IFeedbackFormulierService feedbackFormulierService,
            bool isDocent = false)
        {
            _zelfreflectieService = zelfreflectieService;
            _navigatieService = navigatieService;
            _meldingService = meldingService;
            _feedbackFormulierService = feedbackFormulierService;
            _isDocent = isDocent;

            OpslaanCommand = new Command(async () => await BewaarReflectie());

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

        // ⭐ Lijst van beoordelingsitems
        public ObservableCollection<BeoordelingItem> Beoordelingen { get; }

        private string _statusmelding;
        public string StatusMelding
        {
            get => _statusmelding;
            set { _statusmelding = value; Notify(nameof(StatusMelding)); }
        }

        public ICommand OpslaanCommand { get; }

        // ⭐ Opslaan van feedback
        private async Task BewaarReflectie()
        {
            StatusMelding = string.Empty;
            bool allesGeldig = true;

            foreach (var item in Beoordelingen)
            {
                bool itemGeldig = ValideerItem(item);
                item.IsPrestatieNiveauInvalid = !itemGeldig;

                item.IsToelichtingInvalid =
                    string.IsNullOrWhiteSpace(item.Toelichting) && !_isDocent;

                if (!itemGeldig || item.IsToelichtingInvalid)
                    allesGeldig = false;
            }

            if (!allesGeldig)
            {
                StatusMelding = "Controleer alle velden a.u.b.";
                return;
            }

            foreach (var item in Beoordelingen)
            {
                if (!string.IsNullOrWhiteSpace(item.Toelichting))
                {
                    try
                    {
                        _feedbackFormulierService.SlaToelichtingOp(item.Toelichting, 1); // studentId = 1
                    }
                    catch (Exception ex)
                    {
                        StatusMelding = "Fout bij opslaan: " + ex.Message;
                        return;
                    }
                }
            }

            await _meldingService.ToonMeldingAsync("Succes", "Feedback (toelichting) is succesvol opgeslagen!");
        }

        // ⭐ Validatie per item
        private bool ValideerItem(BeoordelingItem item)
        {
            return item.InOntwikkeling ||
                   item.OpNiveauSyntaxCorrect ||
                   item.OpNiveauVastgelegd ||
                   item.OpNiveauDomeinWeerspiegelt ||
                   item.BovenNiveauVolledig;
        }

        private void Notify(string eigenschap) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(eigenschap));
    }
}
