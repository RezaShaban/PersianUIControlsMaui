namespace MauiPersianToolkit.Core;

/// <summary>
/// Shared surface of the transient, self-dismissing notifications (toast and snackbar).
/// </summary>
public interface IAlert : IAsyncDisposable
{
    /// <summary>
    /// Message shown to the user.
    /// </summary>
    string Text { get; }

    /// <summary>
    /// Displays the alert, replacing any alert of the same kind already on screen.
    /// </summary>
    Task Show(CancellationToken token = default);

    /// <summary>
    /// Hides the alert if it is currently visible.
    /// </summary>
    Task Dismiss(CancellationToken token = default);
}

/// <summary>
/// A short, non-interactive message anchored to the bottom of the screen.
/// </summary>
public interface IToast : IAlert
{
    /// <summary>
    /// How long the toast stays on screen.
    /// </summary>
    ToastDuration Duration { get; }
}

/// <summary>
/// A message anchored to the bottom of the screen carrying a single action button.
/// </summary>
public interface ISnackbar : IAlert
{
    /// <summary>
    /// Label of the action button.
    /// </summary>
    string ActionButtonText { get; }

    /// <summary>
    /// Invoked when the user taps the action button.
    /// </summary>
    Action? Action { get; }

    /// <summary>
    /// How long the snackbar stays on screen before dismissing itself.
    /// </summary>
    TimeSpan Duration { get; }

    /// <summary>
    /// Appearance of the snackbar surface.
    /// </summary>
    SnackbarOptions VisualOptions { get; }
}
