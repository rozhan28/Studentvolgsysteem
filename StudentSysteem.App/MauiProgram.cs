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
                    fonts.AddFont("Poppins-Bold.ttf", "PoppinsBold");
                    fonts.AddFont("Poppins-BoldItalic.ttf", "PoppinsBoldItalic");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif


            // Config 
            var assembly = typeof(MauiProgram).Assembly;
            using var stream = assembly.GetManifestResourceStream("StudentSysteem.App.appsettings.json");

            if (stream == null)
                throw new InvalidOperationException("appsettings.json niet gevonden als Embedded Resource.");

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            builder.Configuration.AddConfiguration(config);

            // Helpers
            builder.Services.AddSingleton<DbConnectieHelper>();


            // Repositories DEZE VOLGORDE AANHOUDEN AUB (anders haalt hij de db niet op!!)
            builder.Services.AddSingleton<ICriteriumRepository, CriteriumRepository>();
            builder.Services.AddSingleton<IPrestatiedoelRepository, PrestatiedoelRepository>();
            builder.Services.AddSingleton<IFeedbackRepository, FeedbackRepository>();

            // Services
            builder.Services.AddSingleton<IZelfEvaluatieService, ZelfEvaluatieService>();
            builder.Services.AddSingleton<INavigatieService, NavigatieService>();
            builder.Services.AddSingleton<IMeldingService, MeldingService>();
            builder.Services.AddSingleton<IFeedbackFormulierService, FeedbackFormulierService>();
            builder.Services.AddSingleton<IPrestatiedoelService, PrestatiedoelService>();

            // ViewModels
            builder.Services.AddTransient<FeedbackFormulierViewModel>();

            // Views
            builder.Services.AddTransient<FeedbackFormulierView>();

            var app = builder.Build();

            // Database initialisatie 
            using (var scope = app.Services.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<ICriteriumRepository>();
                scope.ServiceProvider.GetRequiredService<IPrestatiedoelRepository>();
                scope.ServiceProvider.GetRequiredService<IFeedbackRepository>();
            }

            return app;
        }
    }
}
