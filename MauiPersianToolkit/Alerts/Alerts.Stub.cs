#if !ANDROID && !IOS && !MACCATALYST && !WINDOWS

namespace MauiPersianToolkit.Alerts;

/// <summary>
/// Placeholder used by the platform-neutral target framework, which exists for reference
/// and unit testing only.
/// </summary>
public partial class Toast
{
    private partial Task ShowPlatform(CancellationToken token) =>
        throw new PlatformNotSupportedException(AlertPlatformSupport.Message);

    private partial Task DismissPlatform(CancellationToken token) => Task.CompletedTask;
}

/// <inheritdoc cref="Toast"/>
public partial class Snackbar
{
    private partial Task ShowPlatform(CancellationToken token) =>
        throw new PlatformNotSupportedException(AlertPlatformSupport.Message);

    private partial Task DismissPlatform(CancellationToken token) => Task.CompletedTask;
}

internal static class AlertPlatformSupport
{
    public const string Message =
        "Toasts and snackbars require a platform target framework (Android, iOS, MacCatalyst or Windows).";
}

#endif
