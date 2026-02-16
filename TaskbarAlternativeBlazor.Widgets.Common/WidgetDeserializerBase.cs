using YamlDotNet.Serialization;

namespace TaskbarAlternativeBlazor.Widgets.Common;

public abstract class WidgetDeserializerBase<TWidget>
    where TWidget : IWidget
{
    public abstract Task<bool> CanDeserialize(Stream stream);

    public async Task<TWidget> DeserializeAsync(Stream stream)
    {
        using StreamReader reader = new(stream);

        return new Deserializer().Deserialize<TWidget>(reader);
    }
}