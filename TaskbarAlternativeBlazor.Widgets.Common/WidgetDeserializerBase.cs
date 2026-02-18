using YamlDotNet.Serialization;

namespace TaskbarAlternativeBlazor.Widgets.Common;

public abstract class WidgetDeserializerBase<TWidget>
    where TWidget : IWidget
{
    public abstract bool CanDeserialize(string type);

    public IWidget Deserialize(StreamReader reader) => new Deserializer().Deserialize<TWidget>(reader);
}