using StudentVolgSysteem.Core.Models;
using StudentVolgSysteem.Core.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace StudentSysteem.App.ViewModels
{
    public class FeedbackFormViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ISelfReflectionService _service;
        private readonly bool _isDocent;

        public FeedbackFormViewModel(ISelfReflectionService service, bool isDocent = false)
        {
            _service = service;
            _isDocent = isDocent;

            SaveCommand = new Command(async () => await SaveReflection());

            PrestatieNiveaus = new List<string>
            {
                "In ontwikkeling",
                "Op niveau",
                "Boven niveau"
            };

            // beoordelingsitems
            Beoordelingen = new ObservableCollection<BeoordelingItem>
            {
                new BeoordelingItem { Titel = "Maken domeinmodel", Beschrijving = "" },
                new BeoordelingItem { Titel = "Analyseren proces", Beschrijving = "" },
                new BeoordelingItem { Titel = "Ontwikkelen ontwerp", Beschrijving = "" }
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
                bool itemValid = true;

                // --- Prestatie niveau validatie ---
                if (string.IsNullOrWhiteSpace(item.PrestatieNiveau))
                    itemValid = false;
                else if (item.PrestatieNiveau == "Op niveau" &&
                         !item.OpNiveauDomeinWeerspiegelt &&
                         !item.OpNiveauSyntaxCorrect &&
                         !item.OpNiveauVastgelegd)
                    itemValid = false;
                else if (item.PrestatieNiveau == "Boven niveau" && !item.BovenNiveauVolledig)
                    itemValid = false;

                item.IsPrestatieNiveauInvalid = !itemValid;
                if (!itemValid) allValid = false;

                // --- Toelichting validatie ---
                item.IsToelichtingInvalid = string.IsNullOrWhiteSpace(item.Toelichting) && _isDocent;
                if (item.IsToelichtingInvalid) allValid = false;
            }

            if (!allValid)
            {
                StatusMelding = "Controleer alle velden a.u.b.";
                return;
            }

            // Opslaan
            foreach (var item in Beoordelingen)
            {
                _service.Add(new SelfReflection
                {
                    StudentId = 1,
                    PrestatieNiveau = item.PrestatieNiveau,
                    Toelichting = item.Toelichting,
                    Datum = DateTime.Now
                });
            }

            // Popup 
            await Application.Current.MainPage.DisplayAlert("Succes", "Alle feedback succesvol opgeslagen!", "OK");
            await Application.Current.MainPage.Navigation.PopAsync();

            ClearFields();
        }

        private void ClearFields()
        {
            foreach (var item in Beoordelingen)
            {
                item.PrestatieNiveau = null;
                item.Toelichting = string.Empty;
                item.OpNiveauDomeinWeerspiegelt = false;
                item.OpNiveauSyntaxCorrect = false;
                item.OpNiveauVastgelegd = false;
                item.BovenNiveauVolledig = false;
                item.IsPrestatieNiveauInvalid = false;
                item.IsToelichtingInvalid = false;
            }

            StatusMelding = string.Empty;
        }

        private void Notify(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    public class BeoordelingItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _titel;
        public string Titel { get => _titel; set { _titel = value; Notify(nameof(Titel)); } }

        private string _beschrijving;
        public string Beschrijving { get => _beschrijving; set { _beschrijving = value; Notify(nameof(Beschrijving)); } }

        private string _toelichting;
        public string Toelichting { get => _toelichting; set { _toelichting = value; Notify(nameof(Toelichting)); } }

        private string _prestatieNiveau;
        public string PrestatieNiveau { get => _prestatieNiveau; set { _prestatieNiveau = value; Notify(nameof(PrestatieNiveau)); } }

        private bool _opNiveauDomeinWeerspiegelt;
        public bool OpNiveauDomeinWeerspiegelt { get => _opNiveauDomeinWeerspiegelt; set { _opNiveauDomeinWeerspiegelt = value; Notify(nameof(OpNiveauDomeinWeerspiegelt)); } }

        private bool _opNiveauSyntaxCorrect;
        public bool OpNiveauSyntaxCorrect { get => _opNiveauSyntaxCorrect; set { _opNiveauSyntaxCorrect = value; Notify(nameof(OpNiveauSyntaxCorrect)); } }

        private bool _opNiveauVastgelegd;
        public bool OpNiveauVastgelegd { get => _opNiveauVastgelegd; set { _opNiveauVastgelegd = value; Notify(nameof(OpNiveauVastgelegd)); } }

        private bool _bovenNiveauVolledig;
        public bool BovenNiveauVolledig { get => _bovenNiveauVolledig; set { _bovenNiveauVolledig = value; Notify(nameof(BovenNiveauVolledig)); } }

        // Validatie flags
        private bool _isPrestatieNiveauInvalid;
        public bool IsPrestatieNiveauInvalid { get => _isPrestatieNiveauInvalid; set { _isPrestatieNiveauInvalid = value; Notify(nameof(IsPrestatieNiveauInvalid)); } }

        private bool _isToelichtingInvalid;
        public bool IsToelichtingInvalid { get => _isToelichtingInvalid; set { _isToelichtingInvalid = value; Notify(nameof(IsToelichtingInvalid)); } }

        private void Notify(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
