namespace MauiPersianToolkit.Core;

/// <summary>
/// Cross-platform contract a platform popup host renders. Implemented by
/// <see cref="Controls.Popup"/> and consumed by the per-platform popup handlers.
/// </summary>
public interface IPopup : IContentView
{
    /// <summary>
    /// When <see langword="true"/>, tapping the dimmed area outside the popup closes it.
    /// </summary>
    bool CanBeDismissedByTappingOutsideOfPopup { get; }

    /// <summary>
    /// Raised by the platform host once the popup surface is on screen.
    /// </summary>
    void OnOpened();

    /// <summary>
    /// Raised by the platform host when the popup surface has been torn down, whether
    /// closed programmatically or dismissed by the user.
    /// </summary>
    void OnClosed();

    /// <summary>
    /// Raised by the platform host when the user dismisses the popup without using one
    /// of its own buttons, either by tapping outside of it or via the hardware back button.
    /// </summary>
    void OnDismissedByTappingOutsideOfPopup();
}
