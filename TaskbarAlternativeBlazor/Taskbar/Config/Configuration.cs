namespace TaskbarAlternativeBlazor.Taskbar.Config;

internal record Configuration(
    bool WatchStyles,
    bool WatchConfig,
    Bar[] Bars);