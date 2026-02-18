namespace TaskbarAlternativeBlazor.Taskbar.Config;

internal record Configuration
{
    public bool WatchStyles { get; init; }

    public bool WatchConfig { get; init; }

    public IReadOnlyCollection<Bar> Bars { get; init; } = [];
}