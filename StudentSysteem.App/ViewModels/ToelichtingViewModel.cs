using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

namespace StudentSysteem.App.ViewModels;

// Handelt alle toelichtingen af (toevoegen + validatie)
public partial class ToelichtingViewModel : BasisViewModel
{
    private readonly IToelichtingService _toelichtingService;
    private readonly Prestatiedoel _prestatiedoel;
    
    // Criteria
    private List<Criterium> _beschikbareCriteria;
    public ICommand OptiesCommand { get; }
    
    // Extra toelichting
    public ICommand VoegExtraToelichtingToeCommand { get; }
    [ObservableProperty]
    private bool kanExtraToelichtingToevoegen = true;
    public ObservableCollection<Toelichting> Toelichtingen { get; }
    
    // Validatie
    private readonly bool _isDocent;
    [ObservableProperty]
    private bool isToelichtingInvalid;
    [ObservableProperty]
    private HashSet<Toelichting> ongeldigeTekstVelden = new();
    [ObservableProperty]
    private HashSet<Toelichting> ongeldigeOptieVelden = new();
    
    public ToelichtingViewModel(Prestatiedoel prestatiedoel, IToelichtingService service, bool isDocent)
    {
        _prestatiedoel = prestatiedoel;
        _toelichtingService = service;
        _isDocent = isDocent;
        Toelichtingen = new ObservableCollection<Toelichting>();
        Toelichtingen.Add(new Toelichting());
        _beschikbareCriteria = prestatiedoel.Criteria;
        _beschikbareCriteria.Insert(0, new Criterium(0, "Algemeen", Niveauaanduiding.NietIngeleverd));
        
        VoegExtraToelichtingToeCommand = new Command(OnVoegExtraToelichtingToeCommand);
        OptiesCommand = new Command<Toelichting>(async t => await ShowOptiesPicker(t));
    }

    public void OnVoegExtraToelichtingToeCommand()
    {
        if (Toelichtingen.Count >= _beschikbareCriteria.Count)
        {
            KanExtraToelichtingToevoegen = false;
            OnPropertyChanged(nameof(KanExtraToelichtingToevoegen));
            return;
        }
        
        Toelichtingen.Add(new Toelichting());
        KanExtraToelichtingToevoegen = Toelichtingen.Count < _beschikbareCriteria.Count;
        OnPropertyChanged(nameof(Toelichtingen));
        OnPropertyChanged(nameof(KanExtraToelichtingToevoegen));
    }
    
    private async Task ShowOptiesPicker(Toelichting toelichting)
    {
        Criterium huidigCriterium = toelichting.GeselecteerdeOptie;
        
        List<int> alGeselecteerdeIds = Toelichtingen
            .Where(t => t != toelichting && t.GeselecteerdeOptie is Criterium)
            .Select(t => t.GeselecteerdeOptie.Id)
            .ToList();
    
        List<Criterium> beschikbareCriteriaVoorSelectie = _beschikbareCriteria
            .Where(c => !alGeselecteerdeIds.Contains(c.Id))
            .ToList();
    
        Dictionary<string, Criterium> criteriaDict = beschikbareCriteriaVoorSelectie
            .ToDictionary(c => c.Beschrijving, c => c);
    
        List<string> opties = criteriaDict.Keys.ToList();
    
        if (Application.Current?.MainPage == null) return;

        String selected = await Application.Current.MainPage.DisplayActionSheet(
            "Toelichting gekoppeld aan...",
            "Annuleren",
            null,
            opties.ToArray());

        if (selected != null && selected != "Annuleren" && criteriaDict.ContainsKey(selected))
        {
            Criterium gekozenCriterium = criteriaDict[selected];
            
            int index = Toelichtingen.IndexOf(toelichting);
            if (index != -1)
            {
                Toelichtingen.RemoveAt(index);
                toelichting.GeselecteerdeOptie = gekozenCriterium;
                Toelichtingen.Insert(index, toelichting);
            }
            
            KanExtraToelichtingToevoegen = Toelichtingen.Count < _beschikbareCriteria.Count;
            OnPropertyChanged(nameof(KanExtraToelichtingToevoegen));
        }
    }

    public bool CheckValidatie()
    {
        HashSet<Toelichting> nieuweTekstFouten = new HashSet<Toelichting>();
        HashSet<Toelichting> nieuweOptieFouten = new HashSet<Toelichting>();
        
        // Een ingevulde tekst moet een geselecteerde optie hebben en een geselecteerde optie moet een tekst hebben
        foreach (Toelichting t in Toelichtingen)
        {
            if (!string.IsNullOrWhiteSpace(t.Tekst) && t.GeselecteerdeOptie == null) // Wel tekst geen geselecteerde optie
            { nieuweOptieFouten.Add(t); }
            else if (string.IsNullOrWhiteSpace(t.Tekst) && t.GeselecteerdeOptie != null) // Geen tekst wel geselecteerde optie
            { nieuweTekstFouten.Add(t); }
        }
        
        // Een student moet minimaal 1 toelichting hebben
        if (!_isDocent && !Toelichtingen.Any(t => !string.IsNullOrWhiteSpace(t.Tekst) && t.GeselecteerdeOptie != null))
        {
            // De eerste toelichting ongeldig als niks is ingevult
            Toelichting eersteToelichting = Toelichtingen.FirstOrDefault();
            if (eersteToelichting != null)
            {
                nieuweTekstFouten.Add(eersteToelichting);
                nieuweOptieFouten.Add(eersteToelichting);
            } else return false; // Als er geen toelichtingen bestaan
        }
        
        OngeldigeTekstVelden = nieuweTekstFouten;
        OngeldigeOptieVelden = nieuweOptieFouten;
        
        // Check of alle toelichtingen correct zijn
        bool allesCorrect = OngeldigeTekstVelden.Count == 0 && OngeldigeOptieVelden.Count == 0;
        IsToelichtingInvalid = !allesCorrect;
        return allesCorrect;
    }
}