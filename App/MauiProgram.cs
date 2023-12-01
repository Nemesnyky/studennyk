using App.ViewModels;
using App.Views;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("LibreFranklin-Medium.ttf", "LibreFranklinMedium");
                    fonts.AddFont("LibreFranklin-Bold.ttf", "LibreFranklinBold");
                    fonts.AddFont("LibreFranklin-SemiBold.ttf", "LibreFranklinSemiBold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<TaskListViewModel>();
            builder.Services.AddSingleton<AgendaViewModel>();
            builder.Services.AddSingleton<TaskDescriprionViewModel>();
            builder.Services.AddSingleton<Agenda>();
            builder.Services.AddSingleton<TaskDescription>();
            builder.Services.AddSingleton<MainPage>();

            return builder.Build();
        }
    }
}
