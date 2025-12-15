using System.ComponentModel;
using System.Windows.Input;

namespace StudentSysteem.Core.Models
{
    public class ExtraToelichting : INotifyPropertyChanged
    
    {
        public BeoordelingItem ParentBeoordelingItem { get; set; }
        private string _tekst;
        public string Tekst
        {
            get => _tekst;
            set
            {
                _tekst = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tekst)));
            }
        }

        public List<string> Opties { get; } = new List<string> { "Algemeen", "Criteria 1", "Criteria 2", "Criteria 3" };

        private string _geselecteerdeOptie = "Toelichting gekoppeld aan...";
        public string GeselecteerdeOptie
        {
            get => _geselecteerdeOptie;
            set
            {
                _geselecteerdeOptie = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GeselecteerdeOptie)));
            }
        }

        public ICommand OptiesCommand { get; }

        public ExtraToelichting()
        {
            OptiesCommand = new Command(ShowOptiesPicker);
        }

        private async void ShowOptiesPicker()
        {
            var beschikbareOpties = ParentBeoordelingItem.GetBeschikbareOpties();

            var selectedOption = await Application.Current.MainPage.DisplayActionSheet(
                "Toelichting gekoppeld aan...",
                "Annuleren",
                null,
                beschikbareOpties.ToArray()
            );

            if (!string.IsNullOrEmpty(selectedOption) && selectedOption != "Annuleren")
                GeselecteerdeOptie = selectedOption;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}