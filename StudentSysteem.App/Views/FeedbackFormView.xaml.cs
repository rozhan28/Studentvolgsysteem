using StudentSysteem.App.ViewModels;
using StudentVolgSysteem.Core.Services;

namespace StudentSysteem.App.Views
{
    public partial class FeedbackFormView : ContentPage
    {
        public FeedbackFormView()
        {
            InitializeComponent();

            var service = App.Current.Handler.MauiContext.Services.GetService<ISelfReflectionService>();

            bool isDocent = UserSession.HuidigeRol == "Docent";

            BindingContext = new FeedbackFormViewModel(service, isDocent);
        }
    }
}


