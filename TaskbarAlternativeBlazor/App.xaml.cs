using System.Runtime.InteropServices;
using Microsoft.UI.Xaml;
using Frame = Microsoft.UI.Xaml.Controls.Frame;
using Window = Microsoft.UI.Xaml.Window;

namespace TaskbarAlternativeBlazor;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App
{
    private const int TaskbarHeight = 60;

    private Window? _window;
    private Microsoft.UI.Windowing.AppWindow? _appWindow;
    private nint _windowHandle;

    // Win32 style indices
    private const int GwlStyle = -16;
    private const int GwlExstyle = -20;

    // Win32 styles
    private const long WsCaption = 0x00C00000L;
    private const long WsThickframe = 0x00040000L;
    private const long WsBorder = 0x00800000L;
    private const long WsDlgframe = 0x00400000L;

    // Win32 extended styles
    private const long WsExClientedge = 0x00000200L;
    private const long WsExStaticedge = 0x00020000L;
    private const long WsExDlgmodalframe = 0x00000001L;

    // SetWindowPos flags
    private const uint SwpNosize = 0x0001;
    private const uint SwpNomove = 0x0002;
    private const uint SwpNozorder = 0x0004;
    private const uint SwpNoactivate = 0x0010;
    private const uint SwpFramechanged = 0x0020;

    // DWM attributes
    private const int DwmwaWindowCornerPreference = 33;

    // DWM corner preferences
    private const int DwmwcpDefault = 0;
    private const int DwmwcpDonotround = 1;
    private const int DwmwcpRound = 2;
    private const int DwmwcpRoundsmall = 3;

    /// <summary>
    /// Initializes the singleton application object. This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Invoked when the application is launched normally by the end user. Other entry points
    /// will be used such as when the application is launched to open a specific file.
    /// </summary>
    /// <param name="e">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs e)
    {
        _window = new();

        _windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(_window);
        var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(_windowHandle);
        _appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

        ConfigureTaskbarMode();

        if (_window.Content is not Frame rootFrame)
        {
            rootFrame = new();
            _window.Content = rootFrame;
        }

        _ = rootFrame.Navigate(typeof(MainPage), e.Arguments);
        _window.Activate();

        ApplyBorderlessSquareWindow(_windowHandle);
    }

    private void ConfigureTaskbarMode()
    {
        if (_appWindow == null)
        {
            return;
        }

        _appWindow.SetPresenter(Microsoft.UI.Windowing.AppWindowPresenterKind.Overlapped);
        if (_appWindow.Presenter is Microsoft.UI.Windowing.OverlappedPresenter p)
        {
            p.IsAlwaysOnTop = true;
            p.IsResizable = false;
            p.IsMinimizable = false;
            p.IsMaximizable = false;
            p.SetBorderAndTitleBar(false, false);
        }

        var displayArea = Microsoft.UI.Windowing.DisplayArea.Primary;
        int width = displayArea.OuterBounds.Width;
        int height = TaskbarHeight;
        int x = displayArea.OuterBounds.X;
        int y = displayArea.OuterBounds.Y + displayArea.OuterBounds.Height - height;

        _appWindow.MoveAndResize(new(x, y, width, height));
    }

    private static void ApplyBorderlessSquareWindow(nint hwnd)
    {
        if (hwnd == 0) return;

        RemoveWin32Borders(hwnd);
        SetCornerPreference(hwnd, DwmwcpDonotround);
    }

    private static void RemoveWin32Borders(nint hwnd)
    {
        nint style = GetWindowLongPtr(hwnd, GwlStyle);
        nint exStyle = GetWindowLongPtr(hwnd, GwlExstyle);

        long s = style.ToInt64();
        long xs = exStyle.ToInt64();

        s &= ~WsCaption;
        s &= ~WsThickframe;
        s &= ~WsBorder;
        s &= ~WsDlgframe;

        xs &= ~WsExClientedge;
        xs &= ~WsExStaticedge;
        xs &= ~WsExDlgmodalframe;

        SetWindowLongPtr(hwnd, GwlStyle, new nint(s));
        SetWindowLongPtr(hwnd, GwlExstyle, new nint(xs));

        SetWindowPos(
            hwnd,
            0,
            0, 0, 0, 0,
            SwpNomove | SwpNosize | SwpNozorder | SwpNoactivate | SwpFramechanged);
    }

    private static void SetCornerPreference(nint hwnd, int preference)
    {
        _ = DwmSetWindowAttribute(hwnd, DwmwaWindowCornerPreference, ref preference, sizeof(int));
    }

    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtrW")]
    private static extern nint GetWindowLongPtr(nint hWnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtrW")]
    private static extern nint SetWindowLongPtr(nint hWnd, int nIndex, nint dwNewLong);

    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    private static extern bool SetWindowPos(
        nint hWnd,
        nint hWndInsertAfter,
        int x,
        int y,
        int cx,
        int cy,
        uint uFlags);

    [DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(nint hwnd, int dwAttribute, ref int pvAttribute, int cbAttribute);
}