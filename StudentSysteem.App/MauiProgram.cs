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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

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
                    fonts.AddFont("Poppins-Regular.ttf", "Regular");
                    fonts.AddFont("Poppins-Bold.ttf", "Bold");
                    fonts.AddFont("Poppins-BoldItalic.ttf", "BoldItalic");
                    fonts.AddFont("Poppins-Italic.ttf", "Italic");
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


            // Repositories DEZE VOLGORDE AANHOUDEN AUB (anders crasht de solution!!)
            builder.Services.AddSingleton<ICriteriumRepository, CriteriumRepository>();
            builder.Services.AddSingleton<IPrestatiedoelRepository, PrestatiedoelRepository>();
            builder.Services.AddSingleton<IFeedbackRepository, FeedbackRepository>();
            builder.Services.AddSingleton<ILeeruitkomstRepository, LeeruitkomstRepository>();
            builder.Services.AddSingleton<IVaardigheidRepository, VaardigheidRepository>();
            builder.Services.AddSingleton<IClusterRepository, ClusterRepository>();
            builder.Services.AddSingleton<IStudentRepository, StudentRepository>();
            builder.Services.AddSingleton<IDocentRepository, DocentRepository>();

            // Services
            builder.Services.AddSingleton<IZelfEvaluatieService, ZelfEvaluatieService>();
            builder.Services.AddSingleton<IMeldingService, MeldingService>();
            builder.Services.AddSingleton<IFeedbackFormulierService, FeedbackFormulierService>();
            builder.Services.AddSingleton<IPrestatiedoelService, PrestatiedoelService>();
            builder.Services.AddSingleton<ILeeruitkomstService, LeeruitkomstService>();
            builder.Services.AddSingleton<IVaardigheidService, VaardigheidService>();
            builder.Services.AddSingleton<IToelichtingService, ToelichtingService>();
            builder.Services.AddSingleton<IClusterService, ClusterService>();
            builder.Services.AddSingleton<IStudentService, StudentService>();
            builder.Services.AddSingleton<IDocentService, DocentService>();

            // ViewModels
            builder.Services.AddSingleton<GlobaleViewModel>();
            builder.Services.AddTransient<LoginView>().AddTransient<LoginViewModel>();
            builder.Services.AddTransient<StartView>().AddTransient<StartViewModel>(); 
            builder.Services.AddTransient<FeedbackFormulierView>().AddTransient<FeedbackFormulierViewModel>();
            
            // Views
            builder.Services.AddTransient<FeedbackFormulierView>();

            var app = builder.Build();

            // Database initialisatie 
            using (var scope = app.Services.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<ICriteriumRepository>();
                scope.ServiceProvider.GetRequiredService<IPrestatiedoelRepository>();
                scope.ServiceProvider.GetRequiredService<IFeedbackRepository>();
                scope.ServiceProvider.GetRequiredService<ILeeruitkomstRepository>();
                scope.ServiceProvider.GetRequiredService<IVaardigheidRepository>();
                scope.ServiceProvider.GetRequiredService<IClusterRepository>();
                scope.ServiceProvider.GetRequiredService<IStudentRepository>();
                scope.ServiceProvider.GetRequiredService<IDocentRepository>();
            }
            return app;
        }
    }
}
