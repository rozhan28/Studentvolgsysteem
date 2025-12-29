using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;
﻿using System;
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
    public class FeedbackFormulierViewModel : BasisViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IMeldingService _meldingService;
        private readonly IFeedbackFormulierService _feedbackService;
        private readonly IPrestatiedoelService _prestatiedoelService;
        private readonly IVaardigheidService _vaardigheidService;
        private readonly IToelichtingService _toelichtingService;
        private readonly ZelfEvaluatieViewModel _zelfEvaluatieViewModel;
        private readonly bool _isDocent;
        
        public ICommand LeertakenCommand { get; }

        private ObservableCollection<BeoordelingItem> _beoordelingen;
        public ObservableCollection<BeoordelingItem> Beoordelingen
        {
            get => _beoordelingen;
            set
            {
                _beoordelingen = value;
                Notify(nameof(Beoordelingen));
            }
        }

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
            IZelfEvaluatieService zelfEvaluatieService,
            IMeldingService meldingService,
            IFeedbackFormulierService feedbackService,
            IPrestatiedoelService prestatiedoelService,
            IVaardigheidService vaardigheidService,
            IToelichtingService toelichtingService,
            GlobaleViewModel globaal)
        {
            _zelfEvaluatieViewModel = new ZelfEvaluatieViewModel(zelfEvaluatieService);
            _meldingService = meldingService;
            _feedbackService = feedbackService;

            _prestatiedoelService = prestatiedoelService;
            _vaardigheidService = vaardigheidService;
            _toelichtingService = toelichtingService;
            _isDocent = globaal.IngelogdeGebruiker?.Rol == Role.Docent;
            
            Task.Run(async () => await InitialiseerPaginaAsync());
            
            OpslaanCommand = new Command(async () => await BewaarEvaluatieAsync());
            LeertakenCommand = new Command<string>(async (url) => await OpenLeertakenUrl(url));
            VoegExtraToelichtingToeCommand = new Command<BeoordelingItem>(item => VoegExtraToelichtingToe(item));
            OptiesCommand = new Command<Toelichting>(async t => await ShowOptiesPicker(t));
        }


        // Toegevoegd voor TC2-01.1 - voorkomt crashen
        // Zorgt ervoor dat indien de database niet beschikbaar is, de applicatie niet crasht
        private async Task InitialiseerPaginaAsync()
        {
            try 
            {
                LaadPrestatiedoelen();
            }
            catch (Exception ex)
            {
                StatusMelding = "Databasefout: De prestatiedoelen konden niet worden geladen.";
                System.Diagnostics.Debug.WriteLine($"Fout: {ex.Message}");
            }
        }
        
        private void LaadPrestatiedoelen()
        {
            IEnumerable<Prestatiedoel> doelen = _prestatiedoelService.HaalPrestatiedoelenOp();
            IEnumerable<Vaardigheid> vaardigheden = _vaardigheidService.HaalAlleVaardighedenOp();
            
            List<BeoordelingItem> items = doelen.Select(delegate(Prestatiedoel d)
            {
                Vaardigheid gekoppeldeVaardigheid = vaardigheden.FirstOrDefault(v => v.Prestatiedoel_id == d.Id);

                return new BeoordelingItem
                {
                    PrestatiedoelId = d.Id,
                    Titel = $"Prestatiedoel {d.Id}",
                    PrestatiedoelBeschrijving = d.Beschrijving,
                    AiAssessmentScale = d.AiAssessmentScale,

                    Vaardigheid = gekoppeldeVaardigheid?.VaardigheidNaam ?? "Geen vaardigheid gekoppeld",
                    LeertakenUrl = gekoppeldeVaardigheid?.LeertakenUrl,
                    HboiActiviteit = gekoppeldeVaardigheid?.HboiActiviteit,
                    Beschrijving = gekoppeldeVaardigheid?.VaardigheidBeschrijving
                };
            }).ToList();

            Beoordelingen = new ObservableCollection<BeoordelingItem>(items);
            
                        foreach (BeoordelingItem item in Beoordelingen)
            {
                if (item.Toelichtingen == null)
                    item.Toelichtingen = new ObservableCollection<Toelichting>();

                item.Toelichtingen.Add(_toelichtingService.MaakNieuweToelichting());
                HookToelichtingen(item);
            }
        }

        private async Task BewaarEvaluatieAsync()
        {
            StatusMelding = string.Empty;

            if (!ValideerBeoordelingen())
            {
                StatusMelding = "Controleer alle velden a.u.b.";
                return;
            }

            try
            {
                int zelfEvaluatieId = _zelfEvaluatieViewModel.SlaZelfEvaluatieOp(1);

                foreach (BeoordelingItem item in Beoordelingen)
                {
                    if (item.Toelichtingen != null && item.Toelichtingen.Any())
                    {
                        _feedbackService.SlaToelichtingenOp(item.Toelichtingen.ToList(), zelfEvaluatieId);
                    }
                }

                await _meldingService.ToonMeldingAsync(
                    "Succes",
                    "Toelichting is opgeslagen!");
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
                bool niveauGekozen = ValideerPrestatieNiveau(item);
                bool criteriumGekozen = false;

                bool niveauOfCriteriumOk = niveauGekozen || criteriumGekozen;
                item.IsPrestatieNiveauInvalid = !niveauOfCriteriumOk;
                item.IsCriteriumInvalid = !niveauOfCriteriumOk;

                bool alleToelichtingenIngevuld = ZijnAlleToelichtingenOk(item.Toelichtingen);
                bool toelichtingOk = _isDocent || alleToelichtingenIngevuld;
                item.IsToelichtingInvalid = !toelichtingOk;
               
                if (!niveauOfCriteriumOk || !toelichtingOk)
                    allesGeldig = false;
            }

            return allesGeldig;
        }
        
        public bool ZijnAlleToelichtingenOk(ObservableCollection<Toelichting> toelichtingen)
        {
            if (_isDocent)
                return true;
            
            if (toelichtingen == null || !toelichtingen.Any())
                return false;
            
            return toelichtingen.All(t => IsToelichtingCorrect(t));
        }

        private bool IsToelichtingCorrect(Toelichting toelichting)
        {
            return !string.IsNullOrWhiteSpace(toelichting.Tekst) &&
                   toelichting.GeselecteerdeOptie != "Toelichting gekoppeld aan...";
        }
        
        private static bool ValideerPrestatieNiveau(BeoordelingItem item)
        {
            return item.InOntwikkeling
                   || item.IsOpNiveau
                   || item.IsBovenNiveau;
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
        
        private async Task OpenLeertakenUrl(string url)
        {
            if (!string.IsNullOrWhiteSpace(url) && Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                await Browser.Default.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
            }
        }
    }
}
