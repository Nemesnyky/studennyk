using MetroLog.MicrosoftExtensions;
using Microsoft.Extensions.Logging;

namespace App.Services
{
    public static class AppLoggerProvider
    {
        public static ILoggerFactory LoggerFactory { get; } =
            Microsoft.Extensions.Logging.LoggerFactory.Create(builder
                =>
#if DEBUG
        builder.AddTraceLogger(_ => { }));
#else
        builder.AddStreamingFileLogger(options =>
        options.FolderPath = ""/*PATH*/));  //TODO: Add path
#endif
        public static ILogger CreateLogger<T>() =>
            LoggerFactory.CreateLogger<T>();
    }
}