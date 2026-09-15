namespace MauiPersianToolkit.Controls;

/// <summary>
/// Wires a <see cref="Popup"/> up to its <see cref="PopupHandler"/> and asks the platform
/// host to present it.
/// </summary>
internal static class PopupPresenter
{
    public static void Show(Popup popup, IMauiContext mauiContext)
    {
        var handler = ResolveHandler(popup, mauiContext);
        handler.ShowPopup();
    }

    /// <summary>
    /// Connects the popup to a handler. A popup is not part of the page's visual tree, so
    /// the handler has to be created and wired up explicitly rather than by the layout pass.
    /// </summary>
    private static PopupHandler ResolveHandler(Popup popup, IMauiContext mauiContext)
    {
        // A fresh host per display: two popups can be on screen at once, and the handler
        // owns the native surface of exactly one of them.
        var handler = new PopupHandler();

        handler.SetMauiContext(mauiContext);
        handler.SetVirtualView(popup);
        popup.Host = handler;

        return handler;
    }
}
