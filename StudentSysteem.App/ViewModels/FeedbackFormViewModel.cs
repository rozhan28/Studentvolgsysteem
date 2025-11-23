using StudentVolgSysteem.Core.Models;
using StudentVolgSysteem.Core.Services;
using System.ComponentModel;
using System.Windows.Input;

namespace StudentSysteem.App.ViewModels
{
    public class FeedbackFormViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ISelfReflectionService _service;
        private readonly bool _isDocent;

        public FeedbackFormViewModel(ISelfReflectionService service, bool isDocent)
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
        }

        public List<string> PrestatieNiveaus { get; }

        public bool IsLeeruitkomstInvalid { get; set; }
        public bool IsPrestatieNiveauInvalid { get; set; }
        public bool IsToelichtingInvalid { get; set; }

        private string _leeruitkomst;
        public string Leeruitkomst
        {
            get => _leeruitkomst;
            set { _leeruitkomst = value; Notify(nameof(Leeruitkomst)); }
        }

        private string _prestatieNiveau;
        public string PrestatieNiveau
        {
            get => _prestatieNiveau;
            set { _prestatieNiveau = value; Notify(nameof(PrestatieNiveau)); }
        }

        private string _toelichting;
        public string Toelichting
        {
            get => _toelichting;
            set { _toelichting = value; Notify(nameof(Toelichting)); }
        }

        private string _statusmelding;
        public string StatusMelding
        {
            get => _statusmelding;
            set { _statusmelding = value; Notify(nameof(StatusMelding)); }
        }

        public ICommand SaveCommand { get; }

        private async Task SaveReflection()
        {
            // Reset kleuren
            IsLeeruitkomstInvalid = false;
            IsPrestatieNiveauInvalid = false;
            IsToelichtingInvalid = false;

            Notify(nameof(IsLeeruitkomstInvalid));
            Notify(nameof(IsPrestatieNiveauInvalid));
            Notify(nameof(IsToelichtingInvalid));

            bool isValid = true;

            // --- Validatie ---
            if (string.IsNullOrWhiteSpace(Leeruitkomst))
            {
                IsLeeruitkomstInvalid = true;
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(PrestatieNiveau))
            {
                IsPrestatieNiveauInvalid = true;
                isValid = false;
            }

            if (_isDocent && string.IsNullOrWhiteSpace(Toelichting))
            {
                IsToelichtingInvalid = true;
                isValid = false;
            }

            Notify(nameof(IsLeeruitkomstInvalid));
            Notify(nameof(IsPrestatieNiveauInvalid));
            Notify(nameof(IsToelichtingInvalid));

            if (!isValid)
            {
                StatusMelding = "Vul alle verplichte velden in.";
                return;
            }

            // Opslaan
            var reflection = new SelfReflection
            {
                StudentId = 1,
                Leeruitkomst = Leeruitkomst,
                PrestatieNiveau = PrestatieNiveau,
                Toelichting = Toelichting,
                Datum = DateTime.Now
            };

            _service.Add(reflection);

            await Application.Current.MainPage.DisplayAlert(
                "Succes",
                "Feedback succesvol opgeslagen!",
                "OK"
            );

            await Application.Current.MainPage.Navigation.PopAsync();

            ClearFields();
        }

        private void ClearFields()
        {
            Leeruitkomst = string.Empty;
            PrestatieNiveau = null;
            Toelichting = string.Empty;

            IsLeeruitkomstInvalid = false;
            IsPrestatieNiveauInvalid = false;
            IsToelichtingInvalid = false;

            Notify(nameof(IsLeeruitkomstInvalid));
            Notify(nameof(IsPrestatieNiveauInvalid));
            Notify(nameof(IsToelichtingInvalid));
        }

        private void Notify(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
