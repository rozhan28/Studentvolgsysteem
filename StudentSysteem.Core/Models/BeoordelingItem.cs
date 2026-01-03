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
        
        // Criteria
        public ObservableCollection<Criterium> OpNiveauCriteria { get; set; } = new();
        public ObservableCollection<Criterium> BovenNiveauCriteria { get; set; } = new();
        public ObservableCollection<Criterium> BeschikbareCriteria { get; set; } = new();

        private Criterium _geselecteerdCriterium;
        public Criterium GeselecteerdCriterium
        {
            get => _geselecteerdCriterium;
            set { _geselecteerdCriterium = value; Notify(); }
        }
        
        // Niveauaanduiding
        private Niveauaanduiding _geselecteerdNiveau = Niveauaanduiding.NietIngeleverd;

        public Niveauaanduiding GeselecteerdNiveau
        {
            get => _geselecteerdNiveau;
            set
            {
                if (_geselecteerdNiveau == value) return;
                _geselecteerdNiveau = value;
                Notify(nameof(GeselecteerdNiveau));
                Notify(nameof(PrestatieNiveau));
            }
        }

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

        // Status
        private bool _inOntwikkeling;
        public bool InOntwikkeling
        {
            get => _inOntwikkeling;
            set
            {
                if (_inOntwikkeling == value) return;
                _inOntwikkeling = value;
                if (value) ResetCriteria();
                UpdateStatus();
                Notify();
            }
        }

        public bool IsOpNiveau => !InOntwikkeling && OpNiveauCriteria.Any(c => c.IsGeselecteerd);
        public bool IsBovenNiveau => !InOntwikkeling && BovenNiveauCriteria.Any(c => c.IsGeselecteerd);

        private Niveauaanduiding _kleur = Niveauaanduiding.NietIngeleverd;
        public Niveauaanduiding Kleur
        {
            get => _kleur;
            private set { _kleur = value; Notify(); }
        }

        private bool _isPrestatieNiveauInvalid;
        public bool IsPrestatieNiveauInvalid
        {
            get => _isPrestatieNiveauInvalid;
            set { _isPrestatieNiveauInvalid = value; Notify(); }
        }

        private bool _isCriteriumInvalid;
        public bool IsCriteriumInvalid
        {
            get => _isCriteriumInvalid;
            set { _isCriteriumInvalid = value; Notify(); }
        }

        private bool _isToelichtingInvalid;
        public bool IsToelichtingInvalid
        {
            get => _isToelichtingInvalid;
            set { _isToelichtingInvalid = value; Notify(); }
        }

        public string PrestatieNiveau
        {
            get
            {
                if (IsBovenNiveau) return "Boven niveau";
                if (IsOpNiveau) return "Op niveau";
                if (InOntwikkeling) return "In ontwikkeling";
                return string.Empty;
            }
        }

        public BeoordelingItem()
        {
            var algemeen = new Criterium { Id = -1, Beschrijving = "Algemeen", Niveau = "Algemeen" };
            BeschikbareCriteria.Add(algemeen);

            OpNiveauCriteria.CollectionChanged += (_, __) => HookCriteria(OpNiveauCriteria);
            BovenNiveauCriteria.CollectionChanged += (_, __) => HookCriteria(BovenNiveauCriteria);

        }

        private void ResetCriteria()
        {
            foreach (var c in OpNiveauCriteria) c.IsGeselecteerd = false;
            foreach (var c in BovenNiveauCriteria) c.IsGeselecteerd = false;
        }

        private void HookCriteria(ObservableCollection<Criterium> criteria)
        {
            foreach (var c in criteria)
                c.PropertyChanged += (_, __) => UpdateStatus();
        }

        private void UpdateStatus()
        {
            if (InOntwikkeling)
            {
                GeselecteerdNiveau = Niveauaanduiding.InOntwikkeling;
            }
            else if (IsBovenNiveau)
            {
                GeselecteerdNiveau = Niveauaanduiding.BovenNiveau;
            }
            else if (IsOpNiveau)
            {
                GeselecteerdNiveau = Niveauaanduiding.OpNiveau;
            }
            else
            {
                GeselecteerdNiveau = Niveauaanduiding.NietIngeleverd;
            }
        }

        private void Notify([CallerMemberName] string prop = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}