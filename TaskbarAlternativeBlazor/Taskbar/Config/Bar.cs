namespace TaskbarAlternativeBlazor.Taskbar.Config;

public record Bar
{
    public required string Name { get; init; }

    public bool Enabled { get; init; }

    public required string[] Screens { get; init; }

    public required string Position { get; init; }

    public bool Centered { get; init; }

    public int BorderRadius { get; init; }

    public required string Width { get; init; }

    public required string Height { get; init; }

    public required BarWidgets Widgets { get; init; }
}