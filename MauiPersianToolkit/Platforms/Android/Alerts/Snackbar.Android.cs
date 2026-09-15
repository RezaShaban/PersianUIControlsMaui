using Android.Graphics.Drawables;
using Microsoft.Maui.Platform;
using MaterialSnackbar = Google.Android.Material.Snackbar.Snackbar;

namespace MauiPersianToolkit.Alerts;

public partial class Snackbar
{
    private static MaterialSnackbar? _current;

    private partial Task ShowPlatform(CancellationToken token)
    {
        DismissCurrent();

        var activity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
        var anchor = activity?.Window?.DecorView?.FindViewById(Android.Resource.Id.Content);

        if (activity is null || anchor is null)
            return Task.CompletedTask;

        var snackbar = MaterialSnackbar.Make(anchor, Text, (int)Duration.TotalMilliseconds);

        snackbar.SetTextColor(VisualOptions.TextColor.ToPlatform().ToArgb());
        snackbar.SetActionTextColor(VisualOptions.ActionButtonTextColor.ToPlatform().ToArgb());

        // SetBackgroundTint cannot express a corner radius, so the surface is styled directly.
        var density = activity.Resources?.DisplayMetrics?.Density ?? 1f;
        var background = new GradientDrawable();
        background.SetColor(VisualOptions.BackgroundColor.ToPlatform().ToArgb());
        background.SetCornerRadius((float)VisualOptions.CornerRadius.TopLeft * density);
        snackbar.View.Background = background;

        var action = Action;
        snackbar.SetAction(ActionButtonText, _ => action?.Invoke());

        snackbar.Show();
        _current = snackbar;

        return Task.CompletedTask;
    }

    private partial Task DismissPlatform(CancellationToken token)
    {
        DismissCurrent();
        return Task.CompletedTask;
    }

    private static void DismissCurrent()
    {
        _current?.Dismiss();
        _current = null;
    }
}
