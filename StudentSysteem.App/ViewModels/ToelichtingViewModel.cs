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
    private readonly bool _isDocent;
    private readonly int _prestatiedoelId;

    public ObservableCollection<Toelichting> Toelichtingen { get; }

    [ObservableProperty]
    private bool kanExtraToelichtingToevoegen;

    public ICommand VoegExtraToelichtingToeCommand { get; }

    // Constructor 
    public ToelichtingViewModel(
        int prestatiedoelId,
        IToelichtingService toelichtingService,
        bool isDocent)
    {
        _prestatiedoelId = prestatiedoelId;
        _toelichtingService = toelichtingService;
        _isDocent = isDocent;

        Toelichtingen = new ObservableCollection<Toelichting>
        {
            _toelichtingService.MaakNieuweToelichting()
        };

        VoegExtraToelichtingToeCommand =
            new Command(VoegExtraToelichtingToe);

        HookToelichtingen();
    }

    // Validatie
    public bool CheckValidatie()
    {
        if (_isDocent) return true;
        if (!Toelichtingen.Any()) return false;

        return Toelichtingen.All(t =>
            !string.IsNullOrWhiteSpace(t.Tekst) &&
            t.GeselecteerdeOptie != "Toelichting gekoppeld aan...");
    }

    private void VoegExtraToelichtingToe()
    {
        if (Toelichtingen.Count >=
            _toelichtingService.BerekenMaxToelichtingen(_prestatiedoelId))
            return;

        Toelichtingen.Add(
            _toelichtingService.MaakNieuweToelichting());
    }

    private void HookToelichtingen()
    {
        UpdateKanExtraToelichtingToevoegen();

        Toelichtingen.CollectionChanged += (_, _) =>
        {
            UpdateKanExtraToelichtingToevoegen();
        };
    }

    private void UpdateKanExtraToelichtingToevoegen()
    {
        KanExtraToelichtingToevoegen =
            Toelichtingen.Count <
            _toelichtingService.BerekenMaxToelichtingen(_prestatiedoelId);
    }
}
