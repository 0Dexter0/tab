using TaskbarAlternativeBlazor.Widgets.Common;

namespace TaskbarAlternativeBlazor.Taskbar;

internal interface IWidgetProvider
{
    IWidget? Get(string name);
}