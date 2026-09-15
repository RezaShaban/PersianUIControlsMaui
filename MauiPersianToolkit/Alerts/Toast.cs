using MauiPersianToolkit.Core;

namespace MauiPersianToolkit.Alerts;

/// <summary>
/// A short, non-interactive message anchored to the bottom of the screen.
/// </summary>
public partial class Toast : IToast
{
    /// <inheritdoc/>
    public string Text { get; init; } = string.Empty;

    /// <inheritdoc/>
    public ToastDuration Duration { get; init; } = ToastDuration.Short;

    /// <summary>
    /// Size of the message text.
    /// </summary>
    public double TextSize { get; init; } = 14d;

    /// <summary>
    /// Creates a toast. Call <see cref="Show"/> to display it.
    /// </summary>
    public static IToast Make(string message, ToastDuration duration = ToastDuration.Short, double textSize = 14d) =>
        new Toast { Text = message, Duration = duration, TextSize = textSize };

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
