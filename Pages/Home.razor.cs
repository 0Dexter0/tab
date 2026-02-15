using Microsoft.AspNetCore.Components;
using Timer = System.Timers.Timer;

namespace WinUIApp1.Pages;

public partial class Home : ComponentBase, IDisposable
{
    private DateTime _currentTime = DateTime.Now;
    private Timer? _timer;

    protected override void OnInitialized()
    {
        _timer = new(500);
        _timer.Elapsed += (s, e) =>
        {
            _currentTime = DateTime.Now;
            InvokeAsync(StateHasChanged);
        };
        _timer.Start();
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

    public void Dispose()
    {
        _timer?.Dispose();
    }
}