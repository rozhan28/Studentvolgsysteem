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

        // Basis info
        public int PrestatiedoelId { get; set; }
        public string PrestatiedoelBeschrijving { get; set; }
        public string AiAssessmentScale { get; set; }
        public string Titel { get; set; }
        public string Vaardigheid { get; set; }
        public string Beschrijving { get; set; }
        public string LeertakenUrl { get; set; }
        public string HboiActiviteit { get; set; }

        // Extra toelichting
        public ObservableCollection<ExtraToelichting> ExtraToelichtingVak { get; } = new ObservableCollection<ExtraToelichting>();

        // Criteria
        private Criterium _geselecteerdCriterium;
        public Criterium GeselecteerdCriterium
        {
            get => _geselecteerdCriterium;
            set
            {
                if (_geselecteerdCriterium == value) return;
                _geselecteerdCriterium = value;
                Notify();
            }
        }

        public Criterium AlgemeenCriterium { get; } = new Criterium
        {
            Id = -1,
            Beschrijving = "Algemeen",
            Niveau = "Algemeen"
        };

        public ObservableCollection<Criterium> OpNiveauCriteria { get; } = new ObservableCollection<Criterium>();
        public ObservableCollection<Criterium> BovenNiveauCriteria { get; } = new ObservableCollection<Criterium>();
        public ObservableCollection<Criterium> BeschikbareCriteria { get; } = new ObservableCollection<Criterium>();

        // Dropdown geselecteerde optie
        private string _geselecteerdeOptie = "Toelichting koppelen aan...";
        public string GeselecteerdeOptie
        {
            get => _geselecteerdeOptie;
            set
            {
                if (_geselecteerdeOptie == value) return;
                _geselecteerdeOptie = value;
                Notify();
            }
        }

        // Prestatie-niveau
        private PrestatieNiveauKleur _kleur = PrestatieNiveauKleur.NietIngeleverd;
        public PrestatieNiveauKleur Kleur
        {
            get => _kleur;
            private set
            {
                if (_kleur == value) return;
                _kleur = value;
                Notify();
            }
        }

        private bool _inOntwikkeling;
        public bool InOntwikkeling
        {
            get => _inOntwikkeling;
            set
            {
                if (_inOntwikkeling == value) return;
                _inOntwikkeling = value;
                if (value)
                    ResetCriteria();
                UpdateStatus();
                Notify();
            }
        }

        public bool IsOpNiveau => !InOntwikkeling && OpNiveauCriteria.Any() && OpNiveauCriteria.All(c => c.IsGeselecteerd);
        public bool IsBovenNiveau => !InOntwikkeling && BovenNiveauCriteria.Any() && BovenNiveauCriteria.All(c => c.IsGeselecteerd);

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

        // Validatie
        private bool _isPrestatieNiveauInvalid;
        public bool IsPrestatieNiveauInvalid
        {
            get => _isPrestatieNiveauInvalid;
            set { _isPrestatieNiveauInvalid = value; Notify(); }
        }

        private bool _isToelichtingInvalid;
        public bool IsToelichtingInvalid
        {
            get => _isToelichtingInvalid;
            set { _isToelichtingInvalid = value; Notify(); }
        }

        private bool _isCriteriumInvalid;
        public bool IsCriteriumInvalid
        {
            get => _isCriteriumInvalid;
            set { _isCriteriumInvalid = value; Notify(); }
        }

        // Toelichting
        private string _toelichting;
        public string Toelichting
        {
            get => _toelichting;
            set
            {
                if (_toelichting == value) return;
                _toelichting = value;
                Notify();
            }
        }

        // Commands
        public ICommand VoegExtraToelichtingToeCommand { get; }
        public ICommand OptiesCommand { get; }

        // Constructor
        public BeoordelingItem()
        {
            BeschikbareCriteria.Add(AlgemeenCriterium);

            // Commands zouden hier normaal geinitialiseerd worden in MVVM
            // Bijvoorbeeld OptiesCommand = new Command(async () => await ToonCriteriaKeuze());

            OpNiveauCriteria.CollectionChanged += (_, __) => HookCriteria(OpNiveauCriteria);
            BovenNiveauCriteria.CollectionChanged += (_, __) => HookCriteria(BovenNiveauCriteria);
        }

        // Helper: reset criteria bij InOntwikkeling
        private void ResetCriteria()
        {
            foreach (var c in OpNiveauCriteria) c.IsGeselecteerd = false;
            foreach (var c in BovenNiveauCriteria) c.IsGeselecteerd = false;
        }

        // Helper: hook criteria PropertyChanged
        private void HookCriteria(ObservableCollection<Criterium> criteria)
        {
            foreach (var c in criteria)
            {
                c.PropertyChanged += (_, __) => UpdateStatus();
            }
        }

        // Update Kleur/status
        private void UpdateStatus()
        {
            if (IsBovenNiveau)
                Kleur = PrestatieNiveauKleur.BovenNiveau;
            else if (IsOpNiveau)
                Kleur = PrestatieNiveauKleur.OpNiveau;
            else if (InOntwikkeling)
                Kleur = PrestatieNiveauKleur.InOntwikkeling;
            else
                Kleur = PrestatieNiveauKleur.NietIngeleverd;

            Notify(nameof(InOntwikkeling));
            Notify(nameof(IsOpNiveau));
            Notify(nameof(IsBovenNiveau));
            Notify(nameof(PrestatieNiveau));
        }

        private void Notify([CallerMemberName] string prop = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        // Voor dropdown keuze (async voorbeeld)
        private async Task ToonCriteriaKeuze()
        {
            var opties = BeschikbareCriteria.Select(c => c.DisplayNaam).ToArray();

            // Simulatie van action sheet
            string keuze = await Task.FromResult(opties.FirstOrDefault());

            if (string.IsNullOrWhiteSpace(keuze)) return;

            GeselecteerdeOptie = keuze;
            GeselecteerdCriterium = BeschikbareCriteria.FirstOrDefault(c => c.DisplayNaam == keuze);
        }
    }
}

