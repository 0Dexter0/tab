using TaskbarAlternativeBlazor.Widgets.Common;

namespace TaskbarAlternativeBlazor.Taskbar;

internal sealed class WidgetProvider
{
    private readonly IWidget[] _widgets;

    public WidgetProvider(IEnumerable<IWidget> widgets)
    {
        _widgets = widgets.ToArray();
    }

    public IWidget GetWidget(string type) => _widgets.Single(x => x.Type == type);
}