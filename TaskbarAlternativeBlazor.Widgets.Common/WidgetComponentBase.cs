using Microsoft.AspNetCore.Components;

namespace TaskbarAlternativeBlazor.Widgets.Common;

public class WidgetComponentBase<TWidgetSettings> : ComponentBase
    where TWidgetSettings : IWidgetSettings
{
    [Parameter]
    public required TWidgetSettings Settings { get; init; }
}