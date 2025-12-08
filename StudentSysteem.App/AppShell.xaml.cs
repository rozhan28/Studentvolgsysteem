using StudentSysteem.App.ViewModels;

namespace StudentSysteem.App;

public partial class AppShell : Shell
{
    public string PaginaTitel { get; set; }
    public string gebruikersNaam { get; set; }
    
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(FeedbackFormViewModel), typeof(FeedbackFormViewModel));
        gebruikersNaam = "Naam";
        PaginaTitel = "Dashboard";
        BindingContext = this;
    }
    
    private void OnDashboardClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Dashboard");
    }
}
