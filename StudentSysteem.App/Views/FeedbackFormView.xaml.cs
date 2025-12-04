using System.ComponentModel;
using StudentSysteem.App.Models;
using StudentSysteem.App.ViewModels;
using StudentVolgSysteem.Core.Services;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

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

        private void OnVoegExtraFeedbackToeClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var parent = button?.Parent;
    
            if (parent?.BindingContext is BeoordelingItem item)
            {
                item.VoegExtraToelichtingToe(); // Gebruik de nieuwe method
            }
        }
    }
}