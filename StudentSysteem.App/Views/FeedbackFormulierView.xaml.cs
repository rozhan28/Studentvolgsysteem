using StudentSysteem.App.ViewModels;
using StudentVolgSysteem.Core.Services;
using StudentSysteem.Core.Interfaces.Services;

namespace StudentSysteem.App.Views
{
    public partial class FeedbackFormView : ContentPage
    {
        public FeedbackFormView(
            IZelfReflectieService zelfReflectieService,
            INavigatieService navigatieService,
            IMeldingService meldingService)
        {
            InitializeComponent();

            bool isDocent = GebruikerSessie.HuidigeRol == "Docent";

            BindingContext = new FeedbackFormViewModel(
                zelfReflectieService,
                navigatieService,
                meldingService,
                isDocent
            );
        }
    }
}
