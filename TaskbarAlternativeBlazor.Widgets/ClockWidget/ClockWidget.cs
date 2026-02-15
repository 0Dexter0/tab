using TaskbarAlternativeBlazor.Widgets.Common;

namespace TaskbarAlternativeBlazor.Widgets.ClockWidget;

public sealed class ClockWidget : IWidget<ClockWidgetSettings>
{
    public required string Name { get; init; }

    public string Type => "tab.clock";

    public Type RuntimeType => typeof(Clock);

    public Dictionary<string, object> ComponentSettings => new()
    {
        ["Settings"] = Settings
    };

    public ClockWidgetSettings Settings { get; init; } = ClockWidgetSettings.Default;
}