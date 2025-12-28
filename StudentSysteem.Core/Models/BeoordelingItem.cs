using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

        public int PrestatiedoelId { get; set; }

        public string Titel { get; set; }
        public string Vaardigheid { get; set; }
        public string Beschrijving { get; set; }
        public string PrestatiedoelBeschrijving { get; set; }
        public string AiAssessmentScale { get; set; }
        public string HboiActiviteit { get; set; }

        private bool _isUpdating;

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded == value) return;
                _isExpanded = value;
                Notify();
            }
        }

        // ===================== CRITERIA (US3_Niveau) =====================

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

        public ObservableCollection<Criterium> OpNiveauCriteria { get; }
            = new ObservableCollection<Criterium>();

        public ObservableCollection<Criterium> BovenNiveauCriteria { get; }
            = new ObservableCollection<Criterium>();

        public ObservableCollection<Criterium> BeschikbareCriteria { get; }
            = new ObservableCollection<Criterium>();

        // ===================== STATUS / NIVEAUS =====================

        private bool _inOntwikkeling;
        public bool InOntwikkeling
        {
            get => _inOntwikkeling;
            set
            {
                if (_inOntwikkeling == value) return;
                if (_isUpdating) return;

                _isUpdating = true;
                _inOntwikkeling = value;

                if (value)
                {
                    ResetCriteria();
                    _opNiveauDomeinWeerspiegelt = false;
                    _opNiveauSyntaxCorrect = false;
                    _opNiveauVastgelegd = false;
                    _bovenNiveauVolledig = false;
                }

                UpdateStatus();
                _isUpdating = false;
                Notify();
            }
        }

        public bool IsOpNiveau =>
            !InOntwikkeling &&
            OpNiveauCriteria.Any() &&
            OpNiveauCriteria.All(c => c.IsGeselecteerd);

        public bool IsBovenNiveau =>
            !InOntwikkeling &&
            BovenNiveauCriteria.Any() &&
            BovenNiveauCriteria.All(c => c.IsGeselecteerd);

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

        // ===================== KLEUR =====================

        private PrestatieNiveauKleur _kleur = PrestatieNiveauKleur.NietIngeleverd;
        public PrestatieNiveauKleur Kleur
        {
            get => _kleur;
            private set
            {
                if (_kleur == value) return;
                _kleur = value;
                Notify(nameof(Kleur));
            }
        }

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

            Notify(nameof(IsOpNiveau));
            Notify(nameof(IsBovenNiveau));
            Notify(nameof(PrestatieNiveau));
        }

        // ===================== VALIDATIE =====================

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
            set
            {
                if (_isCriteriumInvalid == value) return;
                _isCriteriumInvalid = value;
                Notify();
            }
        }

        // ===================== TOELICHTING =====================

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

        public ObservableCollection<ExtraToelichting> ExtraToelichtingVak { get; }
            = new ObservableCollection<ExtraToelichting>();

        public ObservableCollection<Toelichting> Toelichtingen { get; set; }
            = new ObservableCollection<Toelichting>();

        // ===================== COMMANDS =====================

        public ICommand OptiesCommand { get; }
        public ICommand VoegExtraToelichtingToeCommand { get; }

        private string _geselecteerdeOptie = "Toelichting koppelen aan...";
        public string GeselecteerdeOptie
        {
            get => _geselecteerdeOptie;
            set
            {
                _geselecteerdeOptie = value;
                Notify();
            }
        }

        private string _leertakenUrl;
        public string LeertakenUrl
        {
            get => _leertakenUrl;
            set { _leertakenUrl = value; Notify(); }
        }

        // ===================== CONSTRUCTOR =====================

        public BeoordelingItem()
        {
            BeschikbareCriteria.Add(AlgemeenCriterium);

            OptiesCommand = new Command(async () => await ToonCriteriaKeuze());

            OpNiveauCriteria.CollectionChanged += (_, __) => HookCriteria(OpNiveauCriteria);
            BovenNiveauCriteria.CollectionChanged += (_, __) => HookCriteria(BovenNiveauCriteria);
        }

        // ===================== HULPMETHODES =====================

        private async Task ToonCriteriaKeuze()
        {
            var opties = BeschikbareCriteria
                .Select(c => c.DisplayNaam)
                .ToArray();

            string keuze = await Shell.Current.DisplayActionSheet(
                "Koppel toelichting aan criterium",
                "Annuleren",
                null,
                opties);

            if (keuze == "Annuleren" || string.IsNullOrWhiteSpace(keuze))
                return;

            GeselecteerdeOptie = keuze;

            GeselecteerdCriterium =
                BeschikbareCriteria.FirstOrDefault(c => c.DisplayNaam == keuze);
        }

        private void ResetCriteria()
        {
            foreach (var c in OpNiveauCriteria)
                c.IsGeselecteerd = false;

            foreach (var c in BovenNiveauCriteria)
                c.IsGeselecteerd = false;
        }

        private void HookCriteria(ObservableCollection<Criterium> criteria)
        {
            foreach (var c in criteria)
            {
                c.PropertyChanged += (_, __) =>
                {
                    UpdateStatus();
                };
            }
        }

        private void Notify([CallerMemberName] string prop = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
