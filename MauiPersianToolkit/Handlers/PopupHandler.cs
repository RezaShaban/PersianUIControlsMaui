using MauiPersianToolkit.Core;

#if ANDROID || IOS || MACCATALYST || WINDOWS
using PlatformPopup = MauiPersianToolkit.Platform.MauiPopup;
#else
using PlatformPopup = System.Object;
#endif

namespace MauiPersianToolkit.Controls;

/// <summary>
/// Owns the native surface a <see cref="Popup"/> is presented on: an
/// <c>Android.App.Dialog</c>, a modal <c>UIViewController</c>, or a WinUI <c>Popup</c>.
/// </summary>
/// <remarks>
/// Deliberately not a MAUI <c>ElementHandler</c>. <see cref="Popup"/> is a
/// <see cref="ContentView"/>, and MAUI only accepts an <c>IViewHandler</c> on a visual
/// element's <c>Handler</c>, so a handler of this shape can never be attached to one.
/// The popup's content still goes through the normal handler pipeline via
/// <c>ToPlatform</c>; this type only owns the surface that content sits on.
/// </remarks>
public partial class PopupHandler
{
    /// <summary>
    /// Popup currently hosted, or <see langword="null"/> once disconnected.
    /// </summary>
    public IPopup? VirtualView { get; private set; }

    /// <summary>
    /// Context used to render the popup content and reach the platform window.
    /// </summary>
    public IMauiContext? MauiContext { get; private set; }

    /// <summary>
    /// Native surface hosting the popup.
    /// </summary>
    public PlatformPopup? PlatformView { get; private set; }

    /// <summary>
    /// Supplies the context the popup content is rendered with.
    /// </summary>
    public void SetMauiContext(IMauiContext mauiContext) => MauiContext = mauiContext;

    /// <summary>
    /// Attaches <paramref name="virtualView"/> and creates the native surface for it.
    /// </summary>
    public void SetVirtualView(IPopup virtualView)
    {
        VirtualView = virtualView;
        PlatformView ??= CreatePlatformElement();
    }

    /// <summary>
    /// Releases the native surface. Called once the popup has been torn down.
    /// </summary>
    public void Disconnect()
    {
        if (PlatformView is { } platformView)
            DestroyPlatformElement(platformView);

        PlatformView = null;
        VirtualView = null;
        MauiContext = null;
    }

    private partial PlatformPopup CreatePlatformElement();

    private partial void DestroyPlatformElement(PlatformPopup platformView);
}
