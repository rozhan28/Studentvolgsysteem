using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;
using StudentSysteem.Core.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace StudentSysteem.App.ViewModels
{
    public class FeedbackFormulierViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ICriteriumRepository _criteriumRepository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IPrestatiedoelService _prestatiedoelService;
        private readonly ICriteriumService _criteriumService;
        private readonly IMeldingService _meldingService;
        private readonly IFeedbackFormulierService _feedbackService;
        private readonly IZelfEvaluatieService _zelfEvaluatieService;

        private readonly bool _isDocent;

        public ObservableCollection<BeoordelingItem> Beoordelingen { get; }
            = new();

        private string _statusMelding;
        public string StatusMelding
        {
            get => _statusMelding;
            set
            {
                _statusMelding = value;
                Notify(nameof(StatusMelding));
            }
        }

        public ICommand OpslaanCommand { get; }

        public FeedbackFormulierViewModel(
        IPrestatiedoelService prestatiedoelService,
        ICriteriumService criteriumService,
        ICriteriumRepository criteriumRepository,
        IFeedbackRepository feedbackRepository,
        IFeedbackFormulierService feedbackService,
        IZelfEvaluatieService zelfEvaluatieService,
        IMeldingService meldingService)
        {
            _prestatiedoelService = prestatiedoelService;
            _criteriumService = criteriumService;
            _criteriumRepository = criteriumRepository;
            _feedbackRepository = feedbackRepository;
            _feedbackService = feedbackService;
            _zelfEvaluatieService = zelfEvaluatieService;
            _meldingService = meldingService;

            _isDocent = GebruikerSessie.HuidigeRol == "Docent";

            OpslaanCommand = new Command(async () => await BewaarReflectieAsync());
            LaadPrestatiedoelen();
        }


        private void LaadPrestatiedoelen()
        {
            var doelen = _prestatiedoelService.HaalPrestatiedoelenOp();

            foreach (var d in doelen)
            {
                var item = new BeoordelingItem
                {
                    PrestatiedoelId = d.Id,
                    Titel = $"Prestatiedoel {d.Id}",
                    PrestatiedoelBeschrijving = d.Beschrijving
                };

                foreach (var c in _criteriumService.HaalOpNiveauCriteriaOp())
                {
                    item.OpNiveauCriteria.Add(c);
                    item.BeschikbareCriteria.Add(c);
                }

                foreach (var c in _criteriumService.HaalBovenNiveauCriteriaOp())
                {
                    item.BovenNiveauCriteria.Add(c);
                    item.BeschikbareCriteria.Add(c);
                }

                Beoordelingen.Add(item);
            }
        }

        //Opslaan
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
                // Zelfevaluatie aanmaken
                int zelfEvaluatieId = _zelfEvaluatieService.Add(new ZelfEvaluatie
                {
                    StudentId = 1,
                    PrestatieNiveau = "Ingevuld"
                });

                foreach (var item in Beoordelingen)
                {
                    string niveau =
                        item.IsBovenNiveau ? "Boven niveau" :
                        item.IsOpNiveau ? "Op niveau" :
                        item.InOntwikkeling ? "In ontwikkeling" :
                        null;

                    if (niveau == null)
                        continue;

                    int feedbackId = _feedbackRepository.MaakFeedbackAan(niveau);

                    // Toelichting (alleen als ingevuld)
                    if (!string.IsNullOrWhiteSpace(item.Toelichting))
                    {
                        _feedbackRepository.VoegToelichtingToe(
                            feedbackId,
                            item.Toelichting);
                    }

                    // Criteria
                    var geselecteerdeCriteria = item.BeschikbareCriteria
                        .Where(c => c.IsGeselecteerd)
                        .ToList();

                    if (geselecteerdeCriteria.Any())
                    {
                        _criteriumRepository.SlaGeselecteerdeCriteriaOp(
                            feedbackId,
                            geselecteerdeCriteria,
                            niveau);
                    }
                }

                await _meldingService.ToonMeldingAsync(
                    "Succes",
                    "Zelfevaluatie en feedback zijn opgeslagen!");
            }
            catch (Exception ex)
            {
                StatusMelding = $"Fout bij opslaan: {ex.Message}";
            }
        }

        //Validatie
        private bool ValideerBeoordelingen()
        {
            bool allesGeldig = true;

            foreach (var item in Beoordelingen)
            {
                bool prestatieOk = !_isDocent || ValideerPrestatieNiveau(item);
                item.IsPrestatieNiveauInvalid = _isDocent && !prestatieOk;

                bool toelichtingOk = _isDocent || !string.IsNullOrWhiteSpace(item.Toelichting);
                item.IsToelichtingInvalid = !_isDocent && !toelichtingOk;

                bool criteriumOk = true;
                if (_isDocent)
                {
                    criteriumOk = item.BeschikbareCriteria.Any(c => c.IsGeselecteerd);
                    item.IsCriteriumInvalid = !criteriumOk;
                }
                else
                {
                    item.IsCriteriumInvalid = false;
                }

                if (!prestatieOk || !toelichtingOk || !criteriumOk)
                    allesGeldig = false;
            }

            return allesGeldig;
        }


        private static bool ValideerPrestatieNiveau(BeoordelingItem item)
        {
            return item.InOntwikkeling
                || item.IsOpNiveau
                || item.IsBovenNiveau;
        }

        private void Notify(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}