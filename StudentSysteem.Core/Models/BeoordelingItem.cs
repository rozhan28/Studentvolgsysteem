using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

public enum PrestatieNiveauKleur
{
    NietIngeleverd,
    InOntwikkeling,
    OpNiveau,
    BovenNiveau
}

namespace StudentSysteem.Core.Models
{
    public class BeoordelingItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int PrestatiedoelId { get; set; }

        public string PrestatiedoelBeschrijving { get; set; }

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
        public string PrestatiedoelNiveau { get; set; } = string.Empty;




        private ObservableCollection<ExtraToelichting> _extraToelichtingVak = new ObservableCollection<ExtraToelichting>();
        public ObservableCollection<ExtraToelichting> ExtraToelichtingVak
        {
            get => _extraToelichtingVak;
            set
            {
                _extraToelichtingVak = value;
                Notify(nameof(ExtraToelichtingVak));
            }
        }

        public ICommand VoegExtraToelichtingToeCommand { get; }
        public List<string> Opties { get; } = new() { "Algemeen", "Criteria 1", "Criteria 2", "Criteria 3" };
        private string _geselecteerdeOptie = "Toelichting gekoppeld aan...";
        public string GeselecteerdeOptie
        {
            get => _geselecteerdeOptie;
            set { _geselecteerdeOptie = value; Notify(nameof(GeselecteerdeOptie)); }
        }
        public ICommand OptiesCommand { get; }

        // --- PrestatieNiveau ---
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
                Notify(nameof(PrestatieNiveau));
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
                Notify(nameof(PrestatieNiveau));
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
                Notify(nameof(PrestatieNiveau));
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
                Notify(nameof(PrestatieNiveau));
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

        // --- Toelichting ---
        private string _toelichting;
        public string Toelichting
        {
            get => _toelichting;
            set { _toelichting = value; Notify(nameof(Toelichting)); }
        }

        // --- Validatie ---
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

        // --- Constructor ---
        public BeoordelingItem()
        {
            VoegExtraToelichtingToeCommand = new Command(() => VoegExtraToelichtingToe());
            OptiesCommand = new Command(ShowOptiesPicker);
        }

        // --- Methods ---
        public void VoegExtraToelichtingToe()
        {
            ExtraToelichtingVak.Add(new ExtraToelichting());
            Notify(nameof(ExtraToelichtingVak));
        }

        private void UpdateColor()
        {
            if (_isUpdating) return;
            
            System.Diagnostics.Debug.WriteLine($"=== UpdateColor aangeroepen ===");
            System.Diagnostics.Debug.WriteLine($"IsInOntwikkeling: {InOntwikkeling}");
            System.Diagnostics.Debug.WriteLine($"IsOpNiveau: {IsOpNiveau}");
            System.Diagnostics.Debug.WriteLine($"IsBovenNiveau: {BovenNiveauVolledig}");
            
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

        private async void ShowOptiesPicker()
        {
            var result = await Application.Current.MainPage.DisplayActionSheet(
                "Toelichting gekoppeld aan...",
                "Annuleren",
                null,
                Opties.ToArray()
            );

            if (!string.IsNullOrEmpty(result) && result != "Annuleren")
                GeselecteerdeOptie = result;
        }
    }
}