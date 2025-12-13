using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

namespace StudentSysteem.App.ViewModels
{
    public class FeedbackFormulierViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly INavigatieService _navigatieService;
        private readonly IMeldingService _meldingService;
        private readonly IFeedbackFormulierService _feedbackService;
        private readonly IPrestatiedoelService _prestatiedoelService;
        private readonly bool _isDocent;

        private ObservableCollection<BeoordelingItem> _beoordelingen;
        public ObservableCollection<BeoordelingItem> Beoordelingen
        {
            get => _beoordelingen;
            set
            {
                _beoordelingen = value;
                OnPropertyChanged(nameof(Beoordelingen));
            }
        }

        private string _statusMelding;
        public string StatusMelding
        {
            get => _statusMelding;
            set
            {
                _statusMelding = value;
                OnPropertyChanged(nameof(StatusMelding));
            }
        }

        public ICommand OpslaanCommand { get; }

        public FeedbackFormulierViewModel(
            IZelfEvaluatieService zelfEvaluatieService,
            INavigatieService navigatieService,
            IMeldingService meldingService,
            IFeedbackFormulierService feedbackService,
            IPrestatiedoelService prestatiedoelService,
            bool isDocent = false)
        {
            _navigatieService = navigatieService;
            _meldingService = meldingService;
            _feedbackService = feedbackService;
            _prestatiedoelService = prestatiedoelService;
            _isDocent = isDocent;

            OpslaanCommand = new Command(async () => await BewaarReflectieAsync());

            LaadPrestatiedoelen();
        }
        private void LaadPrestatiedoelen()
        {
            var doelen = _prestatiedoelService.HaalPrestatiedoelenOp();

            Beoordelingen = new ObservableCollection<BeoordelingItem>(
                doelen.Select(d => new BeoordelingItem
                {
                    Titel = d.Beschrijving,              // prestatiedoel tekst
                    PrestatiedoelNiveau = d.Niveau,      // "Op niveau" / "Boven niveau"
                    Vaardigheid = $"Criterium {d.CriteriumId}"
                })
            );
        }


        private async Task BewaarReflectieAsync()
        {
            StatusMelding = string.Empty;

            if (!ValideerBeoordelingen())
            {
                StatusMelding = "Controleer alle velden a.u.b.";
                return;
            }

            try
            {
                foreach (var item in Beoordelingen)
                {
                    if (!string.IsNullOrWhiteSpace(item.Toelichting))
                    {
                        _feedbackService.SlaToelichtingOp(item.Toelichting, 1);
                    }
                }

                await _meldingService.ToonMeldingAsync(
                    "Succes",
                    "Toelichting is opgeslagen!"
                );
            }
            catch (Exception ex)
            {
                StatusMelding = $"Fout bij opslaan: {ex.Message}";
            }
        }

        private bool ValideerBeoordelingen()
        {
            bool allesGeldig = true;

            foreach (var item in Beoordelingen)
            {
                bool prestatieOk = ValideerPrestatieNiveau(item);
                item.IsPrestatieNiveauInvalid = !prestatieOk;

                bool toelichtingOk = !(string.IsNullOrWhiteSpace(item.Toelichting) && !_isDocent);
                item.IsToelichtingInvalid = !toelichtingOk;

                if (!prestatieOk || !toelichtingOk)
                    allesGeldig = false;
            }

            return allesGeldig;
        }

        private static bool ValideerPrestatieNiveau(BeoordelingItem item)
        {
            return item.InOntwikkeling
                || item.OpNiveauSyntaxCorrect
                || item.OpNiveauVastgelegd
                || item.OpNiveauDomeinWeerspiegelt
                || item.BovenNiveauVolledig;
        }

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
