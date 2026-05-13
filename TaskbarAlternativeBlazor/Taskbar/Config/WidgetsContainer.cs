using TaskbarAlternativeBlazor.Widgets.Common;

namespace TaskbarAlternativeBlazor.Taskbar.Config;

public sealed record WidgetsContainer
{
    public Dictionary<string, IWidget> Widgets { get; set; }
}