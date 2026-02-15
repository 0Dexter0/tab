using TaskbarAlternativeBlazor.Widgets.Common;

namespace TaskbarAlternativeBlazor.Widgets.ClockWidget;

public partial class Clock : WidgetComponentBase<ClockWidgetSettings>, IDisposable
{
    private Timer? _timer;

    private DateTime Date { get; set; }

    protected override void OnInitialized()
    {
        _timer = new(Tick);
        _timer.Change(TimeSpan.Zero,  TimeSpan.FromMilliseconds(1000));
    }

    private void Tick(object? state)
    {
        Date = DateTime.Now;
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}