using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StudentSysteem.App.Views;
using StudentSysteem.App.ViewModels;
using StudentSysteem.Core.Services;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Data.Repositories;

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

            // Prestatiedoelen
            builder.Services.AddSingleton<IPrestatiedoelRepository, PrestatiedoelRepository>();
            builder.Services.AddSingleton<IPrestatiedoelService, PrestatiedoelService>();


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