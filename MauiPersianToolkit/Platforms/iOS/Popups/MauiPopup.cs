using MauiPersianToolkit.Core;
using UIKit;

namespace MauiPersianToolkit.Platform;

/// <summary>
/// Hosts a popup inside a modal <see cref="UIViewController"/> presented over the current
/// view controller without removing it from the screen.
/// </summary>
public class MauiPopup : UIViewController
{
    private IPopup? _virtualView;
    private IView? _container;
    private UIView? _content;

    public MauiPopup()
    {
        ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;
        ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve;
    }

    /// <summary>
    /// Attaches the rendered popup content.
    /// </summary>
    public void SetContent(IPopup virtualView, IView container, UIView content)
    {
        _virtualView = virtualView;
        _container = container;
        _content = content;

        View!.BackgroundColor = UIColor.Clear;
        View.AddSubview(content);
    }

    /// <summary>
    /// Presents the popup over the topmost view controller.
    /// </summary>
    public void Show(Action? onPresented = null)
    {
        var presenter = ResolvePresentingController();
        if (presenter is null)
            throw new InvalidOperationException("No view controller is available to present a popup from.");

        presenter.PresentViewController(this, animated: true, completionHandler: () => onPresented?.Invoke());
    }

    /// <summary>
    /// Dismisses the popup, reporting the teardown back to the cross-platform popup.
    /// </summary>
    public void HidePopup()
    {
        var virtualView = _virtualView;
        _virtualView = null;

        if (PresentingViewController is null)
        {
            virtualView?.OnClosed();
            return;
        }

        DismissViewController(animated: true, completionHandler: () => virtualView?.OnClosed());
    }

    /// <summary>
    /// Detaches everything this host owns.
    /// </summary>
    public void Cleanup()
    {
        _content?.RemoveFromSuperview();
        _content = null;
        _container = null;
        _virtualView = null;
    }

    /// <summary>
    /// A popup lives outside the page's layout pass, so its cross-platform measure and
    /// arrange have to be driven from the controller's own layout cycle.
    /// </summary>
    public override void ViewDidLayoutSubviews()
    {
        base.ViewDidLayoutSubviews();

        if (_content is null || _container is null)
            return;

        var bounds = View!.Bounds;
        _content.Frame = bounds;

        _container.Measure(bounds.Width, bounds.Height);
        _container.Arrange(new Rect(0, 0, bounds.Width, bounds.Height));
    }

    private static UIViewController? ResolvePresentingController() => PlatformWindow.GetTopViewController();
}
