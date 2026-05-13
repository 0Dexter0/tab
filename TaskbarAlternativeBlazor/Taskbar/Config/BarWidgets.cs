namespace TaskbarAlternativeBlazor.Taskbar.Config;

public record BarWidgets
{
    public string[] Left { get; init; }

    public string[] Center { get; init; }

    public string[] Right { get; init; }
}