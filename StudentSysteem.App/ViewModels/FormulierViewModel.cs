using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

namespace StudentSysteem.App.ViewModels;

// Toont een lijst van prestatiedoelen
[QueryProperty(nameof(IsZelfEvaluatie), "isZelf")]
[QueryProperty(nameof(OntvangerId), "ontvangerId")]
public partial class FormulierViewModel : BasisViewModel
{
    // Voor de gebruiker-check
    private readonly GlobaleViewModel _globaal;
    [ObservableProperty] private int ontvangerId;
    
    // Services voor de sub-ViewModels
    private readonly ICriteriumService _criteriumService;
    private readonly IToelichtingService _toelichtingService;
    private readonly IFormulierService _formulierService;
    
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
        IBeoordelingStructuurService beoordelingStructuurService,
        IFormulierService formulierService,
        GlobaleViewModel globaal)
    {
        _criteriumService = criteriumService;
        _toelichtingService = toelichtingService;
        _beoordelingStructuurService = beoordelingStructuurService;
        _formulierService = formulierService;
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
    
    // Laad methode voor BeoordelingStructuur.cs
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
    
    // Opslaan methode
    private async Task BewaarIngevuldFormulierAsync()
    {
        // Alles valideren
        foreach (PrestatiedoelViewModel item in FormulierItems)
        {
            if (!item.Valideer()) 
            {
                StatusMelding = "Controleer alle velden a.u.b.";
                return;
            }
        }

        // Service vragen om op te slaan
        try 
        {
            int feedbackgeverId = _globaal.IngelogdeGebruiker.Id;
            int ontvangerId = 1;

            foreach (PrestatiedoelViewModel prestatiedoelItems in FormulierItems)
            {
                // Pak de toelichtingen uit de rij
                List<Toelichting> toelichtingenVanPrestatiedoelItems = prestatiedoelItems.Feedback.Tekstvakken.ToList();
                
                // Opslaan via service
                _formulierService.SlaToelichtingenOp(
                    toelichtingenVanPrestatiedoelItems, 
                    ontvangerId, 
                    prestatiedoelItems.PrestatiedoelId,
                    feedbackgeverId);
            }

            StatusMelding = "Succesvol opgeslagen!";
            await Shell.Current.DisplayAlert("Succes", "De feedback is succesvol opgeslagen.", "OK");
        }
        catch (Exception ex)
        {
            StatusMelding = $"Fout bij opslaan: {ex.Message}";
            Debug.WriteLine(ex);
        }
    }
}