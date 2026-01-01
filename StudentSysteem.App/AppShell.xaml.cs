using StudentSysteem.App.ViewModels;
using StudentSysteem.App.Views;

namespace StudentSysteem.App;

public partial class AppShell : Shell
{
    public AppShell(GlobaleViewModel globaal)
    {
        InitializeComponent();
        BindingContext = globaal;
        
        this.Navigated += (s, e) =>
        {
            if (CurrentPage != null && CurrentPage is not Views.FeedbackFormulierView)
            {
                UpdateTitel(CurrentPage.Title);
            }
        };
        
        Routing.RegisterRoute("Login", typeof(LoginView));
        Routing.RegisterRoute(nameof(StartView), typeof(StartView));
        Routing.RegisterRoute(nameof(FeedbackFormulierView), typeof(FeedbackFormulierView));
    }
    
    private void OpDashboardGeklikt(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Login");
    }
    
    public void UpdateTitel(string nieuweTitel)
    {
        ShellTitelLabel.Text = nieuweTitel;
    }
}