/*
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentSysteem.Core.Models
{
    public class BeoordelingItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isUpdating;
        // Prestatiedoel
        public int PrestatiedoelId { get; set; }
        public string PrestatiedoelBeschrijving { get; set; }
        public string AiAssessmentScale { get; set; }
        
        // Vaardigheid
        public string VaardigheidNaam { get; set; }
        public string HboiActiviteit { get; set; }
        public string VaardigheidBeschrijving { get; set; }
        public string LeertakenUrl { get; set; } = "";

        // Prestatiedoel-balk
        public string ProcesNaam { get; set; }
        public string ProcesstapNaam { get; set; }
        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set { _isExpanded = value; Notify(); }
        }
        public string ExpanderTitel =>
            $"{ProcesNaam} | {ProcesstapNaam} | {VaardigheidNaam}";

        // Niveauaanduiding
        private Niveauaanduiding _geselecteerdNiveau = Niveauaanduiding.NietIngeleverd;

        
        // Toelichtingen
        public ObservableCollection<Toelichting> Toelichtingen { get; set; } = new();

        private bool _kanExtraToelichtingToevoegen;

        public bool KanExtraToelichtingToevoegen
        {
            get => _kanExtraToelichtingToevoegen;
            set
            {
                _kanExtraToelichtingToevoegen = value;
                Notify();
            }
        }

        private Niveauaanduiding _kleur = Niveauaanduiding.NietIngeleverd;
        public Niveauaanduiding Kleur
        {
            get => _kleur;
            private set { _kleur = value; Notify(); }
        }
        
        private bool _isToelichtingInvalid;
        public bool IsToelichtingInvalid
        {
            get => _isToelichtingInvalid;
            set { _isToelichtingInvalid = value; Notify(); }
        }

        public BeoordelingItem() { }
        
        private void Notify([CallerMemberName] string prop = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
*/