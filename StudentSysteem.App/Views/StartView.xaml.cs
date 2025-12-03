using StudentVolgSysteem.Core.Services;
using Microsoft.Extensions.DependencyInjection;

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
            BtnZelfreview.IsVisible = true;
            BtnFeedback.IsVisible = true;
        }
        else if (GebruikerSessie.HuidigeRol == "Docent")
        {
            BtnZelfreview.IsVisible = false;
            BtnFeedback.IsVisible = true;
        }
    }

    private async void OnSelfReviewClicked(object sender, EventArgs e)
    {
        var page = App.Current.Handler.MauiContext.Services.GetRequiredService<FeedbackFormulierView>();
        await Navigation.PushAsync(page);
    }

    private async void OnFeedbackClicked(object sender, EventArgs e)
    {
        var page = App.Current.Handler.MauiContext.Services.GetRequiredService<FeedbackFormulierView>();
        await Navigation.PushAsync(page);
    }
}

