using TaskbarAlternativeBlazor.Widgets.Common;
using YamlDotNet.Serialization;

namespace TaskbarAlternativeBlazor.Widgets.ClockWidget;

public sealed class ClockWidget : IWidget<ClockWidgetSettings>
{
    public required string Name { get; init; }

    public string Type => "tab.clock";

    [YamlIgnore]
    public Type RuntimeType => typeof(Clock);

    [YamlIgnore]
    public Dictionary<string, object> ComponentSettings => new()
    {
        ["Settings"] = Settings
    };

    public ClockWidgetSettings Settings { get; init; } = ClockWidgetSettings.Default;
}