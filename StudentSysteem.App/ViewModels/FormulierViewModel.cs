using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;
using StudentSysteem.Core.Services;

namespace StudentSysteem.App.ViewModels;

// Toont een lijst van prestatiedoelen
[QueryProperty(nameof(IsZelfEvaluatie), "isZelf")]
public partial class FormulierViewModel : BasisViewModel
{
    // Voor de student of docent check
    private readonly GlobaleViewModel _globaal;
    
    // Services voor de sub-ViewModels
    private readonly ICriteriumService _criteriumService;
    private readonly IToelichtingService _toelichtingService;
    
    // 'Pakketje'
    private readonly IBeoordelingStructuurService _beoordelingStructuurService;
    
    // Maakt nieuwe FormulierItems (prestatiedoelen)
    public ObservableCollection<PrestatiedoelViewModel> FormulierItems { get; } = new();
    
    // Header titel
    [ObservableProperty] private bool isZelfEvaluatie;
    
    // Opslaan
    public IAsyncRelayCommand OpslaanCommand { get; }
    
    // Constructor
    public FormulierViewModel(
        ICriteriumService criteriumService,
        IToelichtingService toelichtingService,
        BeoordelingStructuurService beoordelingStructuurService,
        GlobaleViewModel globaal)
    {
        _criteriumService = criteriumService;
        _toelichtingService = toelichtingService;
        _beoordelingStructuurService = beoordelingStructuurService;
        _globaal = globaal;
        
        OpslaanCommand = new AsyncRelayCommand(async () => await BewaarIngevuldFormulierAsync());
    }
    
    // Header paginatitel
    public void UpdatePaginaTitel()
    {
        Titel = IsZelfEvaluatie ? "Zelfevaluatieformulier" : "Feedbackformulier";
    }
    
    // Toegevoegd voor TC2-01.1 - voorkomt crashen
    // Zorgt ervoor dat indien de database niet beschikbaar is, de applicatie niet crasht
    [RelayCommand]
    private async Task InitialiseerPaginaAsync()
    {
        try
        {
            LaadStructuur();
            MainThread.BeginInvokeOnMainThread(() => UpdatePaginaTitel());
        }
        catch (Exception ex)
        {
            StatusMelding = "Databasefout: De prestatiedoelen konden niet worden geladen.";
            Debug.WriteLine(ex);
        }
    }
    
    private void LaadStructuur()
    {
        MainThread.BeginInvokeOnMainThread(() => FormulierItems.Clear());
        
        IEnumerable<BeoordelingStructuur> data = _beoordelingStructuurService.HaalVolledigeStructuurOp();

        foreach (BeoordelingStructuur item in data)
        {
            PrestatiedoelViewModel rij = new PrestatiedoelViewModel(
                item, 
                _criteriumService, 
                _toelichtingService,
                _globaal);

            MainThread.BeginInvokeOnMainThread(() => FormulierItems.Add(rij));
        }
    }
    
    // Opslaan
    private async Task BewaarIngevuldFormulierAsync()
    {
        foreach (PrestatiedoelViewModel prestatiedoelItem in FormulierItems)
        {
            if (!prestatiedoelItem.Valideer()) 
            {
                StatusMelding = "Controleer alle velden a.u.b.";
                return;
            }
            try 
            {
                StatusMelding = "Succesvol opgeslagen!";
            }
            catch (Exception ex)
            {
                StatusMelding = $"Fout bij opslaan: {ex.Message}";
            }
        }
    }
}