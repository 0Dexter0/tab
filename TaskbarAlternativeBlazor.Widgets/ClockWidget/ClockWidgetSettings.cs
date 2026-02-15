using TaskbarAlternativeBlazor.Widgets.Common;

namespace TaskbarAlternativeBlazor.Widgets.ClockWidget;

public sealed class ClockWidgetSettings : IWidgetSettings
{
    public string Label { get; init; } = "hh:mm:ss dd.mm.yyyy";

    public static ClockWidgetSettings Default { get; } = new();
}