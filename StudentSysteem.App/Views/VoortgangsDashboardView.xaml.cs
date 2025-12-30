using StudentSysteem.App.ViewModels;

namespace StudentSysteem.App.Views;

public partial class VoortgangsDashboardView : ContentPage
{
    public VoortgangsDashboardView(VoortgangsDashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}