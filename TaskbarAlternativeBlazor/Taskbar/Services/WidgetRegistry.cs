using TaskbarAlternativeBlazor.Widgets.Common;

namespace TaskbarAlternativeBlazor.Taskbar.Services;

internal sealed class WidgetRegistry : IWidgetRegistry
{
    private readonly Dictionary<string, IWidget> _widgets = [];

    public bool TryGetWidget(string type, out IWidget? widget) => _widgets.TryGetValue(type, out widget);

    public void AddWidget(IWidget widget) => _widgets[widget.Type] = widget;

    public void RemoveWidget(IWidget widget) => _widgets.Remove(widget.Type);
}