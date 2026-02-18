using YamlDotNet.Serialization;

namespace TaskbarAlternativeBlazor.Widgets.Common;

public abstract class WidgetBase<TSettings> : IDisposable
    where TSettings : class, IWidgetSettings
{
    private readonly string _widgetsFolder = Path.Combine(AppContext.BaseDirectory,  "Widgets");
    private readonly List<FileSystemWatcher> _watchers = [];

    public required string Name { get; init; }

    public abstract Type Component { get; }

    [YamlIgnore]
    public Dictionary<string, object> ComponentSettings => new()
    {
        ["Settings"] = Settings
    };

    public required TSettings Settings { get; set; }

    public void Initialize()
    {
        string widgetFolder = Path.Combine(_widgetsFolder, Name);

        if (Directory.Exists(widgetFolder))
        {
            string[] resources =
            [
                ..Directory.GetFiles(widgetFolder, "*.css"),
                ..Directory.GetFiles(widgetFolder, "*.js")
            ];

            foreach (string resource in resources)
            {
                FileSystemWatcher watcher = new(resource);
                watcher.Changed += OnResourceChanged;
                watcher.EnableRaisingEvents = true;
                _watchers.Add(watcher);
            }
        }
    }

    private void OnResourceChanged(object sender, FileSystemEventArgs e)
    {
        // do nothing
    }

    public void Dispose()
    {
        _watchers.ForEach(x => x.Dispose());
        _watchers.Clear();
    }
}