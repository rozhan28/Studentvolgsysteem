using StudentSysteem.App.Views;

namespace StudentSysteem.App;

public partial class AppShell : Shell
{
    public string gebruikersNaam { get; set; }
    
    public AppShell()
    {
        InitializeComponent();
        
        //Wordt gebruikt in AppShell.xalm en deze code moet uiteindelijk vervangen worden bij UC Inloggen.       
        gebruikersNaam = "Naam";
        BindingContext = this;

        //Registreert routes voor navigatie naar het feedbackformulier (detailpagina)
        Routing.RegisterRoute(nameof(FeedbackFormulierView), typeof(FeedbackFormulierView));
    }
    
    //Navigeert naar VoortsgangsDashboardView
    private void OnDashboardClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Dashboard");
    }
}