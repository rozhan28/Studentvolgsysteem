using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StudentSysteem.Core.Models
{
    public class Criterium : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }
        public string Beschrijving { get; set; }
        public string Niveau { get; set; }

        public string DisplayNaam =>
            $"{Beschrijving} ({Niveau})";

        private bool _isGeselecteerd;
        public bool IsGeselecteerd
        {
            get => _isGeselecteerd;
            set
            {
                if (_isGeselecteerd == value) return;
                _isGeselecteerd = value;
                Notify();
            }
        }

        private void Notify([CallerMemberName] string prop = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
