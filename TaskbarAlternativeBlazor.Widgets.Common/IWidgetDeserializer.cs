namespace TaskbarAlternativeBlazor.Widgets.Common;

public interface IWidgetDeserializer
{
    bool CanDeserialize(string type);

    IWidget Deserialize(StreamReader reader);
}