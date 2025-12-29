using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StudentSysteem.Core.Models
{
    public class Toelichting : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _tekst;
        public string Tekst
        {
            get => _tekst;
            set { _tekst = value; Notify(); }
        }

        private Criterium _criterium;
        public Criterium GekoppeldCriterium
        {
            get => _criterium;
            set { _criterium = value; Notify(); Notify(nameof(GeselecteerdeOptie)); }
        }

        // UI-helper (blijft bestaan!)
        public string GeselecteerdeOptie =>
            GekoppeldCriterium?.DisplayNaam ?? "Toelichting koppelen aan...";

        private void Notify([CallerMemberName] string p = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
    }
}