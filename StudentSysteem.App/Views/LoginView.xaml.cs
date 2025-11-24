using StudentVolgSysteem.Core.Services;

namespace StudentSysteem.App.Views;

public partial class LoginView : ContentPage
{
    public LoginView()
    {
        InitializeComponent();
    }

    private async void OnStudentClicked(object sender, EventArgs e)
    {
        UserSession.LoginAls("Student");
        await Navigation.PushAsync(new StartView());
    }

    private async void OnDocentClicked(object sender, EventArgs e)
    {
        UserSession.LoginAls("Docent");
        await Navigation.PushAsync(new StartView());
    }
}
