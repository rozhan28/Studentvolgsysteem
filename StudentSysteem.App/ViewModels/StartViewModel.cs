using CommunityToolkit.Mvvm.Input;
using StudentSysteem.App.Views;
using StudentSysteem.Core.Models;

namespace StudentSysteem.App.ViewModels;

public partial class StartViewModel : BasisViewModel
{
    public GlobaleViewModel Globaal { get; set; }
    
    public bool IsStudent => Globaal.IngelogdeGebruiker?.Rol == Role.Student;
    public bool IsDocent => Globaal.IngelogdeGebruiker?.Rol == Role.Docent;

    public StartViewModel(GlobaleViewModel globaal)
    {
        this.Globaal = globaal;
        Titel = "Startpagina";
    }

    [RelayCommand]
    private async Task GaNaarZelfEvaluatie()
    {
        await Shell.Current.GoToAsync(nameof(FeedbackFormulierView));
    }

    [RelayCommand]
    private async Task GaNaarFeedback()
    {
        await Shell.Current.GoToAsync(nameof(FeedbackFormulierView));
    }
}