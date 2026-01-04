using System.Collections.ObjectModel;
using System.Windows.Input;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

namespace StudentSysteem.App.ViewModels;

public class ToelichtingViewModel : BasisViewModel
{
    private readonly IToelichtingService _toelichtingService;
    private readonly bool _isDocent;

    public BeoordelingItem Beoordeling { get; }

    public ICommand VoegExtraToelichtingToeCommand { get; }

    public ToelichtingViewModel(
        BeoordelingItem beoordeling,
        IToelichtingService toelichtingService,
        bool isDocent)
    {
        Beoordeling = beoordeling;
        _toelichtingService = toelichtingService;
        _isDocent = isDocent;

        VoegExtraToelichtingToeCommand =
            new Command(VoegExtraToelichtingToe);

        HookToelichtingen();
    }

    // Validatie
    public bool ZijnAlleToelichtingenOk()
    {
        if (_isDocent) return true;
        if (Beoordeling.Toelichtingen == null || !Beoordeling.Toelichtingen.Any())
            return false;

        return Beoordeling.Toelichtingen.All(t =>
            !string.IsNullOrWhiteSpace(t.Tekst) &&
            t.GeselecteerdeOptie != "Toelichting gekoppeld aan...");
    }

    // Toevoegen
    private void VoegExtraToelichtingToe()
    {
        if (Beoordeling.Toelichtingen.Count >=
            _toelichtingService.BerekenMaxToelichtingen(Beoordeling.PrestatiedoelId))
            return;

        Beoordeling.Toelichtingen.Add(
            _toelichtingService.MaakNieuweToelichting());
    }

    private void HookToelichtingen()
    {
        UpdateKanToevoegen();

        Beoordeling.Toelichtingen.CollectionChanged += (_, _) =>
        {
            UpdateKanToevoegen();
        };
    }

    private void UpdateKanToevoegen()
    {
        Beoordeling.KanExtraToelichtingToevoegen =
            Beoordeling.Toelichtingen.Count <
            _toelichtingService.BerekenMaxToelichtingen(Beoordeling.PrestatiedoelId);
    }
}
