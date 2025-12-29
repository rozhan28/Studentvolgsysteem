using StudentSysteem.App.Views;

namespace StudentSysteem.App;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute("Login", typeof(LoginView));
        Routing.RegisterRoute(nameof(StartView), typeof(StartView));
        Routing.RegisterRoute(nameof(FeedbackFormulierView), typeof(FeedbackFormulierView));
    }
    
    private void OpDashboardGeklikt(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Login");
    }
}