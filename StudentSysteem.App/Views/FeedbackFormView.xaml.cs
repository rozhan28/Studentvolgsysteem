using StudentSysteem.Core.Models;
using StudentSysteem.App.ViewModels;
using StudentVolgSysteem.Core.Services;
using StudentSysteem.Core.Interfaces.Services;

namespace StudentSysteem.App.Views
{
    public partial class FeedbackFormView : ContentPage
    {
        public FeedbackFormView(
            IZelfevaluatieService zelfevaluatieService,
            INavigatieService navigatieService,
            IMeldingService meldingService)
        {
            InitializeComponent();

            bool isDocent = GebruikerSessie.HuidigeRol == "Docent";

            BindingContext = new FeedbackFormViewModel(
                zelfevaluatieService,
                navigatieService,
                meldingService,
                isDocent
            );
        }

        // In FeedbackFormView.xaml.cs

        private void OnVoegExtraFeedbackToeClicked(object sender, EventArgs e)
        {
            // Stap 1: Haal de Button op
            if (sender is Button button)
            {
                // Stap 2: De BindingContext van de Button zou het BeoordelingItem moeten zijn.
                // We gebruiken de volledige namespace voor zekerheid.
                if (button.BindingContext is StudentSysteem.Core.Models.BeoordelingItem item)
                {
                    item.VoegExtraToelichtingToe();
                    System.Diagnostics.Debug.WriteLine("SUCCESS: Extra toelichting toegevoegd via BindingContext!");
                    return; // Gelukt
                }
        
                // Stap 3: Als de BindingContext niet werkt (wat het probleem lijkt te zijn op Mac), 
                // proberen we de parent op te zoeken om het item te vinden.
                // We traverseren de visuele boom om te zoeken naar de DataTemplate's container
        
                Element parent = button.Parent;
                while (parent != null)
                {
                    if (parent.BindingContext is StudentSysteem.Core.Models.BeoordelingItem parentItem)
                    {
                        parentItem.VoegExtraToelichtingToe();
                        System.Diagnostics.Debug.WriteLine("SUCCESS: Extra toelichting toegevoegd via Parent Context!");
                        return; // Gelukt
                    }
                    parent = parent.Parent;
                }

                // Stap 4: Foutmelding als we het item niet kunnen vinden
                System.Diagnostics.Debug.WriteLine("FATAL ERROR: Kon het BeoordelingItem niet vinden in de Visual Tree voor de knop.");
            }
        }
    }
}