using CommunityToolkit.Mvvm.ComponentModel;
using StudentSysteem.Core.Models;

namespace StudentSysteem.App.ViewModels;

public partial class GlobaleViewModel : BasisViewModel
{
    [ObservableProperty] private Gebruiker ingelogdeGebruiker;
}