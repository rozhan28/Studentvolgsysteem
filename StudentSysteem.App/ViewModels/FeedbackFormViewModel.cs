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
                    PrestatieNiveau = item.CurrentLevel,
                    Toelichting = item.Toelichting,
                    Datum = DateTime.Now
                });
            }

            await _alertService.ShowAlertAsync("Succes", "Feedback succesvol opgeslagen!");
        }

        private bool ValidateItem(BeoordelingItem item)
        {
            // Minimaal 1 checkbox verplicht
            return item.InOntwikkeling ||
                   item.OpNiveauSyntaxCorrect ||
                   item.OpNiveauVastgelegd ||
                   item.OpNiveauDomeinWeerspiegelt ||
                   item.BovenNiveauVolledig;
        }

        private void Notify(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    public class BeoordelingItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Titel { get; set; }
        public string Domein { get; set; }
        public string MakenDomeinmodel { get; set; }
        public string Beschrijving { get; set; }

        private string _toelichting;
        public string Toelichting
        {
            get => _toelichting;
            set { _toelichting = value; Notify(nameof(Toelichting)); }
        }


        // In ontwikkeling checkbox
        private bool _inOntwikkeling;
        public bool InOntwikkeling
        {
            get => _inOntwikkeling;
            set { _inOntwikkeling = value; UpdateColor(); Notify(nameof(InOntwikkeling)); }
        }

        // Op niveau checkboxes
        private bool _opNiveauDomeinWeerspiegelt;
        public bool OpNiveauDomeinWeerspiegelt
        {
            get => _opNiveauDomeinWeerspiegelt;
            set { _opNiveauDomeinWeerspiegelt = value; UpdateColor(); Notify(nameof(OpNiveauDomeinWeerspiegelt)); }
        }

        private bool _opNiveauSyntaxCorrect;
        public bool OpNiveauSyntaxCorrect
        {
            get => _opNiveauSyntaxCorrect;
            set { _opNiveauSyntaxCorrect = value; UpdateColor(); Notify(nameof(OpNiveauSyntaxCorrect)); }
        }

        private bool _opNiveauVastgelegd;
        public bool OpNiveauVastgelegd
        {
            get => _opNiveauVastgelegd;
            set { _opNiveauVastgelegd = value; UpdateColor(); Notify(nameof(OpNiveauVastgelegd)); }
        }

        // Boven niveau checkbox
        private bool _bovenNiveauVolledig;
        public bool BovenNiveauVolledig
        {
            get => _bovenNiveauVolledig;
            set { _bovenNiveauVolledig = value; UpdateColor(); Notify(nameof(BovenNiveauVolledig)); }
        }

        // -----------------------

        private string _containerColor = "White";
        public string ContainerColor
        {
            get => _containerColor;
            set { _containerColor = value; Notify(nameof(ContainerColor)); }
        }

        public string CurrentLevel
        {
            get
            {
                if (BovenNiveauVolledig)
                    return "Boven niveau";

                if (OpNiveauDomeinWeerspiegelt && OpNiveauSyntaxCorrect && OpNiveauVastgelegd)
                    return "Op niveau";

                if (InOntwikkeling)
                    return "In ontwikkeling";

                return "";
            }
        }

        private void UpdateColor()
        {
            if (BovenNiveauVolledig)
            {
                ContainerColor = "#348000";
            }
            else if (OpNiveauDomeinWeerspiegelt && OpNiveauSyntaxCorrect && OpNiveauVastgelegd)
            {
                ContainerColor = "#68ca26"; 
            }
            else if (InOntwikkeling)
            {
                ContainerColor = "#2e95d4"; 
            }
            else
            {
                ContainerColor = "White"; 
            }

            Notify(nameof(ContainerColor));
            Notify(nameof(CurrentLevel));
        }

        // Validatie flags
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
