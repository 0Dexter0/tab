using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using TaskbarAlternativeBlazor.Native;
using TaskbarAlternativeBlazor.Taskbar.Config;
using TaskbarAlternativeBlazor.Taskbar.Services;

namespace TaskbarAlternativeBlazor;

internal sealed class MainConfigWatchService : IDisposable
{
    private readonly ConfigProvider _configProvider;

    private FileSystemWatcher? _watcher;
    private readonly Lock _gate = new();

    private CancellationTokenSource? _debounceCts;

    private string _lastNativeFingerprint = "";
    private string _lastWebFingerprint = "";

    public event Action<Taskbar.Config.BarWidgets[]>? WebConfigChanged;

    public MainConfigWatchService(ConfigProvider configProvider)
    {
        _configProvider = configProvider;
    }

    public async Task StartAsync()
    {
        var cfg = await _configProvider.GetConfigurationAsync();
        _lastNativeFingerprint = ComputeNativeFingerprint(cfg);
        _lastWebFingerprint = ComputeWebFingerprint(cfg);

        string path = Path.Combine(AppContext.BaseDirectory, "config.yaml");

        _watcher = new(path);
        _watcher.Changed += OnConfigChanged;
        _watcher.EnableRaisingEvents = true;
    }

    private void OnConfigChanged(object sender, FileSystemEventArgs e)
    {
        lock (_gate)
        {
            _debounceCts?.Cancel();
            _debounceCts?.Dispose();
            _debounceCts = new();

            var token = _debounceCts.Token;

            _ = Task.Run(async () =>
            {
                await Task.Delay(350, token);

                var cfg = await _configProvider.GetConfigurationAsync();

                var nativeFp = ComputeNativeFingerprint(cfg);
                var webFp = ComputeWebFingerprint(cfg);

                var nativeChanged = nativeFp != _lastNativeFingerprint;
                var webChanged = webFp != _lastWebFingerprint;

                if (!nativeChanged && !webChanged)
                {
                    return;
                }

                _lastNativeFingerprint = nativeFp;
                _lastWebFingerprint = webFp;

                if (nativeChanged)
                {
                    // RestartCurrentProcess();
                    Kernel32.RestartProcess();
                    return;
                }

                WebConfigChanged?.Invoke(cfg.Bars.Where(x => x.Enabled).Select(x => x.Widgets).ToArray());
            }, token);
        }
    }

    private static string ComputeNativeFingerprint(Configuration cfg) =>
        Sha256(cfg.Bars.Select(x => x with{ Widgets = null! }).ToString()!);

    private static string ComputeWebFingerprint(Configuration cfg) =>
        Sha256(cfg.Bars.Select(x => x.Widgets).ToString()!);

    private static string Sha256(string input) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(input)));

    private static void RestartCurrentProcess()
    {
        string? exePath = Environment.ProcessPath;

        if (string.IsNullOrWhiteSpace(exePath))
        {
            Environment.Exit(0);
        }

        ProcessStartInfo psi = new()
        {
            FileName = exePath,
            UseShellExecute = true,
            WorkingDirectory = AppContext.BaseDirectory
        };

        Process.Start(psi);
        Environment.Exit(0);
    }

    public void Dispose()
    {
        lock (_gate)
        {
            _debounceCts?.Cancel();
            _debounceCts?.Dispose();
            _debounceCts = null;
        }

        if (_watcher is not null)
        {
            _watcher.EnableRaisingEvents = false;
            _watcher.Changed -= OnConfigChanged;
            _watcher.Dispose();
            _watcher = null;
        }
    }
}