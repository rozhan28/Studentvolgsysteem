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
    }
}