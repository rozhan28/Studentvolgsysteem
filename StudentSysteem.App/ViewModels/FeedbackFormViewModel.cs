using StudentSysteem.App.Models;
using StudentSysteem.Core.Interfaces.Services;
using StudentVolgSysteem.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentSysteem.App.ViewModels
{
    public class FeedbackFormViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IZelfReflectieService _zelfreflectieService;
        private readonly INavigatieService _navigatieService;
        private readonly IMeldingService _meldingService;
        private readonly bool _isDocent;

        public FeedbackFormViewModel(
            IZelfReflectieService zelfreflectieService,
            INavigatieService navigatieService,
            IMeldingService meldingService,
            bool isDocent = false)
        {
            _zelfreflectieService = zelfreflectieService;
            _navigatieService = navigatieService;
            _meldingService = meldingService;   // ✔ bugfix: puntkomma verwijderd
            _isDocent = isDocent;

            OpslaanCommand = new Command(async () => await BewaarReflectie());

            // ⭐ STARTDATA
            Beoordelingen = new ObservableCollection<BeoordelingItem>
            {
                new BeoordelingItem {
                    Titel = "Requirementsanalyseproces – Definiëren van het probleemdomein",
                    Domein = "Analyseren",
                    MakenDomeinmodel = "Maken van een domeinmodel",
                    Beschrijving = "Het maken van een domeinmodel volgens een UML-klassendiagram"
                },
                new BeoordelingItem {
                    Titel = "Requirementsanalyseproces – Definiëren van het probleemdomein",
                    Domein = "Analyseren",
                    MakenDomeinmodel = "Bestuderen van de probleemstelling",
                    Beschrijving = "Het achterhalen van het probleem"
                },
                new BeoordelingItem {
                    Titel = "Requirementsanalyseproces – Verzamelen van requirements",
                    Domein = "Analyseren",
                    MakenDomeinmodel = "Beschrijven van stakeholders",
                    Beschrijving = "Het opstellen van een stakeholderanalyse"
                }
            };
        }

        // ⭐ Lijst van beoordelingsitems
        public ObservableCollection<BeoordelingItem> Beoordelingen { get; }

        private string _statusmelding;
        public string StatusMelding
        {
            get => _statusmelding;
            set { _statusmelding = value; Notify(nameof(StatusMelding)); }
        }

        public ICommand OpslaanCommand { get; }

        // ⭐ Opslaan van feedback
        private async Task BewaarReflectie()
        {
            StatusMelding = string.Empty;
            bool allesGeldig = true;

            foreach (var item in Beoordelingen)
            {
                bool itemGeldig = ValideerItem(item);
                item.IsPrestatieNiveauInvalid = !itemGeldig;

                item.IsToelichtingInvalid =
                    string.IsNullOrWhiteSpace(item.Toelichting) && !_isDocent;

                if (!itemGeldig || item.IsToelichtingInvalid)
                    allesGeldig = false;
            }

            if (!allesGeldig)
            {
                StatusMelding = "Controleer alle velden a.u.b.";
                return;
            }

            foreach (var item in Beoordelingen)
            {
                _zelfreflectieService.Add(new ZelfEvaluatie
                {
                    StudentId = 1,
                    PrestatieNiveau = item.PrestatieNiveau,
                    Toelichting = item.Toelichting,
                    Datum = DateTime.Now
                });
            }

            await _meldingService.ToonMeldingAsync("Succes", "Feedback is succesvol opgeslagen!");
        }

        // ⭐ Validatie per item
        private bool ValideerItem(BeoordelingItem item)
        {
            return item.InOntwikkeling ||
                   item.OpNiveauSyntaxCorrect ||
                   item.OpNiveauVastgelegd ||
                   item.OpNiveauDomeinWeerspiegelt ||
                   item.BovenNiveauVolledig;
        }

        private void Notify(string eigenschap) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(eigenschap));
    }
}

