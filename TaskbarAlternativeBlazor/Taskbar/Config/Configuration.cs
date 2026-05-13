namespace TaskbarAlternativeBlazor.Taskbar.Config;

public record Configuration
{
    public bool WatchStyles { get; init; }

    public bool WatchConfig { get; init; }

    public Bar[] Bars { get; init; } = [];
}