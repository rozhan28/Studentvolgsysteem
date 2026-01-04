using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StudentSysteem.App.ViewModels;

// Handelt alle toelichtingen af (toevoegen + validatie)
public partial class ToelichtingViewModel : BasisViewModel
{
    private readonly IToelichtingService _toelichtingService;
    private readonly bool _isDocent;

    public ObservableCollection<Toelichting> Toelichtingen { get; }
    public int PrestatiedoelId { get; }

    public ICommand VoegExtraToelichtingToeCommand { get; }

    public bool KanExtraToelichtingToevoegen
    {
        get => Toelichtingen.Count <
               _toelichtingService.BerekenMaxToelichtingen(PrestatiedoelId);
    }

    public ToelichtingViewModel(
        ObservableCollection<Toelichting> toelichtingen,
        int prestatiedoelId,
        IToelichtingService toelichtingService,
        bool isDocent)
    {
        Toelichtingen = toelichtingen;
        PrestatiedoelId = prestatiedoelId;
        _toelichtingService = toelichtingService;
        _isDocent = isDocent;

        VoegExtraToelichtingToeCommand =
            new Command(VoegExtraToelichtingToe);

        Toelichtingen.CollectionChanged += (_, _) =>
            OnPropertyChanged(nameof(KanExtraToelichtingToevoegen));
    }

    public bool ZijnAlleToelichtingenOk()
    {
        if (_isDocent) return true;
        if (!Toelichtingen.Any()) return false;

        return Toelichtingen.All(t =>
            !string.IsNullOrWhiteSpace(t.Tekst) &&
            t.GeselecteerdeOptie != "Toelichting gekoppeld aan...");
    }

    private void VoegExtraToelichtingToe()
    {
        if (!KanExtraToelichtingToevoegen) return;

        Toelichtingen.Add(_toelichtingService.MaakNieuweToelichting());
        OnPropertyChanged(nameof(KanExtraToelichtingToevoegen));
    }
}
