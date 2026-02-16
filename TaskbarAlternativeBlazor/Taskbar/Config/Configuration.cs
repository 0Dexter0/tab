namespace TaskbarAlternativeBlazor.Taskbar.Config;

internal record Configuration(
    bool WatchStyles,
    bool WatchConfig,
    Dictionary<string, Bar> Bars);