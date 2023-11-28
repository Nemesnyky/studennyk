using App.Services;
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
            builder.Services.AddSingleton<RestService>(Temporary.Initializators.InitializeInMemoryDataBase(new RestService()));
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
