#if !ANDROID && !IOS && !MACCATALYST && !WINDOWS

namespace MauiPersianToolkit.Controls;

/// <summary>
/// Placeholder used by the platform-neutral target framework, which exists for reference
/// and unit testing only. Every member throws.
/// </summary>
public partial class PopupHandler
{
    private const string NotSupported =
        "Popups require a platform target framework (Android, iOS, MacCatalyst or Windows).";

    private partial object CreatePlatformElement() => throw new PlatformNotSupportedException(NotSupported);

    private partial void DestroyPlatformElement(object platformView)
    {
    }

    internal void ShowPopup() => throw new PlatformNotSupportedException(NotSupported);

    internal void ClosePopup() => throw new PlatformNotSupportedException(NotSupported);
}

#endif
