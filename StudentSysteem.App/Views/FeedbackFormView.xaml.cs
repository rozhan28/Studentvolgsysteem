using StudentSysteem.App.ViewModels;
using StudentVolgSysteem.Core.Services;

namespace StudentSysteem.App.Views
{
    public partial class FeedbackFormView : ContentPage
    {
        public FeedbackFormView(FeedbackFormViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;

            BindingContext = new FeedbackFormViewModel(new MockSelfReflectionService());
        }
    }
}

