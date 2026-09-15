using MauiPersianToolkit.Core;

namespace MauiPersianToolkit.Controls;

/// <summary>
/// A view presented over the current page inside a native popup surface: an
/// <c>Android.App.Dialog</c> on Android, a modal <c>UIViewController</c> on iOS, and a
/// WinUI <c>Popup</c> on Windows.
/// </summary>
/// <remarks>
/// Show one with <see cref="Extensions.PopupExtensions.ShowPopupAsync(Page, Popup, PopupOptions?, CancellationToken)"/>
/// and close it with <see cref="CloseAsync"/>.
/// </remarks>
public class Popup : ContentView, IPopup
{
    public Popup()
    {
        // Library XAML StaticResource cannot reliably see Application MergedDictionaries.
        PersianTheme.SeedControlResources(Resources);
    }

    /// <summary>Backing store for <see cref="CanBeDismissedByTappingOutsideOfPopup"/>.</summary>
    public static readonly BindableProperty CanBeDismissedByTappingOutsideOfPopupProperty =
        BindableProperty.Create(
            nameof(CanBeDismissedByTappingOutsideOfPopup), typeof(bool), typeof(Popup), true);

    private TaskCompletionSource<PopupResult> _closedCompletion =
        new(TaskCreationOptions.RunContinuationsAsynchronously);
    private bool _wasDismissedByTappingOutside;

    /// <summary>
    /// Raised once the popup surface is on screen.
    /// </summary>
    public event EventHandler? Opened;

    /// <summary>
    /// Raised once the popup surface has been torn down.
    /// </summary>
    public event EventHandler? Closed;

    /// <summary>
    /// When <see langword="true"/> (the default), tapping the dimmed area outside the
    /// popup closes it. <see cref="PopupOptions.CanBeDismissedByTappingOutsideOfPopup"/>
    /// overrides this for a single display.
    /// </summary>
    public bool CanBeDismissedByTappingOutsideOfPopup
    {
        get => (bool)GetValue(CanBeDismissedByTappingOutsideOfPopupProperty);
        set => SetValue(CanBeDismissedByTappingOutsideOfPopupProperty, value);
    }

    /// <summary>
    /// Options the popup is currently being displayed with.
    /// </summary>
    internal PopupOptions Options { get; private set; } = PopupOptions.Default;

    /// <summary>
    /// Full-window layout the platform host renders for the current display.
    /// </summary>
    internal PopupContainer? Container { get; set; }

    /// <summary>
    /// Handler owning the native popup surface.
    /// </summary>
    /// <remarks>
    /// Deliberately separate from <see cref="VisualElement.Handler"/>: the popup sits inside
    /// <see cref="Container"/>, so MAUI assigns it an ordinary content view handler, while
    /// the native dialog, view controller or WinUI popup is owned by this one.
    /// </remarks>
    internal PopupHandler? Host { get; set; }

    /// <summary>
    /// Completes when the popup surface has been torn down.
    /// </summary>
    internal Task<PopupResult> ClosedTask => _closedCompletion.Task;

    /// <summary>
    /// Closes the popup and waits for the platform surface to be torn down.
    /// </summary>
    public async Task CloseAsync(CancellationToken token = default)
    {
        // A popup that was never shown has no surface to tear down, and awaiting its
        // completion would never return.
        if (Host is null || _closedCompletion.Task.IsCompleted)
            return;

        Host.ClosePopup();
        await _closedCompletion.Task.WaitAsync(token).ConfigureAwait(false);
    }

    /// <summary>
    /// Closes the popup, reporting it as dismissed by the user rather than by the
    /// popup's own buttons.
    /// </summary>
    internal Task DismissAsync(CancellationToken token = default)
    {
        ((IPopup)this).OnDismissedByTappingOutsideOfPopup();
        return CloseAsync(token);
    }

    /// <summary>
    /// Resets the popup for a fresh display. A popup instance may be shown more than once.
    /// </summary>
    internal void Prepare(PopupOptions options)
    {
        Options = options;
        _wasDismissedByTappingOutside = false;

        if (_closedCompletion.Task.IsCompleted)
            _closedCompletion = new TaskCompletionSource<PopupResult>(TaskCreationOptions.RunContinuationsAsynchronously);
    }

    /// <summary>
    /// Whether a tap outside the popup should close it, honoring the per-display override.
    /// </summary>
    internal bool ResolveDismissOnTappingOutside() =>
        Options.CanBeDismissedByTappingOutsideOfPopup ?? CanBeDismissedByTappingOutsideOfPopup;

    void IPopup.OnOpened() => Opened?.Invoke(this, EventArgs.Empty);

    void IPopup.OnClosed()
    {
        Closed?.Invoke(this, EventArgs.Empty);
        _closedCompletion.TrySetResult(new PopupResult(_wasDismissedByTappingOutside));

        Host?.Disconnect();
        Host = null;

        // Detach from the container so the popup instance can be shown again later.
        Container?.Release(this);
        Container = null;
    }

    void IPopup.OnDismissedByTappingOutsideOfPopup()
    {
        _wasDismissedByTappingOutside = true;
        Options.OnTappingOutsideOfPopup?.Invoke();
    }
}
