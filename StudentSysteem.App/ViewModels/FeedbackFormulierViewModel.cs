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
        private readonly IZelfEvaluatieService _zelfEvaluatieService;
        private readonly IMeldingService _meldingService;

        private readonly bool _isDocent;

        public ObservableCollection<BeoordelingItem> Beoordelingen { get; }
            = new();

        private readonly Dictionary<int, bool> _geselecteerdeCriteria = new();
        private readonly Dictionary<int, string> _toelichtingen = new();

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
            ICriteriumRepository criteriumRepository,
            IFeedbackRepository feedbackRepository,
            IZelfEvaluatieService zelfEvaluatieService,
            IMeldingService meldingService)
        {
            _prestatiedoelService = prestatiedoelService;
            _criteriumRepository = criteriumRepository;
            _feedbackRepository = feedbackRepository;
            _zelfEvaluatieService = zelfEvaluatieService;
            _meldingService = meldingService;

            _isDocent = GebruikerSessie.HuidigeRol == "Docent";

            OpslaanCommand = new Command(async () => await BewaarReflectieAsync());
            LaadPrestatiedoelen();
        }

        #region Criterium UI-state

        public bool IsCriteriumGeselecteerd(Criterium criterium)
        {
            return _geselecteerdeCriteria.TryGetValue(criterium.Id, out var value) && value;
        }

        public void ZetCriteriumGeselecteerd(Criterium criterium, bool waarde)
        {
            _geselecteerdeCriteria[criterium.Id] = waarde;
            Notify(nameof(Beoordelingen));
        }

        public string GetToelichting(Criterium criterium)
        {
            return _toelichtingen.TryGetValue(criterium.Id, out var value)
                ? value
                : string.Empty;
        }

        public void ZetToelichting(Criterium criterium, string tekst)
        {
            _toelichtingen[criterium.Id] = tekst;
            Notify(nameof(Beoordelingen));
        }

        #endregion

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

                var opNiveau = _criteriumRepository
                    .HaalCriteriaOpVoorPrestatiedoel(d.Id, "Op niveau");

                var bovenNiveau = _criteriumRepository
                    .HaalCriteriaOpVoorPrestatiedoel(d.Id, "Boven niveau");

                foreach (var c in opNiveau)
                {
                    item.OpNiveauCriteria.Add(c);
                    item.BeschikbareCriteria.Add(c);
                }

                foreach (var c in bovenNiveau)
                {
                    item.BovenNiveauCriteria.Add(c);
                    item.BeschikbareCriteria.Add(c);
                }

                Beoordelingen.Add(item);
            }
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
                _zelfEvaluatieService.Add(new ZelfEvaluatie
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

                    foreach (var criterium in item.BeschikbareCriteria)
                    {
                        if (!IsCriteriumGeselecteerd(criterium))
                            continue;

                        _criteriumRepository.SlaGeselecteerdeCriteriaOp(
                            feedbackId,
                            new List<Criterium> { criterium },
                            niveau);

                        var toelichting = GetToelichting(criterium);
                        if (!string.IsNullOrWhiteSpace(toelichting))
                        {
                            _feedbackRepository.VoegToelichtingToe(
                                feedbackId,
                                toelichting);
                        }
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

        private bool ValideerBeoordelingen()
        {
            bool allesGeldig = true;

            foreach (var item in Beoordelingen)
            {
                bool niveauGekozen = ValideerPrestatieNiveau(item);
                bool criteriumGekozen =
                    item.BeschikbareCriteria.Any(c => IsCriteriumGeselecteerd(c));

                bool niveauOfCriteriumOk = niveauGekozen || criteriumGekozen;

                item.IsPrestatieNiveauInvalid = !niveauOfCriteriumOk;
                item.IsCriteriumInvalid = !niveauOfCriteriumOk;

                bool toelichtingOk = _isDocent ||
                    item.BeschikbareCriteria.Any(c =>
                        IsCriteriumGeselecteerd(c) &&
                        !string.IsNullOrWhiteSpace(GetToelichting(c)));

                item.IsToelichtingInvalid = !_isDocent && !toelichtingOk;

                if (!niveauOfCriteriumOk || !toelichtingOk)
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
