using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using MauiPersianToolkit.Core;
using AColor = Android.Graphics.Color;
using AView = Android.Views.View;

namespace MauiPersianToolkit.Platform;

/// <summary>
/// Hosts a popup inside a translucent, full-screen <see cref="Dialog"/>.
/// </summary>
public class MauiPopup : Dialog
{
    private IPopup? _virtualView;
    private AView? _content;

    public MauiPopup(Context context)
        : base(context, Android.Resource.Style.ThemeTranslucentNoTitleBar)
    {
        CancelEvent += OnCancelled;
    }

    /// <summary>
    /// Attaches the rendered popup content and stretches the dialog window over the screen.
    /// </summary>
    public void SetContent(IPopup virtualView, AView content)
    {
        _virtualView = virtualView;
        _content = content;

        RequestWindowFeature((int)WindowFeatures.NoTitle);
        SetContentView(content);

        // The toolkit draws its own dimmed backdrop and handles taps on it, so the dialog
        // window itself must be fully transparent and edge to edge.
        if (Window is { } window)
        {
            window.SetBackgroundDrawable(new ColorDrawable(AColor.Transparent));
            window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            window.SetSoftInputMode(SoftInput.AdjustResize);
            window.ClearFlags(WindowManagerFlags.DimBehind);
        }

        // There is no area outside the dialog to tap, so dismissal runs through the backdrop
        // the toolkit draws. Cancellation is left to the hardware back button.
        SetCanceledOnTouchOutside(false);
        SetCancelable(virtualView.CanBeDismissedByTappingOutsideOfPopup);
    }

    /// <summary>
    /// Shows the dialog.
    /// </summary>
    public void ShowPopup()
    {
        if (!IsShowing)
            Show();
    }

    /// <summary>
    /// Dismisses the dialog. <see cref="OnStop"/> reports the teardown back to the
    /// cross-platform popup.
    /// </summary>
    public void HidePopup()
    {
        if (IsShowing)
            Dismiss();
        else
            NotifyClosed();
    }

    /// <summary>
    /// Detaches everything this host owns.
    /// </summary>
    public void Cleanup()
    {
        CancelEvent -= OnCancelled;
        (_content?.Parent as ViewGroup)?.RemoveView(_content);
        _content = null;
        _virtualView = null;
    }

    protected override void OnStop()
    {
        base.OnStop();
        NotifyClosed();
    }

    /// <summary>
    /// The hardware back button cancels the dialog, which is the Android equivalent of
    /// tapping outside the popup.
    /// </summary>
    private void OnCancelled(object? sender, EventArgs e) =>
        _virtualView?.OnDismissedByTappingOutsideOfPopup();

    private void NotifyClosed()
    {
        var virtualView = _virtualView;
        _virtualView = null;
        virtualView?.OnClosed();
    }
}
