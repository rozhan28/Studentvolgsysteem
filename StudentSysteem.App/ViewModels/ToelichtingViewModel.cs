using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

namespace StudentSysteem.App.ViewModels;

// Handelt alle toelichtingen af (toevoegen + validatie)
public partial class ToelichtingViewModel : BasisViewModel, INotifyPropertyChanged
{
    private readonly IToelichtingService _toelichtingService;
    private readonly bool _isDocent;
    private readonly Prestatiedoel _prestatiedoel;
    private List<Criterium> _beschikbareCriteria;

    public ICommand VoegExtraToelichtingToeCommand { get; }
    public ICommand OptiesCommand { get; }
    
    [ObservableProperty]
    private bool kanExtraToelichtingToevoegen = true;
    
    [ObservableProperty]
    private bool isToelichtingInvalid;

    
    public event PropertyChangedEventHandler PropertyChanged;
    
    public ObservableCollection<Toelichting> Toelichtingen { get; }

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
            Notify(nameof(KanExtraToelichtingToevoegen));
            return;
        }
        
        Toelichtingen.Add(new Toelichting());
        KanExtraToelichtingToevoegen = Toelichtingen.Count < _beschikbareCriteria.Count;
        Notify(nameof(Toelichtingen));
        Notify(nameof(KanExtraToelichtingToevoegen));
    }
    
    private async Task ShowOptiesPicker(Toelichting toelichting)
    {
        Criterium huidigCriterium = toelichting.GeselecteerdeOptie;
        
        var alGeselecteerdeIds = Toelichtingen
            .Where(t => t != toelichting && t.GeselecteerdeOptie is Criterium)
            .Select(t => t.GeselecteerdeOptie.Id)
            .ToList();
    
        var beschikbareCriteriaVoorSelectie = _beschikbareCriteria
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
            Notify(nameof(KanExtraToelichtingToevoegen));
        }
    }

    public bool CheckValidatie()
    {
        if (_isDocent)
        {
            isToelichtingInvalid = true;
            return true;
        }

        if (Toelichtingen == null || !Toelichtingen.Any())
        {
            isToelichtingInvalid = false;
            return false;
        }
            
        bool result = Toelichtingen.All(t => IsToelichtingCorrect(t));
        isToelichtingInvalid = result;
        return result;
    }
    
    private bool IsToelichtingCorrect(Toelichting toelichting)
    {
        return !string.IsNullOrWhiteSpace(toelichting.Tekst) &&
               toelichting.GeselecteerdeOptie is Criterium;
    }
    
    private void Notify([CallerMemberName] string prop = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
}
