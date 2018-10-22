using System;
using Avalonia;
using Avalonia.Logging.Serilog;

namespace SCAS
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildAvaloniaApp().Start<ControlWindow>();
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToDebug();
    }
}
