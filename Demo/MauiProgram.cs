using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;

namespace ToDoList
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
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("MauiSampleFontIcon.ttf", "MauiSampleFontIcon");
                    fonts.AddFont("Roboto-Regular.ttf", "Roboto-Regular");
                    fonts.AddFont("Roboto-Medium.ttf", "Roboto-Medium");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.ConfigureSyncfusionCore();
            return builder.Build();
        }
    }
}
