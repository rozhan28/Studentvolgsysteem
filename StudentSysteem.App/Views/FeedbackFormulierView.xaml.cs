using StudentSysteem.App.ViewModels;
using StudentVolgSysteem.Core.Services;
using StudentSysteem.Core.Interfaces.Services;

namespace StudentSysteem.App.Views
{
    public partial class FeedbackFormulierView : ContentPage
    {
        public FeedbackFormulierView(
            IZelfEvaluatieService zelfEvaluatieService,
            INavigatieService navigatieService,
            IMeldingService meldingService,
            IFeedbackFormulierService feedbackFormulierService)
        {
            InitializeComponent();

            bool isDocent = GebruikerSessie.HuidigeRol == "Docent";

            BindingContext = new FeedbackFormulierViewModel(
                zelfEvaluatieService,
                navigatieService,
                meldingService,
                feedbackFormulierService,
                isDocent
            );
        }
    }
}
