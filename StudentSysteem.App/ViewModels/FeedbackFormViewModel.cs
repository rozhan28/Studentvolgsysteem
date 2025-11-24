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

            PrestatieNiveaus = new List<string>
            {
                "In ontwikkeling",
                "Op niveau",
                "Boven niveau"
            };

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

        public List<string> PrestatieNiveaus { get; }
        public ObservableCollection<BeoordelingItem> Beoordelingen { get; }

        private string _statusmelding;
        public string StatusMelding
        {
            get => _statusmelding;
            set { _statusmelding = value; Notify(nameof(StatusMelding)); }
        }

        public ICommand SaveCommand { get; }

        private async Task SaveReflection()
        {
            StatusMelding = string.Empty;
            bool allValid = true;

            foreach (var item in Beoordelingen)
            {
                bool itemValid = ValidateItem(item);
                item.IsPrestatieNiveauInvalid = !itemValid;

                // toelichting is verplicht voor studenten
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

            //Data wordt opgeslagen
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


        private bool ValidateItem(BeoordelingItem item)
        {
            if (string.IsNullOrWhiteSpace(item.PrestatieNiveau))
                return false;

            return item.PrestatieNiveau switch
            {
                "In ontwikkeling" =>
                    !(item.OpNiveauDomeinWeerspiegelt ||
                      item.OpNiveauSyntaxCorrect ||
                      item.OpNiveauVastgelegd ||
                      item.BovenNiveauVolledig),

                "Op niveau" =>
                    (item.OpNiveauDomeinWeerspiegelt ||
                     item.OpNiveauSyntaxCorrect ||
                     item.OpNiveauVastgelegd),

                "Boven niveau" =>
                    item.BovenNiveauVolledig,

                _ => false
            };
        }

        private void Notify(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    public class BeoordelingItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Titel { get; set; }
        public string SubTitel { get; set; }
        public string Domein { get; set; }
        public string MakenDomeinmodel { get; set; }
        public string Beschrijving { get; set; }

        private string _toelichting;
        public string Toelichting
        {
            get => _toelichting;
            set { _toelichting = value; Notify(nameof(Toelichting)); }
        }

        private string _prestatieNiveau;
        public string PrestatieNiveau
        {
            get => _prestatieNiveau;
            set { _prestatieNiveau = value; Notify(nameof(PrestatieNiveau)); }
        }

        private bool _opNiveauDomeinWeerspiegelt;
        public bool OpNiveauDomeinWeerspiegelt
        {
            get => _opNiveauDomeinWeerspiegelt;
            set { _opNiveauDomeinWeerspiegelt = value; Notify(nameof(OpNiveauDomeinWeerspiegelt)); }
        }

        private bool _opNiveauSyntaxCorrect;
        public bool OpNiveauSyntaxCorrect
        {
            get => _opNiveauSyntaxCorrect;
            set { _opNiveauSyntaxCorrect = value; Notify(nameof(OpNiveauSyntaxCorrect)); }
        }

        private bool _opNiveauVastgelegd;
        public bool OpNiveauVastgelegd
        {
            get => _opNiveauVastgelegd;
            set { _opNiveauVastgelegd = value; Notify(nameof(OpNiveauVastgelegd)); }
        }

        private bool _bovenNiveauVolledig;
        public bool BovenNiveauVolledig
        {
            get => _bovenNiveauVolledig;
            set { _bovenNiveauVolledig = value; Notify(nameof(BovenNiveauVolledig)); }
        }

        private bool _isPrestatieNiveauInvalid;
        public bool IsPrestatieNiveauInvalid
        {
            get => _isPrestatieNiveauInvalid;
            set { _isPrestatieNiveauInvalid = value; Notify(nameof(IsPrestatieNiveauInvalid)); }
        }

        private bool _isToelichtingInvalid;
        public bool IsToelichtingInvalid
        {
            get => _isToelichtingInvalid;
            set { _isToelichtingInvalid = value; Notify(nameof(IsToelichtingInvalid)); }
        }

        private void Notify(string p) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
    }
}
