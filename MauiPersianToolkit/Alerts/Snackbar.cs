using MauiPersianToolkit.Core;
using MauiPersianToolkit.Localization;

namespace MauiPersianToolkit.Alerts;

/// <summary>
/// A message anchored to the bottom of the screen carrying a single action button.
/// </summary>
public partial class Snackbar : ISnackbar
{
    /// <inheritdoc/>
    public string Text { get; init; } = string.Empty;

    /// <inheritdoc/>
    public string ActionButtonText { get; init; } = PersianToolkitStrings.Ok;

    /// <inheritdoc/>
    public Action? Action { get; init; }

    /// <inheritdoc/>
    public TimeSpan Duration { get; init; } = TimeSpan.FromSeconds(3);

    /// <inheritdoc/>
    public SnackbarOptions VisualOptions { get; init; } = new();

    /// <summary>
    /// Creates a snackbar. Call <see cref="Show"/> to display it.
    /// </summary>
    public static ISnackbar Make(
        string message,
        Action? action = null,
        string? actionButtonText = null,
        TimeSpan? duration = null,
        SnackbarOptions? visualOptions = null) =>
        new Snackbar
        {
            Text = message,
            Action = action,
            ActionButtonText = actionButtonText ?? PersianToolkitStrings.Ok,
            Duration = duration ?? TimeSpan.FromSeconds(3),
            VisualOptions = visualOptions ?? new SnackbarOptions()
        };

    /// <inheritdoc/>
    public Task Show(CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();
        return ShowPlatform(token);
    }

    /// <inheritdoc/>
    public Task Dismiss(CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();
        return DismissPlatform(token);
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        await Dismiss().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    private partial Task ShowPlatform(CancellationToken token);

    private partial Task DismissPlatform(CancellationToken token);
}
