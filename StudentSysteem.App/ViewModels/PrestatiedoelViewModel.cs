using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

namespace StudentSysteem.App.ViewModels;

// Toont een prestatiedoel
public partial class PrestatiedoelViewModel : BasisViewModel
{
    // Voor de student of docent check
    private readonly GlobaleViewModel _globaal;
    
    // Services voor de sub-ViewModels
    private readonly ICriteriumService _criteriumService;
    private readonly IToelichtingService _toelichtingService;
    
    // De sub-ViewModels
    public CriteriumViewModel Beoordeling { get; set; }
    public ToelichtingViewModel Feedback { get; set; }
    
    // Expander properties
    [ObservableProperty] private bool isExpanded;
    public string ExpanderTitel { get; }
    public string VaardigheidNaam { get; }
    
    // Vaardigheid properties
    public string VaardigheidBeschrijving { get; }
    public string HboiActiviteit { get; set; }
    
    // Prestatiedoel properties
    public string LeertakenUrl { get; }
    public string PrestatiedoelBeschrijving { get; set; }
    public string AiAssessmentScale { get; }
    public ICommand LeertakenCommand { get; }

    // Constructor
    public PrestatiedoelViewModel(
        BeoordelingStructuur structuur,
        ICriteriumService criteriumService,
        IToelichtingService toelichtingService,
        GlobaleViewModel globaal)
    {
        _criteriumService = criteriumService;
        _toelichtingService = toelichtingService;
        _globaal = globaal;
        
        // Data toewijzen vanuit BeoordelingStructuur.cs
        ExpanderTitel = $"{structuur.Proces.Naam} - {structuur.Stap.Naam}";
        HboiActiviteit = structuur.Vaardigheid.HboiActiviteit;
        VaardigheidNaam = structuur.Vaardigheid.VaardigheidNaam;
        VaardigheidBeschrijving = structuur.Vaardigheid.VaardigheidBeschrijving;
        PrestatiedoelBeschrijving = structuur.Doel.Beschrijving;
        LeertakenUrl = structuur.Vaardigheid.LeertakenUrl;
        AiAssessmentScale = structuur.Doel.AiAssessmentScale;
        
        Beoordeling = new CriteriumViewModel(structuur.Doel.Id, _criteriumService);
        Feedback = new ToelichtingViewModel(structuur.Doel.Id, _toelichtingService);
        
        LeertakenCommand = new Command<string>(async url => await OpenLeertakenUrl(url));
    }
    
    // Validatie vanuit de sub-ViewModels
    public bool Valideer()
    {
        bool isDocent = _globaal.IngelogdeGebruiker?.Rol == Role.Docent;
        
        bool beoordelingOk = Beoordeling.CheckValidatie();
        bool feedbackOk = Feedback.CheckValidatie(isDocent);
    
        return beoordelingOk && feedbackOk;
    }
    
    // Leertaken URL
    private async Task OpenLeertakenUrl(string url)
    {
        if (!string.IsNullOrWhiteSpace(url) &&
            Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            await Browser.Default.OpenAsync(url);
        }
    }
}