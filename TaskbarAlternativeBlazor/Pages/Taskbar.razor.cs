using Microsoft.AspNetCore.Components;
using TaskbarAlternativeBlazor.Taskbar;
using TaskbarAlternativeBlazor.Taskbar.Services;
using TaskbarAlternativeBlazor.Widgets.ClockWidget;
using TaskbarAlternativeBlazor.Widgets.Common;

namespace TaskbarAlternativeBlazor.Pages;

public partial class Taskbar : ComponentBase
{
    private IWidget[] Widgets { get; set; } = null!;

    protected override void OnInitialized()
    {
        var config = new ConfigProvider().GetConfiguration();
        var widgetProvider = new WidgetProvider([new ClockWidget { Name = "clock" }]);

        var widgetNames = config.Bars[0].Widgets.Center;

        Widgets = widgetNames.Select(widgetProvider.GetWidget).ToArray();
    }

    private static object? _shellInstance;
    private static Type? _shellType;

    private void LaunchApp(string uriOrCommand)
    {
        // Offload to background thread to keep UI responsive
        Task.Run(() =>
        {
            try
            {
                using (var process = new System.Diagnostics.Process())
                {
                    process.StartInfo = new(uriOrCommand)
                    {
                        UseShellExecute = true,
                        WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal
                    };
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                // Fallback for MAUI schemes if direct process start fails
                MainThread.BeginInvokeOnMainThread(async void () => {
                    try
                    {
                        await Launcher.Default.OpenAsync(new Uri(uriOrCommand));
                    }
                    catch
                    {
                        Console.WriteLine($"Error launching app: {ex.Message}");
                    }
                });
            }
        });
    }

    private void OpenStartMenu()
    {
        // Direct explorer call for Start Menu
        _ = Task.Run(() => ShellExecute("explorer.exe", "shell:::{2559a1f8-21d7-11d4-bdaf-00c04f60b9f0}"));
    }

    private void OpenSearch()
    {
        LaunchApp("search-ms:");
    }

    private void ToggleTray()
    {
        // Mock toggle
    }

    private void ShowDesktop()
    {
        _ = Task.Run(() =>
        {
            try
            {
                if (_shellType == null)
                {
                    _shellType = Type.GetTypeFromProgID("Shell.Application");
                }

                if (_shellType != null)
                {
                    _shellInstance ??= Activator.CreateInstance(_shellType);

                    if (_shellInstance != null)
                    {
                        _shellType.InvokeMember("MinimizeAll", System.Reflection.BindingFlags.InvokeMethod, null, _shellInstance, null);
                    }
                }
            }
            catch
            {
                // ignored
            }
        });
    }

    private void ShellExecute(string file, string args)
    {
        using (var process = new System.Diagnostics.Process())
        {
            process.StartInfo = new(file, args)
            {
                UseShellExecute = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal
            };
            process.Start();
        }
    }
}