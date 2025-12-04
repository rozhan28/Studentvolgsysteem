using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace StudentSysteem.Core.Models
{
    public class ExtraToelichting : INotifyPropertyChanged
    {
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

        public List<string> Opties { get; } = new List<string>
        {
            "Algemeen",
            "Criteria 1",
            "Criteria 2",
            "Criteria 3"
        };

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
            var result = await Application.Current.MainPage.DisplayActionSheet(
                "Toelichting gekoppeld aan...",
                "Annuleren",
                null,
                Opties.ToArray()
            );

            if (!string.IsNullOrEmpty(result) && result != "Annuleren")
            {
                GeselecteerdeOptie = result;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}