using StudentSysteem.App.ViewModels;

namespace StudentSysteem.App.Views;

public partial class StartView : ContentPage
{
    public StartView(StartViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

