using System.Collections.ObjectModel;
using System.ComponentModel;

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

        private bool _isUpdating;

        public string Titel { get; set; }
        public string Vaardigheid { get; set; }
        public string Beschrijving { get; set; }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded == value) return;
                _isExpanded = value;
                Notify(nameof(IsExpanded));
            }
        }

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
                    _opNiveauDomeinWeerspiegelt = false;
                    _opNiveauSyntaxCorrect = false;
                    _opNiveauVastgelegd = false;
                    _bovenNiveauVolledig = false;
                }
                UpdateColor();
                _isUpdating = false;
                Notify(nameof(InOntwikkeling));
                Notify(nameof(IsOpNiveau));
                Notify(nameof(IsBovenNiveau));
                Notify(nameof(IsInOntwikkeling));
                Notify(nameof(PrestatieNiveau));
            }
        }

        private bool _opNiveauDomeinWeerspiegelt;
        public bool OpNiveauDomeinWeerspiegelt
        {
            get => _opNiveauDomeinWeerspiegelt;
            set
            {
                if (_opNiveauDomeinWeerspiegelt == value) return;
                if (_isUpdating) return;
                _isUpdating = true;
                _opNiveauDomeinWeerspiegelt = value;
                if (value)
                {
                    _inOntwikkeling = false;
                    _bovenNiveauVolledig = false;
                }
                UpdateColor();
                _isUpdating = false;
                Notify(nameof(OpNiveauDomeinWeerspiegelt));
                Notify(nameof(PrestatieNiveau));
                Notify(nameof(IsOpNiveau));
                Notify(nameof(IsBovenNiveau));
                Notify(nameof(IsInOntwikkeling));
            }
        }

        private bool _opNiveauSyntaxCorrect;
        public bool OpNiveauSyntaxCorrect
        {
            get => _opNiveauSyntaxCorrect;
            set
            {
                if (_opNiveauSyntaxCorrect == value) return;
                if (_isUpdating) return;
                _isUpdating = true;
                _opNiveauSyntaxCorrect = value;
                if (value)
                {
                    _inOntwikkeling = false;
                    _bovenNiveauVolledig = false;
                }
                UpdateColor();
                _isUpdating = false;
                Notify(nameof(OpNiveauSyntaxCorrect));
                Notify(nameof(PrestatieNiveau));
                Notify(nameof(IsOpNiveau));
                Notify(nameof(IsBovenNiveau));
                Notify(nameof(IsInOntwikkeling));
            }
        }

        private bool _opNiveauVastgelegd;
        public bool OpNiveauVastgelegd
        {
            get => _opNiveauVastgelegd;
            set
            {
                if (_opNiveauVastgelegd == value) return;
                if (_isUpdating) return;
                _isUpdating = true;
                _opNiveauVastgelegd = value;
                if (value)
                {
                    _inOntwikkeling = false;
                    _bovenNiveauVolledig = false;
                }
                UpdateColor();
                _isUpdating = false;
                Notify(nameof(OpNiveauVastgelegd));
                Notify(nameof(PrestatieNiveau));
                Notify(nameof(IsOpNiveau));
                Notify(nameof(IsBovenNiveau));
                Notify(nameof(IsInOntwikkeling));
            }
        }

        private bool _bovenNiveauVolledig;
        public bool BovenNiveauVolledig
        {
            get => _bovenNiveauVolledig;
            set
            {
                if (_bovenNiveauVolledig == value) return;
                if (_isUpdating) return;
                _isUpdating = true;
                _bovenNiveauVolledig = value;
                if (value)
                {
                    _inOntwikkeling = false;
                    _opNiveauDomeinWeerspiegelt = false;
                    _opNiveauSyntaxCorrect = false;
                    _opNiveauVastgelegd = false;
                }
                UpdateColor();
                _isUpdating = false;
                Notify(nameof(BovenNiveauVolledig));
                Notify(nameof(PrestatieNiveau));
                Notify(nameof(IsOpNiveau));
                Notify(nameof(IsBovenNiveau));
                Notify(nameof(IsInOntwikkeling));
            }
        }

        public string PrestatieNiveau
        {
            get
            {
                if (BovenNiveauVolledig) return "Boven niveau";
                if (IsOpNiveau) return "Op niveau";
                if (InOntwikkeling) return "In ontwikkeling";
                return string.Empty;
            }
        }

        private string _toelichting;
        public string Toelichting
        {
            get => _toelichting;
            set { _toelichting = value; Notify(nameof(Toelichting)); }
        }

        private bool _isPrestatieNiveauInvalid;
        public bool IsPrestatieNiveauInvalid
        {
            get => _isPrestatieNiveauInvalid;
            set { _isPrestatieNiveauInvalid = value; Notify(nameof(IsPrestatieNiveauInvalid)); }
        }

        private bool _isToelichtingInvalid;
        public bool IsToelichtingInvalid
        {
            get => _isToelichtingInvalid;
            set { _isToelichtingInvalid = value; Notify(nameof(IsToelichtingInvalid)); }
        }

        public bool IsOpNiveau =>
            OpNiveauDomeinWeerspiegelt && OpNiveauSyntaxCorrect && OpNiveauVastgelegd;

        public bool IsInOntwikkeling => InOntwikkeling;

        public bool IsBovenNiveau => BovenNiveauVolledig;

        private bool _kanExtraToelichtingToevoegen;
        public bool KanExtraToelichtingToevoegen
        {
            get => _kanExtraToelichtingToevoegen;
            set
            {
                if (_kanExtraToelichtingToevoegen == value) return;
                _kanExtraToelichtingToevoegen = value;
                Notify(nameof(KanExtraToelichtingToevoegen));
            }
        }


        private void UpdateColor()
        {
            if (_isUpdating) return;
            if (BovenNiveauVolledig)
                Kleur = PrestatieNiveauKleur.BovenNiveau;
            else if (OpNiveauDomeinWeerspiegelt && OpNiveauSyntaxCorrect && OpNiveauVastgelegd)
                Kleur = PrestatieNiveauKleur.OpNiveau;
            else if (InOntwikkeling)
                Kleur = PrestatieNiveauKleur.InOntwikkeling;
            else
                Kleur = PrestatieNiveauKleur.NietIngeleverd;
            Notify(nameof(IsOpNiveau));
            Notify(nameof(IsInOntwikkeling));
            Notify(nameof(IsBovenNiveau));
        }

        private void Notify(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        
        public ObservableCollection<Toelichting> Toelichtingen { get; set; } = new ObservableCollection<Toelichting>();

    }
}
