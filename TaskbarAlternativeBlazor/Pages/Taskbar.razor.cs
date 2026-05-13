using Microsoft.AspNetCore.Components;
using TaskbarAlternativeBlazor.Taskbar;
using TaskbarAlternativeBlazor.Taskbar.Services;
using TaskbarAlternativeBlazor.Widgets.ClockWidget;
using TaskbarAlternativeBlazor.Widgets.Common;

namespace TaskbarAlternativeBlazor.Pages;

public partial class Taskbar : ComponentBase, IDisposable
{
    [Inject]
    private IWidgetProvider WidgetProvider { get; init; } = null!;

    [Inject]
    private ConfigProvider ConfigProvider { get; init; } = null!;

    [Inject]
    private WidgetSettingsWatchService WidgetSettingsWatchService { get; init; } = null!;

    private IWidget[] Widgets { get; set; } = null!;

    private HashSet<string> _activeWidgetFiles = new(StringComparer.OrdinalIgnoreCase);

    public void Dispose()
    {
        WidgetSettingsWatchService.WidgetConfigChanged -= OnWidgetConfigChanged;
    }

    protected override async Task OnInitializedAsync()
    {
        // WidgetSettingsWatchService.WidgetConfigChanged += OnWidgetConfigChanged;
        // WidgetSettingsWatchService.Start();

        var config = await ConfigProvider.GetConfigurationAsync();

        var bar = config.Bars.First();

        var widgetFiles = bar.Widgets.Left
            .Concat(bar.Widgets.Center)
            .Concat(bar.Widgets.Right)
            .ToArray();

        _activeWidgetFiles = widgetFiles.ToHashSet(StringComparer.OrdinalIgnoreCase);

        // Widgets = _activeWidgetFiles.Select(WidgetProvider.Get).Where(x => x is not null).ToArray()!;
        Widgets = [new ClockWidget { Name = "Clock", Settings = ClockWidgetSettings.Default }];
    }

    private void OnWidgetConfigChanged(string changedFileName)
    {
        if (!_activeWidgetFiles.Contains(changedFileName))
        {
            return;
        }

        _ = InvokeAsync(() =>
        {
            for (var i = 0; i < Widgets.Length; i++)
            {
                if (string.Equals(Widgets[i].Name,
                    Path.GetFileNameWithoutExtension(changedFileName),
                    StringComparison.OrdinalIgnoreCase))
                {
                    Widgets[i] = WidgetProvider.Get(changedFileName)!;
                    StateHasChanged();
                    return;
                }
            }
        });
    }
}