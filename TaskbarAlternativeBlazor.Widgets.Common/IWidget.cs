namespace TaskbarAlternativeBlazor.Widgets.Common;

public interface IWidget
{
    string Name { get; }

    Type Component { get; }

    Dictionary<string, object> ComponentSettings { get; }

    string Type { get; }
}

public interface IWidget<TSettings> : IWidget
    where TSettings : IWidgetSettings
{
    TSettings Settings { get; }
}