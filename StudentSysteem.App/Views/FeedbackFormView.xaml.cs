using StudentSysteem.App.ViewModels;
using StudentVolgSysteem.Core.Services;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Interfaces;

namespace StudentSysteem.App.Views
{
    public partial class FeedbackFormView : ContentPage
    {
        public FeedbackFormView(
            ISelfReflectionService reflectionService,
            INavigationService navigationService,
            IAlertService alertService)
        {
            InitializeComponent();

            bool isDocent = UserSession.HuidigeRol == "Docent";

            BindingContext = new FeedbackFormViewModel(
                reflectionService,
                navigationService,
                alertService,
                isDocent
            );
        }
    }
}

