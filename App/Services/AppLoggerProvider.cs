using MetroLog.MicrosoftExtensions;
using Microsoft.Extensions.Logging;

namespace App.Services
{
    public static class AppLoggerProvider
    {
        public static ILoggerFactory LoggerFactory { get; } =
            Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
        {
            builder.AddTraceLogger(_ => { });
        });

        public static ILogger CreateLogger<T>() =>
            LoggerFactory.CreateLogger<T>();
    }
}