using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;
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

        public ObservableCollection<BeoordelingItem> Beoordelingen { get; set; }
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
            IMeldingService meldingService)
        {
            _prestatiedoelService = prestatiedoelService;
            _criteriumService = criteriumService;
            _criteriumRepository = criteriumRepository;
            _feedbackRepository = feedbackRepository;
            _meldingService = meldingService;

            OpslaanCommand = new Command(SlaFeedbackOp);
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

        private void SlaFeedbackOp()
        {
            try
            {
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

                    var geselecteerdeCriteria =
                        item.OpNiveauCriteria
                        .Concat(item.BovenNiveauCriteria)
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

                _meldingService.ToonMeldingAsync(
                    "Succes",
                    "Feedback en criteria zijn opgeslagen");
            }
            catch (Exception ex)
            {
                StatusMelding = $"Fout: {ex.Message}";
            }
        }

        private void Notify(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}

