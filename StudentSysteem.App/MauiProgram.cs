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
            
            // Service Interfaces
            builder.Services.AddSingleton<IZelfEvaluatieService, ZelfEvaluatieService>();
            builder.Services.AddSingleton<INavigatieService, NavigatieService>();
            builder.Services.AddSingleton<IMeldingService, MeldingService>();

            // Repository & Formulierservice
            builder.Services.AddSingleton<IFeedbackRepository, FeedbackRepository>();
            builder.Services.AddSingleton<IFeedbackFormulierService, FeedbackFormulierService>();

            // Data registratie
            builder.Services.AddSingleton<DatabaseVerbinding, DatabaseVuller>();

            // Viewmodels
            builder.Services.AddTransient<FeedbackFormulierViewModel>();

            // Views
            builder.Services.AddTransient<FeedbackFormulierView>();

            //Maak database tabbellen aan
            DatabaseVuller MaakTabellen = new();
            MaakTabellen.TabelLader();

            //Laad database vuller
            DatabaseVuller vulTabel = new();
            vulTabel.TabelVuller();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            
            var app = builder.Build();

            var databaseInitializer = app.Services.GetService<DatabaseVuller>();
            if (databaseInitializer != null)
            {
                 databaseInitializer.TabelVuller(); 
            }
            return app;
        }
    }
}