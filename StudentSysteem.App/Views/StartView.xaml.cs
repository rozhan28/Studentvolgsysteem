using StudentSysteem.App.ViewModels;

namespace StudentSysteem.App.Views;

public partial class StartView : ContentPage
{
    public StartView(StartViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
    
        if (BindingContext is StartViewModel vm && Shell.Current is AppShell shell)
        {
            shell.UpdateTitel(vm.Titel);
        }
    }
}

