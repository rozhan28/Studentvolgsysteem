using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using StudentVolgSysteem.App.Services;
using StudentSysteem.App.ViewModels;
using StudentSysteem.App.Views;
using StudentVolgSysteem.Core.Services;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Data;
using StudentSysteem.Core.Data.Repositories;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Services;

namespace StudentSysteem.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit();

            // Services
            builder.Services.AddSingleton<IZelfEvaluatieService, MockZelfEvaluatieService>();
            builder.Services.AddSingleton<INavigatieService, NavigatieService>();
            builder.Services.AddSingleton<IMeldingService, MeldingService>();

            // Repository & Formulierservice
            builder.Services.AddSingleton<IFeedbackRepository, FeedbackRepository>();
            builder.Services.AddSingleton<IFeedbackFormulierService, FeedbackFormulierService>();

            // Viewmodels
            builder.Services.AddTransient<FeedbackFormulierViewModel>();

            // Views
            builder.Services.AddTransient<FeedbackFormulierView>();

            //Laad database vuller
            DatabaseVuller vulTabel = new();
            vulTabel.TabelVuller();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
