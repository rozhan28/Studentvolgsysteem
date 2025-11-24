using StudentSysteem.App.Models;
using StudentVolgSysteem.Core.Models;
using StudentVolgSysteem.App.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using StudentSysteem.Core.Interfaces.Services;

namespace StudentSysteem.App.ViewModels
{
    public class FeedbackFormViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ISelfReflectionService _reflectionService;
        private readonly INavigationService _navigationService;
        private readonly IAlertService _alertService;
        private readonly bool _isDocent;

        public FeedbackFormViewModel(
            ISelfReflectionService reflectionService,
            INavigationService navigationService,
            IAlertService alertService,
            bool isDocent = false)
        {
            _reflectionService = reflectionService;
            _navigationService = navigationService;
            _alertService = alertService;
            _isDocent = isDocent;

            SaveCommand = new Command(async () => await SaveReflection());

            // ⭐ STARTDATA
            Beoordelingen = new ObservableCollection<BeoordelingItem>
            {
                new BeoordelingItem {
                    Titel = "Requirementsanalyseproces – Definiëren probleemdomein",
                    Domein = "Analyseren",
                    MakenDomeinmodel = "Maken domeinmodel",
                    Beschrijving = "Het maken van een domeinmodel volgens een UML klassendiagram"
                },
                new BeoordelingItem {
                    Titel = "Requirementsanalyseproces – Definiëren probleemdomein",
                    Domein = "Analyseren",
                    MakenDomeinmodel = "Bestuderen probleemstelling",
                    Beschrijving = "Het probleem achterhalen"
                },
                new BeoordelingItem {
                    Titel = "Requirementsanalyseproces – Verzamelen requirements",
                    Domein = "Analyseren",
                    MakenDomeinmodel = "Beschrijven stakeholders",
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

        public ICommand SaveCommand { get; }

        // ⭐ Opslaan feedback
        private async Task SaveReflection()
        {
            StatusMelding = string.Empty;
            bool allValid = true;

            foreach (var item in Beoordelingen)
            {
                bool itemValid = ValidateItem(item);
                item.IsPrestatieNiveauInvalid = !itemValid;

                item.IsToelichtingInvalid =
                    string.IsNullOrWhiteSpace(item.Toelichting) && !_isDocent;

                if (!itemValid || item.IsToelichtingInvalid)
                    allValid = false;
            }

            if (!allValid)
            {
                StatusMelding = "Controleer alle velden a.u.b.";
                return;
            }

            foreach (var item in Beoordelingen)
            {
                _reflectionService.Add(new SelfReflection
                {
                    StudentId = 1,
                    PrestatieNiveau = item.PrestatieNiveau,
                    Toelichting = item.Toelichting,
                    Datum = DateTime.Now
                });
            }

            await _alertService.ShowAlertAsync("Succes", "Feedback succesvol opgeslagen!");
        }

        // ⭐ Validatie per item
        private bool ValidateItem(BeoordelingItem item)
        {
            return item.InOntwikkeling ||
                   item.OpNiveauSyntaxCorrect ||
                   item.OpNiveauVastgelegd ||
                   item.OpNiveauDomeinWeerspiegelt ||
                   item.BovenNiveauVolledig;
        }

        private void Notify(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
