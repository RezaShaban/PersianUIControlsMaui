using Android.Widget;
using MauiPersianToolkit.Core;
using AToast = Android.Widget.Toast;

namespace MauiPersianToolkit.Alerts;

public partial class Toast
{
    private static AToast? _current;

    private partial Task ShowPlatform(CancellationToken token)
    {
        DismissCurrent();

        Android.Content.Context context = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity
            ?? Android.App.Application.Context;

        var length = Duration == ToastDuration.Long ? ToastLength.Long : ToastLength.Short;

        _current = AToast.MakeText(context, Text, length);
        _current?.Show();

        return Task.CompletedTask;
    }

    private partial Task DismissPlatform(CancellationToken token)
    {
        DismissCurrent();
        return Task.CompletedTask;
    }

    private static void DismissCurrent()
    {
        _current?.Cancel();
        _current = null;
    }
}
