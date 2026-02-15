namespace WinUIApp1.Views;

public partial class HybridPage : Microsoft.UI.Xaml.Controls.Page
{
    private readonly Lazy<MauiContext> _mauiContext = new(InitializeMauiContext);

    protected MauiContext MauiContext => _mauiContext.Value;

    private static MauiContext InitializeMauiContext()
    {
        var mauiApp = MauiProgram.CreateMauiApp();

        return new(mauiApp.Services);
    }
}