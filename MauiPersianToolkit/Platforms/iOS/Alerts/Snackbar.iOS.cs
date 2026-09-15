using MauiPersianToolkit.Platform;

namespace MauiPersianToolkit.Alerts;

public partial class Snackbar
{
    private partial Task ShowPlatform(CancellationToken token)
    {
        AlertBanner.Show(new AlertBanner.Request(
            Text,
            Duration,
            VisualOptions.BackgroundColor,
            VisualOptions.TextColor,
            VisualOptions.FontSize,
            VisualOptions.FontFamily,
            VisualOptions.CornerRadius.TopLeft,
            ActionButtonText,
            Action,
            VisualOptions.ActionButtonTextColor));

        return Task.CompletedTask;
    }

    private partial Task DismissPlatform(CancellationToken token)
    {
        AlertBanner.DismissCurrent();
        return Task.CompletedTask;
    }
}
