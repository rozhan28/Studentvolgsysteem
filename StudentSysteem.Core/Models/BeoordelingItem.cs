using System.ComponentModel;

namespace StudentSysteem.App.Models
{
    public class BeoordelingItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Titel { get; set; }
        public string Domein { get; set; }
        public string MakenDomeinmodel { get; set; }
        public string Beschrijving { get; set; }

        // ---- Toelichting ----
        private string _toelichting;
        public string Toelichting
        {
            get => _toelichting;
            set
            {
                _toelichting = value;
                Notify(nameof(Toelichting));
            }
        }

        // ---- Validatie ----
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

        public string InOntwikkelingColor { get; set; } = "White";
        public string OpNiveauColor { get; set; } = "White";
        public string BovenNiveauColor { get; set; } = "White";

        private string _containerColor = "White";
        public string ContainerColor
        {
            get => _containerColor;
            set { _containerColor = value; Notify(nameof(ContainerColor)); }
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
                {
                    OpNiveauDomeinWeerspiegelt = false;
                    OpNiveauSyntaxCorrect = false;
                    OpNiveauVastgelegd = false;
                    BovenNiveauVolledig = false;
                }

                UpdateColor();
                Notify(nameof(InOntwikkeling));
                Notify(nameof(PrestatieNiveau));
            }
        }

        private bool _opNiveauDomeinWeerspiegelt;
        public bool OpNiveauDomeinWeerspiegelt
        {
            get => _opNiveauDomeinWeerspiegelt;
            set
            {
                _opNiveauDomeinWeerspiegelt = value;

                if (value)
                {
                    InOntwikkeling = false;
                    BovenNiveauVolledig = false;
                }

                UpdateColor();
                Notify(nameof(OpNiveauDomeinWeerspiegelt));
                Notify(nameof(PrestatieNiveau));
            }
        }

        private bool _opNiveauSyntaxCorrect;
        public bool OpNiveauSyntaxCorrect
        {
            get => _opNiveauSyntaxCorrect;
            set
            {
                _opNiveauSyntaxCorrect = value;

                if (value)
                {
                    InOntwikkeling = false;
                    BovenNiveauVolledig = false;
                }

                UpdateColor();
                Notify(nameof(OpNiveauSyntaxCorrect));
                Notify(nameof(PrestatieNiveau));
            }
        }

        private bool _opNiveauVastgelegd;
        public bool OpNiveauVastgelegd
        {
            get => _opNiveauVastgelegd;
            set
            {
                _opNiveauVastgelegd = value;

                if (value)
                {
                    InOntwikkeling = false;
                    BovenNiveauVolledig = false;
                }

                UpdateColor();
                Notify(nameof(OpNiveauVastgelegd));
                Notify(nameof(PrestatieNiveau));
            }
        }

        private bool _bovenNiveauVolledig;
        public bool BovenNiveauVolledig
        {
            get => _bovenNiveauVolledig;
            set
            {
                _bovenNiveauVolledig = value;

                if (value)
                {
                    InOntwikkeling = false;
                    OpNiveauDomeinWeerspiegelt = false;
                    OpNiveauSyntaxCorrect = false;
                    OpNiveauVastgelegd = false;
                }

                UpdateColor();
                Notify(nameof(BovenNiveauVolledig));
                Notify(nameof(PrestatieNiveau));
            }
        }

        // ---- PrestatieNiveau ----
        public string PrestatieNiveau
        {
            get
            {
                if (BovenNiveauVolledig)
                    return "Boven niveau";

                if (OpNiveauDomeinWeerspiegelt && OpNiveauSyntaxCorrect && OpNiveauVastgelegd)
                    return "Op niveau";

                if (InOntwikkeling)
                    return "In ontwikkeling";

                return "Geen";
            }
        }

        // ---- Update UI kleuren ----
        private void UpdateColor()
        {
            InOntwikkelingColor = "White";
            OpNiveauColor = "White";
            BovenNiveauColor = "White";

            if (BovenNiveauVolledig)
            {
                BovenNiveauColor = "#78D97F";
                ContainerColor = "#E2FFE4";
            }
            else if (OpNiveauDomeinWeerspiegelt || OpNiveauSyntaxCorrect || OpNiveauVastgelegd)
            {
                OpNiveauColor = "#BFF8C6";
                ContainerColor = "#F2FFF3";
            }
            else if (InOntwikkeling)
            {
                InOntwikkelingColor = "#BFD7FF";
                ContainerColor = "#E8F1FF";
            }
            else
            {
                ContainerColor = "White";
            }


            Notify(nameof(InOntwikkelingColor));
            Notify(nameof(OpNiveauColor));
            Notify(nameof(BovenNiveauColor));
        }

        private void Notify(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
