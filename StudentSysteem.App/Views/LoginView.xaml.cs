using StudentSysteem.App.ViewModels;

namespace StudentSysteem.App.Views;

public partial class LoginView : ContentPage
{
    public LoginView(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}