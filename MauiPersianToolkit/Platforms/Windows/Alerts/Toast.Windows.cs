using MauiPersianToolkit.Core;
using MauiPersianToolkit.Platform;

namespace MauiPersianToolkit.Alerts;

public partial class Toast
{
    private partial Task ShowPlatform(CancellationToken token)
    {
        AlertBanner.Show(new AlertBanner.Request(
            Text,
            Duration.ToTimeSpan(),
            BackgroundColor: ThemeColors.AlertBackground,
            TextColor: ThemeColors.AlertForeground,
            FontSize: TextSize,
            FontFamily: null,
            CornerRadius: new CornerRadius(20)));

        return Task.CompletedTask;
    }

    private partial Task DismissPlatform(CancellationToken token)
    {
        AlertBanner.DismissCurrent();
        return Task.CompletedTask;
    }
}
