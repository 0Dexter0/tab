namespace TaskbarAlternativeBlazor.Taskbar.Config;

internal record Bar(
    bool Enabled,
    string[] Screens,
    string Position,
    string Width,
    string Height,
    Widgets Widgets);