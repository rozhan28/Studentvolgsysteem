using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;
using StudentSysteem.Core.Services;

namespace StudentSysteem.App.ViewModels
{
    public class FeedbackFormulierViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly INavigatieService _navigatieService;
        private readonly IMeldingService _meldingService;
        private readonly IFeedbackFormulierService _feedbackService;
        private readonly bool _isDocent;
        private readonly IToelichtingService _toelichtingService;

        public ObservableCollection<BeoordelingItem> Beoordelingen { get; set; }

        private string _statusMelding;
        public string StatusMelding
        {
            get => _statusMelding;
            set { _statusMelding = value; Notify(nameof(StatusMelding)); }
        }

        public ICommand OpslaanCommand { get; }
        public ICommand VoegExtraToelichtingToeCommand { get; }
        public ICommand OptiesCommand { get; }

        public FeedbackFormulierViewModel(
            IZelfEvaluatieService zelfevaluatieService,
            INavigatieService navigatieService,
            IMeldingService meldingService,
            IFeedbackFormulierService feedbackService,
            IToelichtingService toelichtingService,
            bool isDocent = false)
        {
            _navigatieService = navigatieService;
            _meldingService = meldingService;
            _feedbackService = feedbackService;
            _isDocent = isDocent;
            _toelichtingService = toelichtingService;

            OpslaanCommand = new Command(async () => await BewaarReflectieAsync());
            VoegExtraToelichtingToeCommand = new Command<BeoordelingItem>(item => VoegExtraToelichtingToe(item));
            OptiesCommand = new Command<Toelichting>(async t => await ShowOptiesPicker(t));

            Beoordelingen = new ObservableCollection<BeoordelingItem>
            {
                new BeoordelingItem {
                    Titel = "Maken domeinmodel | Definiëren probleemdomein | Requirementsanalyseproces | Analyseren",
                    Vaardigheid = "Maken domeinmodel",
                    Beschrijving = "Het maken van een domeinmodel volgens een UML klassendiagram"
                },
                new BeoordelingItem {
                    Titel = "Bestuderen probleemstelling | Definiëren probleemdomein | Requirementsanalyseproces | Analyseren",
                    Vaardigheid = "Bestuderen probleemstelling",
                    Beschrijving = "Het probleem achterhalen"
                },
                new BeoordelingItem {
                    Titel = "Beschrijven stakeholders | Verzamelen requirement | Requirementsanalyseproces | Analyseren",
                    Vaardigheid = "Beschrijven stakeholders",
                    Beschrijving = "Het maken van een stakeholderanalyse"
                }
            };

            foreach (BeoordelingItem item in Beoordelingen)
            {
                if (item.Toelichtingen == null)
                    item.Toelichtingen = new ObservableCollection<Toelichting>();

                item.Toelichtingen.Add(_toelichtingService.MaakNieuweToelichting());
                HookToelichtingen(item);
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
                foreach (BeoordelingItem item in Beoordelingen)
                {
                    _feedbackService.SlaToelichtingenOp(item.Toelichtingen.ToList(), 1);
                }

                if (Application.Current?.MainPage != null)
                    await _meldingService.ToonMeldingAsync("Succes", "Toelichting is opgeslagen!");
            }
            catch (Exception ex)
            {
                StatusMelding = $"Fout bij opslaan: {ex.Message}";
            }
        }

        private bool ValideerBeoordelingen()
        {
            bool allesGeldig = true;

            foreach (BeoordelingItem item in Beoordelingen)
            {
                bool toelichtingOk = !(item.Toelichtingen.All(t => string.IsNullOrWhiteSpace(t.Tekst)) && !_isDocent);
                if (!toelichtingOk) allesGeldig = false;
            }

            return allesGeldig;
        }

        private void VoegExtraToelichtingToe(BeoordelingItem item)
        {
            if (item == null) return;
            if (item.Toelichtingen.Count >= _toelichtingService.TotaleOptiesCount) return;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                item.Toelichtingen.Add(_toelichtingService.MaakNieuweToelichting());
            });
        }

        private async Task ShowOptiesPicker(Toelichting toelichting)
        {
            if (toelichting == null) return;
            var parent = Beoordelingen.FirstOrDefault(b => b.Toelichtingen.Contains(toelichting));
            if (parent == null) return;

            List<string> opties = _toelichtingService.GetBeschikbareOpties(parent.Toelichtingen);

            if (Application.Current?.MainPage == null) return;

            String selected = await Application.Current.MainPage.DisplayActionSheet(
                "Toelichting gekoppeld aan...",
                "Annuleren",
                null,
                opties.ToArray());

            if (!string.IsNullOrEmpty(selected) && selected != "Annuleren")
            {
                int idx = parent.Toelichtingen.IndexOf(toelichting);
                Toelichting nieuwe = new() { Tekst = toelichting.Tekst, GeselecteerdeOptie = selected };
                parent.Toelichtingen[idx] = nieuwe;
                Notify(nameof(Beoordelingen));
            }
        }

        private void HookToelichtingen(BeoordelingItem item)
        {
            item.KanExtraToelichtingToevoegen = item.Toelichtingen.Count < _toelichtingService.TotaleOptiesCount;

            void Handler(object s, NotifyCollectionChangedEventArgs e)
            {
                item.KanExtraToelichtingToevoegen = item.Toelichtingen.Count < _toelichtingService.TotaleOptiesCount;
                MainThread.BeginInvokeOnMainThread(() => Notify(nameof(Beoordelingen)));
            }

            item.Toelichtingen.CollectionChanged += Handler;
        }

        private void Notify(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
