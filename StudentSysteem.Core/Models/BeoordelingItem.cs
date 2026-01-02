using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentSysteem.Core.Models
{
    public enum PrestatieNiveauKleur
    {
        NietIngeleverd,
        InOntwikkeling,
        OpNiveau,
        BovenNiveau
    }

    public class BeoordelingItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Algemene properties
        public int PrestatiedoelId { get; set; }
        public string Titel { get; set; } = "";
        public string PrestatiedoelBeschrijving { get; set; } = "";
        public string AiAssessmentScale { get; set; } = "";
        public string Vaardigheid { get; set; } = "";
        public string LeertakenUrl { get; set; } = "";
        public string HboiActiviteit { get; set; } = "";
        public string Beschrijving { get; set; } = "";
        public string Proces { get; set; }
        public string Processtap { get; set; }


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

        // Toelichtingen
        public ObservableCollection<Toelichting> Toelichtingen { get; set; } = new();

        private string _geselecteerdeOptie = "Toelichting koppelen aan...";
        public string GeselecteerdeOptie
        {
            get => _geselecteerdeOptie;
            set { _geselecteerdeOptie = value; Notify(); }
        }

        private bool _kanExtraToelichtingToevoegen;
        public bool KanExtraToelichtingToevoegen
        {
            get => _kanExtraToelichtingToevoegen;
            set { _kanExtraToelichtingToevoegen = value; Notify(); }
        }


        public string ExpanderTitel =>
        $"{Proces} | {Processtap} | {Vaardigheid}";

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

        private PrestatieNiveauKleur _kleur = PrestatieNiveauKleur.NietIngeleverd;
        public PrestatieNiveauKleur Kleur
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

        // Commands
        public ICommand VoegExtraToelichtingToeCommand { get; }

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
            if (IsBovenNiveau)
            {
                Kleur = PrestatieNiveauKleur.BovenNiveau;
                _inOntwikkeling = false;
            }
            else if (IsOpNiveau)
            {
                Kleur = PrestatieNiveauKleur.OpNiveau;
                _inOntwikkeling = false;
            }
            else if (InOntwikkeling)
            {
                Kleur = PrestatieNiveauKleur.InOntwikkeling;
            }
            else
            {
                Kleur = PrestatieNiveauKleur.NietIngeleverd;
            }

            Notify(nameof(InOntwikkeling));
            Notify(nameof(IsOpNiveau));
            Notify(nameof(IsBovenNiveau));
            Notify(nameof(Kleur));
            Notify(nameof(PrestatieNiveau));
        }


        private void Notify([CallerMemberName] string prop = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}

