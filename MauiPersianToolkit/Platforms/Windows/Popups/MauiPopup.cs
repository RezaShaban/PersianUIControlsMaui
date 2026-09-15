using MauiPersianToolkit.Core;
using Microsoft.UI.Xaml;
using WinUIPopup = Microsoft.UI.Xaml.Controls.Primitives.Popup;

namespace MauiPersianToolkit.Platform;

/// <summary>
/// Hosts a popup inside a WinUI popup stretched over the whole window.
/// </summary>
/// <remarks>
/// WinUI's <c>Popup</c> is sealed, so this wraps one rather than deriving from it.
/// </remarks>
public class MauiPopup
{
    private readonly WinUIPopup _popup;

    private IPopup? _virtualView;
    private FrameworkElement? _content;
    private XamlRoot? _xamlRoot;

    public MauiPopup()
    {
        _popup = new WinUIPopup
        {
            // The toolkit renders its own dimmed backdrop and handles taps on it, so WinUI's
            // light dismiss would double up and swallow the dismissal callbacks.
            IsLightDismissEnabled = false,
            ShouldConstrainToRootBounds = true
        };

        _popup.Closed += OnPopupClosed;
    }

    /// <summary>
    /// Attaches the rendered popup content and sizes it to the window.
    /// </summary>
    public void SetContent(IPopup virtualView, FrameworkElement content, XamlRoot xamlRoot)
    {
        _virtualView = virtualView;
        _content = content;
        _xamlRoot = xamlRoot;

        _popup.XamlRoot = xamlRoot;
        _popup.Child = content;

        ResizeToWindow();
        xamlRoot.Changed += OnXamlRootChanged;
    }

    /// <summary>
    /// Shows the popup.
    /// </summary>
    public void Show()
    {
        if (!_popup.IsOpen)
            _popup.IsOpen = true;
    }

    /// <summary>
    /// Hides the popup. The WinUI closed event reports the teardown back to the
    /// cross-platform popup.
    /// </summary>
    public void HidePopup()
    {
        if (_popup.IsOpen)
            _popup.IsOpen = false;
        else
            OnPopupClosed(this, null);
    }

    /// <summary>
    /// Detaches everything this host owns so the WinUI popup can be collected.
    /// </summary>
    public void Cleanup()
    {
        _popup.Closed -= OnPopupClosed;

        if (_xamlRoot is not null)
            _xamlRoot.Changed -= OnXamlRootChanged;

        _popup.Child = null;
        _content = null;
        _virtualView = null;
        _xamlRoot = null;
    }

    private void OnXamlRootChanged(XamlRoot sender, XamlRootChangedEventArgs args) => ResizeToWindow();

    private void ResizeToWindow()
    {
        if (_content is null || _xamlRoot is null)
            return;

        _content.Width = _xamlRoot.Size.Width;
        _content.Height = _xamlRoot.Size.Height;
    }

    private void OnPopupClosed(object? sender, object? e)
    {
        var virtualView = _virtualView;
        _virtualView = null;
        virtualView?.OnClosed();
    }
}
