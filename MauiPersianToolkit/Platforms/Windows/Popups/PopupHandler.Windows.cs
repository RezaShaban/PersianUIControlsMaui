using MauiPersianToolkit.Platform;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using MauiApplication = Microsoft.Maui.Controls.Application;
using WinUIWindow = Microsoft.UI.Xaml.Window;

namespace MauiPersianToolkit.Controls;

public partial class PopupHandler
{
    private partial MauiPopup CreatePlatformElement() => new();

    private partial void DestroyPlatformElement(MauiPopup platformView) => platformView.Cleanup();

    internal void ShowPopup()
    {
        if (VirtualView is not Popup popup || popup.Container is null)
            return;

        var mauiContext = MauiContext
            ?? throw new InvalidOperationException("The popup handler has no MauiContext.");

        var xamlRoot = ResolveXamlRoot(mauiContext)
            ?? throw new InvalidOperationException("The application window has no XamlRoot to host a popup.");

        var content = (FrameworkElement)popup.Container.ToPlatform(mauiContext);

        PlatformView.SetContent(popup, content, xamlRoot);
        PlatformView.Show();

        VirtualView.OnOpened();
    }

    internal void ClosePopup() => PlatformView.HidePopup();

    private static XamlRoot? ResolveXamlRoot(IMauiContext mauiContext)
    {
        if (mauiContext.Services.GetService<WinUIWindow>()?.Content is FrameworkElement scoped)
            return scoped.XamlRoot;

        var platformWindow = MauiApplication.Current?.Windows.FirstOrDefault()?.Handler?.PlatformView as WinUIWindow;
        return (platformWindow?.Content as FrameworkElement)?.XamlRoot;
    }
}
