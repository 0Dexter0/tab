using TaskbarAlternativeBlazor.Widgets.Common;

namespace TaskbarAlternativeBlazor.Widgets.ClockWidget;

internal sealed class ClockDeserializer : WidgetDeserializerBase<ClockWidget>, IWidgetDeserializer
{
    public override bool CanDeserialize(string type) => type == "tab.clock";
}