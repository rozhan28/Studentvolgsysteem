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

        public FeedbackFormViewModel(ISelfReflectionService service)
        {
            _service = service;
            SaveCommand = new Command(async () => await SaveReflection());

            PrestatieNiveaus = new List<string>
            {
                "In ontwikkeling",
                "Op niveau",
                "Boven niveau"
            };
        }

        public List<string> PrestatieNiveaus { get; }

        private bool _isLeeruitkomstInvalid;
        public bool IsLeeruitkomstInvalid
        {
            get => _isLeeruitkomstInvalid;
            set { _isLeeruitkomstInvalid = value; Notify(nameof(IsLeeruitkomstInvalid)); }
        }

        private bool _isPrestatieNiveauInvalid;
        public bool IsPrestatieNiveauInvalid
        {
            get => _isPrestatieNiveauInvalid;
            set { _isPrestatieNiveauInvalid = value; Notify(nameof(IsPrestatieNiveauInvalid)); }
        }

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
            IsLeeruitkomstInvalid = false;
            IsPrestatieNiveauInvalid = false;

            bool isValid = true;

            // Validatie
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

            if (!isValid)
            {
                StatusMelding = "Vul alle verplichte velden in.";
                return;
            }

            var reflection = new SelfReflection
            {
                StudentId = 1,
                Leeruitkomst = Leeruitkomst,
                PrestatieNiveau = PrestatieNiveau,
                Toelichting = Toelichting,
                Datum = DateTime.Now
            };

            _service.Add(reflection);
            StatusMelding = "Feedback opgeslagen.";

            await Application.Current.MainPage.DisplayAlert(
                "Succes",
                "Feedback succesvol opgeslagen!",
                "OK"
            );

            ClearFields();
        }

        private void ClearFields()
        {
            Leeruitkomst = string.Empty;
            PrestatieNiveau = null;
            Toelichting = string.Empty;

            IsLeeruitkomstInvalid = false;
            IsPrestatieNiveauInvalid = false;
        }

        private void Notify(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
