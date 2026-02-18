using TaskbarAlternativeBlazor.Widgets.Common;
using YamlDotNet.Serialization;

namespace TaskbarAlternativeBlazor.Widgets.ClockWidget;

public sealed class ClockWidget : WidgetBase<ClockWidgetSettings>, IWidget<ClockWidgetSettings>
{
    public string Type => "tab.clock";

    [YamlIgnore]
    public override Type Component => typeof(Clock);
}