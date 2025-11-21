using Microsoft.Extensions.Logging;
using StudentSysteem.App.ViewModels;
using StudentSysteem.App.Views;
using StudentVolgSysteem.Core.Services;

namespace StudentSysteem.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Services
        builder.Services.AddSingleton<ISelfReflectionService, MockSelfReflectionService>();

        // ViewModels 
        builder.Services.AddSingleton<FeedbackFormViewModel>();

        // Views 
        builder.Services.AddSingleton<StartView>();
        builder.Services.AddSingleton<FeedbackFormView>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
