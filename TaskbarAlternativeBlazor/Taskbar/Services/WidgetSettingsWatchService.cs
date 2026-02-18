using System.Collections.Concurrent;

namespace TaskbarAlternativeBlazor.Taskbar.Services;

internal sealed class WidgetSettingsWatchService : IDisposable
{
    private readonly string _widgetsFolder = Path.Combine(AppContext.BaseDirectory, "Widgets");

    private FileSystemWatcher? _watcher;

    private readonly ConcurrentDictionary<string, CancellationTokenSource> _debounceByFile =
        new(StringComparer.OrdinalIgnoreCase);

    public event Action<string>? WidgetConfigChanged;

    public void Start()
    {
        Directory.CreateDirectory(_widgetsFolder);

        _watcher = new(_widgetsFolder, "*.yaml")
        {
            IncludeSubdirectories = false,
            NotifyFilter = NotifyFilters.FileName
        };

        _watcher.Changed += OnChanged;
        _watcher.EnableRaisingEvents = true;
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        NotifyDebounced(Path.GetFileName(e.FullPath));
    }

    private void NotifyDebounced(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return;
        }

        CancellationTokenSource cts = new();

        _debounceByFile.AddOrUpdate(fileName,
            _ => cts,
            (_, old) =>
            {
                old.Cancel();
                old.Dispose();

                return cts;
            });

        _ = Task.Run(async () =>
        {
            await Task.Delay(250, cts.Token);

            WidgetConfigChanged?.Invoke(fileName);

            if (_debounceByFile.TryRemove(fileName, out var removed))
            {
                removed.Dispose();
            }
        }, cts.Token);
    }

    public void Dispose()
    {
        foreach (var kv in _debounceByFile)
        {
            kv.Value.Cancel();
            kv.Value.Dispose();
        }

        _debounceByFile.Clear();

        if (_watcher is null)
        {
            return;
        }

        _watcher.EnableRaisingEvents = false;
        _watcher.Changed -= OnChanged;
        _watcher.Dispose();
        _watcher = null;
    }
}