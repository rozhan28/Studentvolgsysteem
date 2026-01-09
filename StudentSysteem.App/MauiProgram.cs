using System;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using StudentSysteem.App.Views;
using StudentSysteem.App.ViewModels;
using StudentSysteem.Core.Services;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Data.Repositories;

namespace StudentSysteem.App;

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

        // Repositories
        builder.Services.AddSingleton<IPrestatiedoelRepository, PrestatiedoelRepository>();
        builder.Services.AddSingleton<ICriteriumRepository, CriteriumRepository>();
        builder.Services.AddSingleton<IProcesRepository, ProcesRepository>();
        builder.Services.AddSingleton<IProcesstapRepository, ProcesstapRepository>();
        builder.Services.AddSingleton<ILeeruitkomstRepository, LeeruitkomstRepository>();
        builder.Services.AddSingleton<IVaardigheidRepository, VaardigheidRepository>();
        builder.Services.AddSingleton<IClusterRepository, ClusterRepository>();
        builder.Services.AddSingleton<IStudentRepository, StudentRepository>();
        builder.Services.AddSingleton<IDocentRepository, DocentRepository>();
        builder.Services.AddSingleton<IFeedbackRepository, FeedbackRepository>();

        // Services
        builder.Services.AddSingleton<IFormulierService, FormulierService>();
        builder.Services.AddSingleton<IPrestatiedoelService, PrestatiedoelService>();
        builder.Services.AddSingleton<ICriteriumService, CriteriumService>();
        builder.Services.AddSingleton<IProcesService, ProcesService>();
        builder.Services.AddSingleton<IProcesstapService, ProcesstapService>();
        builder.Services.AddSingleton<ILeeruitkomstService, LeeruitkomstService>();
        builder.Services.AddSingleton<IVaardigheidService, VaardigheidService>();
        builder.Services.AddSingleton<IClusterService, ClusterService>();
        builder.Services.AddSingleton<IStudentService, StudentService>();
        builder.Services.AddSingleton<IDocentService, DocentService>();
        builder.Services.AddSingleton<IToelichtingService, ToelichtingService>();
        builder.Services.AddSingleton<IBeoordelingStructuurService, BeoordelingStructuurService>();

        // ViewModels
        builder.Services.AddSingleton<GlobaleViewModel>();
        builder.Services.AddTransient<LoginView>().AddTransient<LoginViewModel>();
        builder.Services.AddTransient<StartView>().AddTransient<StartViewModel>();
        builder.Services.AddTransient<FormulierView>().AddTransient<FormulierViewModel>();
        var app = builder.Build();


        using (var scope = app.Services.CreateScope())
        {
            scope.ServiceProvider.GetRequiredService<IPrestatiedoelRepository>();
            scope.ServiceProvider.GetRequiredService<ICriteriumRepository>();
            scope.ServiceProvider.GetRequiredService<IProcesRepository>();
            scope.ServiceProvider.GetRequiredService<IProcesstapRepository>();
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
