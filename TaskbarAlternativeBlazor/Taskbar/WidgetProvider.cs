using TaskbarAlternativeBlazor.Widgets.Common;

namespace TaskbarAlternativeBlazor.Taskbar;

internal sealed class WidgetProvider : IWidgetProvider
{
    private readonly string _widgetsFolder = Path.Combine(AppContext.BaseDirectory, "Widgets");

    private readonly IWidgetDeserializer[] _deserializers;

    public WidgetProvider(IEnumerable<IWidgetDeserializer> deserializers)
    {
        _deserializers = deserializers.ToArray();
    }

    public IWidget? Get(string name)
    {
        Directory.CreateDirectory(_widgetsFolder);

        string widgetFile = Path.Combine(_widgetsFolder, $"{name}.yaml");
        if (!File.Exists(widgetFile))
        {
            return null;
        }

        using StreamReader reader = new(File.OpenRead(widgetFile));

        string type = reader.ReadLine() ?? string.Empty;

        return _deserializers.Single(x => x.CanDeserialize(type)).Deserialize(reader);
    }
}