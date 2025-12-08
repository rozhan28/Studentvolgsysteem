using System.Diagnostics;
using StudentSysteem.App.ViewModels;

namespace StudentSysteem.App;

public partial class AppShell : Shell
{
    public string gebruikersNaam { get; set; }
    
    public AppShell()
    {
        InitializeComponent();
        this.Title = "Feedbackformulier";
        Routing.RegisterRoute(nameof(FeedbackFormViewModel), typeof(FeedbackFormViewModel));
        
        gebruikersNaam = "Naam";
        BindingContext = this;
    }
    
    private void OnDashboardClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Dashboard");
    }
}