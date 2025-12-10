using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StudentVolgSysteem.App.Services;
using StudentSysteem.App.ViewModels;
using StudentSysteem.App.Views;
using StudentSysteem.Core.Data;
using StudentSysteem.Core.Data.Helpers;
using StudentVolgSysteem.Core.Services;
using StudentSysteem.Core.Interfaces.Services;
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
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Poppins-Regular.ttf", "PoppinsRegular");
                fonts.AddFont("Poppins-BoldItalic.ttf", "PoppinsBoldItalic");
                fonts.AddFont("Poppins-Bold.ttf", "PoppinsBold");
            });
#if DEBUG
            builder.Logging.AddDebug();
#endif
            
            // Database 
var a = typeof(MauiProgram).Assembly;
using var stream = a.GetManifestResourceStream("StudentSysteem.App.appsettings.json");

if (stream != null)
{
    IConfiguration config = new ConfigurationBuilder()
        .AddJsonStream(stream)
        .Build();

    builder.Configuration.AddConfiguration(config);
}
else
{
 
    throw new InvalidOperationException("Configuratiebestand 'appsettings.json' niet gevonden als Embedded Resource.");
}
            
            // Services
            builder.Services.AddSingleton<IZelfEvaluatieService, ZelfEvaluatieService>();
            builder.Services.AddSingleton<INavigatieService, NavigatieService>();
            builder.Services.AddSingleton<IMeldingService, MeldingService>();
            builder.Services.AddSingleton<IFeedbackFormulierService, FeedbackFormulierService>();
            builder.Services.AddSingleton<DbConnectieHelper>();

            // Repository
            builder.Services.AddSingleton<IFeedbackRepository, FeedbackRepository>();
            builder.Services.AddSingleton<ICriteriumRepository, CriteriumRepository>();

            // Viewmodels
            builder.Services.AddTransient<FeedbackFormulierViewModel>();

            // Views
            builder.Services.AddTransient<FeedbackFormulierView>();
            return builder.Build();
        }
    }
}