using YamlDotNet.Serialization;

namespace TaskbarAlternativeBlazor.Widgets.Common;

public interface IWidget
{
    // string Name { get; }

    string Type { get; }

    [YamlIgnore]
    Type RuntimeType { get; }

    [YamlIgnore]
    Dictionary<string, object> ComponentSettings { get; }
}

public interface IWidget<TSettings> : IWidget
    where TSettings : IWidgetSettings
{
    TSettings Settings { get; }
}