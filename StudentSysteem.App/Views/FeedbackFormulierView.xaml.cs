using StudentSysteem.App.ViewModels;

namespace StudentSysteem.App.Views
{
    public partial class FeedbackFormulierView : ContentPage
    {
        public FeedbackFormulierView(FeedbackFormulierViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
        
        // Code voor paginatitel
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is FeedbackFormulierViewModel vm && Shell.Current is AppShell shell)
            {
                // Vraag de FeedbackFormulierViewModel om de Titel property te vullen
                vm.UpdateTitelGebaseerdOpStatus();
        
                // Geef die waarde door aan de header
                this.Title = vm.Titel;
                shell.UpdateTitel(vm.Titel);
            }
        }
    }
}