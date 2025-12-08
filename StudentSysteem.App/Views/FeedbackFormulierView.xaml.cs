using StudentSysteem.Core.Models;
using StudentSysteem.App.ViewModels;
using StudentVolgSysteem.Core.Services;
using StudentSysteem.Core.Interfaces.Services;

namespace StudentSysteem.App.Views
{
    public partial class FeedbackFormulierView : ContentPage
    {
        public FeedbackFormulierView(
            IZelfEvaluatieService zelfReflectieService,
            INavigatieService navigatieService,
            IMeldingService meldingService,
            IFeedbackFormulierService feedbackFormulierService)
        {
            InitializeComponent();

            bool isDocent = GebruikerSessie.HuidigeRol == "Docent";

            BindingContext = new FeedbackFormulierViewModel(
                zelfReflectieService,
                navigatieService,
                meldingService,
                feedbackFormulierService,
                isDocent
            );
        }
    }
}
