using Foundation;
using Microsoft.Maui.Platform;
using UIKit;

namespace MauiPersianToolkit.Platform;

/// <summary>
/// The bottom-anchored banner used for both toasts and snackbars. UIKit has no native
/// equivalent, so the toolkit renders its own and animates it in and out of the key window.
/// </summary>
internal sealed class AlertBanner
{
    private const double HorizontalMargin = 16;
    private const double BottomMargin = 48;
    private const double AnimationSeconds = 0.25;

    private static AlertBanner? _current;

    private readonly UIView _root;
    private NSTimer? _timer;

    private AlertBanner(UIView root) => _root = root;

    /// <summary>
    /// Describes the banner to render.
    /// </summary>
    public sealed record Request(
        string Text,
        TimeSpan Duration,
        Color BackgroundColor,
        Color TextColor,
        double FontSize,
        string? FontFamily,
        double CornerRadius,
        string? ActionText = null,
        Action? Action = null,
        Color? ActionTextColor = null);

    /// <summary>
    /// Shows a banner, replacing whichever one is currently on screen.
    /// </summary>
    public static void Show(Request request)
    {
        DismissCurrent();

        var window = PlatformWindow.GetKeyWindow();
        if (window is null)
            return;

        var container = BuildContainer(request);
        window.AddSubview(container);
        ApplyConstraints(window, container);

        var banner = new AlertBanner(container);
        _current = banner;

        container.Alpha = 0;
        UIView.Animate(AnimationSeconds, () => container.Alpha = 1);

        banner._timer = NSTimer.CreateScheduledTimer(request.Duration, _ => banner.Hide());
    }

    /// <summary>
    /// Hides the banner currently on screen, if any.
    /// </summary>
    public static void DismissCurrent() => _current?.Hide();

    private void Hide()
    {
        if (_current == this)
            _current = null;

        _timer?.Invalidate();
        _timer = null;

        UIView.Animate(
            AnimationSeconds,
            () => _root.Alpha = 0,
            () => _root.RemoveFromSuperview());
    }

    private static UIView BuildContainer(Request request)
    {
        var container = new UIView
        {
            TranslatesAutoresizingMaskIntoConstraints = false,
            BackgroundColor = request.BackgroundColor.ToPlatform(),
            ClipsToBounds = true
        };
        container.Layer.CornerRadius = (nfloat)request.CornerRadius;

        var label = new UILabel
        {
            TranslatesAutoresizingMaskIntoConstraints = false,
            Text = request.Text,
            TextColor = request.TextColor.ToPlatform(),
            Font = ResolveFont(request.FontFamily, request.FontSize),
            Lines = 0,
            LineBreakMode = UILineBreakMode.WordWrap,
            TextAlignment = UITextAlignment.Natural
        };

        var stack = new UIStackView
        {
            TranslatesAutoresizingMaskIntoConstraints = false,
            Axis = UILayoutConstraintAxis.Horizontal,
            Alignment = UIStackViewAlignment.Center,
            Spacing = 12
        };
        stack.AddArrangedSubview(label);

        if (!string.IsNullOrEmpty(request.ActionText))
        {
            var button = new UIButton(UIButtonType.System) { TranslatesAutoresizingMaskIntoConstraints = false };
            button.SetTitle(request.ActionText, UIControlState.Normal);
            button.SetTitleColor((request.ActionTextColor ?? request.TextColor).ToPlatform(), UIControlState.Normal);
            button.TitleLabel!.Font = ResolveFont(request.FontFamily, request.FontSize);
            button.SetContentHuggingPriority(999, UILayoutConstraintAxis.Horizontal);

            var action = request.Action;
            button.TouchUpInside += (_, _) =>
            {
                action?.Invoke();
                DismissCurrent();
            };

            stack.AddArrangedSubview(button);
        }

        container.AddSubview(stack);

        NSLayoutConstraint.ActivateConstraints(
        [
            stack.LeadingAnchor.ConstraintEqualTo(container.LeadingAnchor, 16),
            stack.TrailingAnchor.ConstraintEqualTo(container.TrailingAnchor, -16),
            stack.TopAnchor.ConstraintEqualTo(container.TopAnchor, 12),
            stack.BottomAnchor.ConstraintEqualTo(container.BottomAnchor, -12)
        ]);

        return container;
    }

    private static void ApplyConstraints(UIWindow window, UIView container)
    {
        var guide = window.SafeAreaLayoutGuide;

        NSLayoutConstraint.ActivateConstraints(
        [
            container.LeadingAnchor.ConstraintGreaterThanOrEqualTo(guide.LeadingAnchor, (nfloat)HorizontalMargin),
            container.TrailingAnchor.ConstraintLessThanOrEqualTo(guide.TrailingAnchor, (nfloat)(-HorizontalMargin)),
            container.CenterXAnchor.ConstraintEqualTo(guide.CenterXAnchor),
            container.BottomAnchor.ConstraintEqualTo(guide.BottomAnchor, (nfloat)(-BottomMargin))
        ]);
    }

    private static UIFont ResolveFont(string? fontFamily, double size)
    {
        if (!string.IsNullOrEmpty(fontFamily) && UIFont.FromName(fontFamily, (nfloat)size) is { } font)
            return font;

        return UIFont.SystemFontOfSize((nfloat)size);
    }
}
