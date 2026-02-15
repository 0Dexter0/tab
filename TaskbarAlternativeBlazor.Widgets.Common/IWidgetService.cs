namespace TaskbarAlternativeBlazor.Widgets.Common;

public interface IWidgetService : IAsyncDisposable // ???
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}