using StudentSysteem.Core.Services;

namespace StudentSysteem.App.ViewModels;

public class LoginViewModel : BasisViewModel
{
    private readonly GlobaleViewModel _globaal;
    
    public LoginViewModel(GlobaleViewModel globaal)
    {
        _globaal = globaal;
    }
    
    private void LoginStudent()
    {
        // 1. Gebruik je bestaande Service
        GebruikerSessie.LoginAls("Student");

        // 2. Update de globale staat voor de UI (de header)
        // Straks haal je 'Sanne' hier op uit je Repository
        _globaal.gebruikersNaam = "Sanne"; 

        // 3. Switch naar de AppShell (volgens docent-methode)
        Application.Current.MainPage = new AppShell();
    }
    
    private void LoginDocent()
    {
        GebruikerSessie.LoginAls("Docent");
        _globaal.gebruikersNaam = "Docent Naam";
        Application.Current.MainPage = new AppShell();
    }
}