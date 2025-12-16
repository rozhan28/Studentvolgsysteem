using StudentSysteem.App.ViewModels;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Services;

namespace StudentSysteem.App.Views
{
    public partial class FeedbackFormulierView : ContentPage
    {
        public FeedbackFormulierView(
            IZelfEvaluatieService zelfEvaluatieService,
            INavigatieService navigatieService,
            IMeldingService meldingService,
            IFeedbackFormulierService feedbackFormulierService,
            IPrestatiedoelService prestatiedoelService)
        {
            InitializeComponent();

            bool isDocent = GebruikerSessie.HuidigeRol == "Docent";

            BindingContext = new FeedbackFormulierViewModel(
                zelfEvaluatieService,
                navigatieService,
                meldingService,
                feedbackFormulierService,
                prestatiedoelService,
                isDocent
            );
        }

    }
}