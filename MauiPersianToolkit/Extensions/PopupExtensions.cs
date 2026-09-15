using MauiPersianToolkit.Controls;
using MauiPersianToolkit.Core;

namespace MauiPersianToolkit.Extensions;

/// <summary>
/// Displays and closes <see cref="Popup"/> instances from a page.
/// </summary>
public static class PopupExtensions
{
    /// <summary>
    /// Displays <paramref name="popup"/> over <paramref name="page"/> and completes once
    /// the popup has been closed.
    /// </summary>
    /// <param name="page">Page the popup is shown over. Must already be attached to a window.</param>
    /// <param name="popup">Popup to display.</param>
    /// <param name="options">Backdrop, shape and dismissal behavior for this display.</param>
    /// <param name="token">Cancels waiting for the popup to close. Does not close the popup.</param>
    public static async Task<PopupResult> ShowPopupAsync(
        this Page page,
        Popup popup,
        PopupOptions? options = null,
        CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(page);
        ArgumentNullException.ThrowIfNull(popup);

        var mauiContext = page.Handler?.MauiContext
            ?? throw new InvalidOperationException(
                "The page must be attached to a window before a popup can be shown over it.");

        var resolvedOptions = options ?? PopupOptions.Default;

        popup.Prepare(resolvedOptions);
        popup.Container = new PopupContainer(popup, resolvedOptions) { Parent = page };

        PopupPresenter.Show(popup, mauiContext);

        return await popup.ClosedTask.WaitAsync(token).ConfigureAwait(false);
    }

    /// <summary>
    /// Displays <paramref name="popup"/> without waiting for it to close.
    /// </summary>
    public static void ShowPopup(this Page page, Popup popup, PopupOptions? options = null) =>
        _ = page.ShowPopupAsync(popup, options);

    /// <summary>
    /// Closes <paramref name="popup"/> and waits for the platform surface to be torn down.
    /// </summary>
    public static Task ClosePopupAsync(this Page page, Popup popup, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(popup);
        return popup.CloseAsync(token);
    }
}
