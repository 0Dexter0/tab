namespace TaskbarAlternativeBlazor.Taskbar.Config;

internal record Widgets
{
    public string[] Left { get; init; }

    public string[] Center { get; init; }

    public string[] Right { get; init; }
}