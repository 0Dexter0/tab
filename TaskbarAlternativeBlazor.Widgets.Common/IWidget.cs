using Microsoft.AspNetCore.Components;

namespace TaskbarAlternativeBlazor.Widgets.Common;

public interface IWidget
{
    string Name { get; }

    string Type { get; }

    Type RuntimeType { get; }

    Dictionary<string, object> ComponentSettings { get; }
}

public interface IWidget<TSettings> : IWidget
    where TSettings : IWidgetSettings
{
    TSettings Settings { get; }
}