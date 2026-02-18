using TaskbarAlternativeBlazor.Widgets.Common;

namespace TaskbarAlternativeBlazor.Taskbar.Services;

internal interface IWidgetRegistry
{
    bool TryGetWidget(string type, out IWidget? widget);

    void AddWidget(IWidget widget);

    void RemoveWidget(IWidget widget);
}