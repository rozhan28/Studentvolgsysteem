using StudentSysteem.App.ViewModels;
using StudentSysteem.App.Views;

namespace StudentSysteem.App;

public partial class App : Application
{
	public App(LoginViewModel viewModel)
	{
		InitializeComponent();
		
		MainPage = new LoginView(viewModel);
	}
}