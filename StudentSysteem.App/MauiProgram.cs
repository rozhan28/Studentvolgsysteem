using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using StudentVolgSysteem.App.Services;
using StudentSysteem.App.ViewModels;
using StudentSysteem.App.Views;
using StudentVolgSysteem.Core.Services;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Data;

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

            // ⭐ Services
            builder.Services.AddSingleton<IZelfReflectieService, MockZelfReflectieService>();
            builder.Services.AddSingleton<INavigatieService, NavigatieService>();
            builder.Services.AddSingleton<IMeldingService, MeldingService>();

            // ⭐ Viewmodels
            builder.Services.AddTransient<FeedbackFormulierViewModel>();

            // ⭐ Views
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
