using StudentSysteem.Core.Services;

namespace StudentSysteem.App.Views;

public partial class StartView : ContentPage
{
    public StartView()
    {
        InitializeComponent();
        PasKnoppenAanOpRol();
    }

    private void PasKnoppenAanOpRol()
    {
        if (GebruikerSessie.HuidigeRol == "Student")
        {
            BtnZelfEvaluatie.IsVisible = true;
            BtnFeedback.IsVisible = true;
        }
        else if (GebruikerSessie.HuidigeRol == "Docent")
        {
            BtnZelfEvaluatie.IsVisible = false;
            BtnFeedback.IsVisible = true;
        }
    }

    private async void OpZelfEvaluatieGeklikt(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Feedbackformulier");
    }

    private async void OpFeedbackGeklikt(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Feedbackformulier");
    }
}

