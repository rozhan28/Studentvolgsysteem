using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
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
    public ToelichtingViewModel Toelichting { get; set; }
    
    // Rode border als prestatiedoel validatie faalt
    [ObservableProperty]
    private bool isPrestatiedoelInvalid;

    // Expander properties
    [ObservableProperty] private bool isExpanded;
    public string ExpanderTitel { get; }
    public string VaardigheidNaam { get; }

    // Vaardigheid properties
    public string VaardigheidBeschrijving { get; }
    public string HboiActiviteit { get; set; }

    // Prestatiedoel properties
    public int VaardigheidId { get; }
    public string LeertakenUrl { get; }
    public string PrestatiedoelBeschrijving { get; set; }
    public string AiAssessmentScale { get; }
    public ICommand LeertakenCommand { get; }

    // Constructor
    public PrestatiedoelViewModel(
        BeoordelingStructuur beoordelingStructuur,
        ICriteriumService criteriumService,
        IToelichtingService toelichtingService,
        GlobaleViewModel globaal)
    {
        _criteriumService = criteriumService;
        _toelichtingService = toelichtingService;
        _globaal = globaal;

        bool isDocent = _globaal.IngelogdeGebruiker?.Rol == Role.Docent;

        // ID toewijzen zodat ViewModel weet welk prestatiedoel hij opslaat
        VaardigheidId = beoordelingStructuur.Vaardigheid.VaardigheidId;

        // Data toewijzen vanuit BeoordelingStructuur.cs
        ExpanderTitel = $"{beoordelingStructuur.Proces.Naam} | {beoordelingStructuur.Processtap.Naam} | {beoordelingStructuur.Vaardigheid.VaardigheidNaam}";
        HboiActiviteit = beoordelingStructuur.Vaardigheid.HboiActiviteit;
        VaardigheidNaam = beoordelingStructuur.Vaardigheid.VaardigheidNaam;
        VaardigheidBeschrijving = beoordelingStructuur.Vaardigheid.VaardigheidBeschrijving;
        PrestatiedoelBeschrijving = beoordelingStructuur.Prestatiedoel.Beschrijving;
        LeertakenUrl = beoordelingStructuur.Vaardigheid.LeertakenUrl;
        AiAssessmentScale = beoordelingStructuur.Prestatiedoel.AiAssessmentScale;

        Beoordeling = new CriteriumViewModel(beoordelingStructuur.Prestatiedoel, _criteriumService);
        Toelichting = new ToelichtingViewModel(beoordelingStructuur.Prestatiedoel, _toelichtingService, isDocent);

        // wijziging van prestatieniveau voor de expander header
        Beoordeling.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(CriteriumViewModel.PrestatieNiveau))
            {
                OnPropertyChanged(nameof(Beoordeling));
            }
        };

        LeertakenCommand = new Command<string>(async url => await OpenLeertakenUrl(url));
    }

    // Validatie vanuit de sub-ViewModels
    public bool Valideer()
    {
        bool validatieSucces = Beoordeling.CheckValidatie() & Toelichting.CheckValidatie();
        
        IsPrestatiedoelInvalid = !validatieSucces;
        return validatieSucces;
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

