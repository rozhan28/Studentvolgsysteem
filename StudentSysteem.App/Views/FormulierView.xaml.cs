using Microsoft.Maui.Controls;
using StudentSysteem.App.ViewModels;

namespace StudentSysteem.App.Views
{
    public partial class FormulierView : ContentPage
    {
        private readonly FormulierViewModel _viewModel;
        
        public FormulierView(FormulierViewModel viewModel)
        {
            InitializeComponent();
            
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        // Code voor paginatitel en aanroep InitialiseerPagina()
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_viewModel.InitialiseerPaginaCommand.CanExecute(null))
            {
                _viewModel.InitialiseerPaginaCommand.Execute(null);
            }
            
            if (Shell.Current is AppShell shell)
            {
                // De titel wordt in de ViewModel gezet op basis van IsZelfEvaluatie
                _viewModel.UpdatePaginaTitel();
            
                // Titel doorgeven aan header
                this.Title = _viewModel.Titel;
                shell.UpdateTitel(_viewModel.Titel);
            }
        }
    }
}
