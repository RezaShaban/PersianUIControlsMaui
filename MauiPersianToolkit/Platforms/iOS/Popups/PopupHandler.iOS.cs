using MauiPersianToolkit.Platform;
using Microsoft.Maui.Platform;

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

        var content = popup.Container.ToPlatform(mauiContext);

        PlatformView.SetContent(popup, popup.Container, content);
        PlatformView.Show(VirtualView.OnOpened);
    }

    internal void ClosePopup() => PlatformView.HidePopup();
}
