using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using StudentVolgSysteem.App.Services;
using StudentSysteem.App.ViewModels;
using StudentSysteem.App.Views;
using StudentVolgSysteem.Core.Services;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Interfaces;

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
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Services
            builder.Services.AddSingleton<ISelfReflectionService, MockSelfReflectionService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<IAlertService, AlertService>();

            // Viewmodels
            builder.Services.AddTransient<FeedbackFormViewModel>();

            // Viems
            builder.Services.AddTransient<FeedbackFormView>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
