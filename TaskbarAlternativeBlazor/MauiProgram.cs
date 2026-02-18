using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Embedding;
using Microsoft.Maui.LifecycleEvents;
using TaskbarAlternativeBlazor.Taskbar;
using TaskbarAlternativeBlazor.Taskbar.Services;

#if WINDOWS
using Microsoft.UI.Windowing;
#endif

namespace TaskbarAlternativeBlazor;

public static partial class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiEmbeddedApp<MyApp>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-SemiBold.ttf", "OpenSansSemiBold");
            });

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddSingleton(AppInfo.Current);

        builder.Services.AddSingleton<ConfigProvider>();
        builder.Services.AddSingleton<WidgetSettingsWatchService>();
        builder.Services.AddSingleton<MainConfigWatchService>();
        builder.Services.AddTransient<IWidgetProvider, WidgetProvider>();

#if DEBUG

        // Caution: Recommended to enable Developer Tools only for development!!!
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.ConfigureLifecycleEvents(events =>
        {
#if WINDOWS
            events.AddWindows(windows =>
            {
                windows.OnWindowCreated(window =>
                {
                    var appWindow = window.AppWindow;

                    if (appWindow.Presenter is OverlappedPresenter p)
                    {
                        // Removes the white resize border and the title bar.
                        // (Note: also removes standard resize handles unless you re-implement them.)
                        p.SetBorderAndTitleBar(false, false);
                    }
                });
            });
#endif
        });

        return builder.Build();
    }
}