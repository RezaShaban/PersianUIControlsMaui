using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Color = Microsoft.Maui.Graphics.Color;
using Colors = Microsoft.Maui.Graphics.Colors;
using CornerRadius = Microsoft.Maui.CornerRadius;
using WinUIBorder = Microsoft.UI.Xaml.Controls.Border;
using WinUIButton = Microsoft.UI.Xaml.Controls.Button;
using WinUICornerRadius = Microsoft.UI.Xaml.CornerRadius;
using WinUIFontFamily = Microsoft.UI.Xaml.Media.FontFamily;
using WinUIPopup = Microsoft.UI.Xaml.Controls.Primitives.Popup;
using WinUISolidColorBrush = Microsoft.UI.Xaml.Media.SolidColorBrush;
using WinUIThickness = Microsoft.UI.Xaml.Thickness;
using WinUIVerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment;
using WinUIWindow = Microsoft.UI.Xaml.Window;

namespace MauiPersianToolkit.Platform;

/// <summary>
/// The bottom-anchored banner used for both toasts and snackbars. WinUI has no native
/// equivalent, so the toolkit renders its own into a light-dismiss-free popup.
/// </summary>
internal sealed class AlertBanner
{
    private const double BottomMargin = 48;

    private static AlertBanner? _current;

    private readonly WinUIPopup _popup;
    private readonly XamlRoot _xamlRoot;
    private readonly FrameworkElement _content;
    private readonly DispatcherTimer _timer;

    private AlertBanner(WinUIPopup popup, FrameworkElement content, XamlRoot xamlRoot, TimeSpan duration)
    {
        _popup = popup;
        _content = content;
        _xamlRoot = xamlRoot;

        _timer = new DispatcherTimer { Interval = duration };
        _timer.Tick += (_, _) => Hide();
    }

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
        CornerRadius CornerRadius,
        string? ActionText = null,
        Action? Action = null,
        Color? ActionTextColor = null);

    /// <summary>
    /// Shows a banner, replacing whichever one is currently on screen.
    /// </summary>
    public static void Show(Request request)
    {
        DismissCurrent();

        var xamlRoot = ResolveXamlRoot();
        if (xamlRoot is null)
            return;

        var content = BuildContent(request);
        var popup = new WinUIPopup
        {
            XamlRoot = xamlRoot,
            IsLightDismissEnabled = false,
            Child = content
        };

        var banner = new AlertBanner(popup, content, xamlRoot, request.Duration);
        _current = banner;

        content.Loaded += banner.OnContentLoaded;
        popup.IsOpen = true;
        banner._timer.Start();
    }

    /// <summary>
    /// Hides the banner currently on screen, if any.
    /// </summary>
    public static void DismissCurrent() => _current?.Hide();

    private void Hide()
    {
        if (_current == this)
            _current = null;

        _timer.Stop();
        _content.Loaded -= OnContentLoaded;
        _popup.IsOpen = false;
        _popup.Child = null;
    }

    /// <summary>
    /// The banner is centered horizontally and pinned above the bottom edge, which can only
    /// be computed once WinUI has measured it.
    /// </summary>
    private void OnContentLoaded(object sender, RoutedEventArgs e)
    {
        _content.Measure(new global::Windows.Foundation.Size(_xamlRoot.Size.Width, _xamlRoot.Size.Height));
        var desired = _content.DesiredSize;

        _popup.HorizontalOffset = Math.Max(0, (_xamlRoot.Size.Width - desired.Width) / 2);
        _popup.VerticalOffset = Math.Max(0, _xamlRoot.Size.Height - desired.Height - BottomMargin);
    }

    private static FrameworkElement BuildContent(Request request)
    {
        var text = new TextBlock
        {
            Text = request.Text,
            Foreground = new WinUISolidColorBrush(request.TextColor.ToWindowsColor()),
            FontSize = request.FontSize,
            TextWrapping = TextWrapping.Wrap,
            VerticalAlignment = WinUIVerticalAlignment.Center
        };

        if (!string.IsNullOrEmpty(request.FontFamily))
            text.FontFamily = new WinUIFontFamily(request.FontFamily);

        var panel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 12,
            VerticalAlignment = WinUIVerticalAlignment.Center
        };
        panel.Children.Add(text);

        if (!string.IsNullOrEmpty(request.ActionText))
        {
            var button = new WinUIButton
            {
                Content = request.ActionText,
                Background = new WinUISolidColorBrush(Colors.Transparent.ToWindowsColor()),
                BorderThickness = new WinUIThickness(0),
                Foreground = new WinUISolidColorBrush((request.ActionTextColor ?? request.TextColor).ToWindowsColor()),
                FontSize = request.FontSize,
                VerticalAlignment = WinUIVerticalAlignment.Center
            };

            var action = request.Action;
            button.Click += (_, _) =>
            {
                action?.Invoke();
                DismissCurrent();
            };

            panel.Children.Add(button);
        }

        return new WinUIBorder
        {
            Background = new WinUISolidColorBrush(request.BackgroundColor.ToWindowsColor()),
            CornerRadius = new WinUICornerRadius(
                request.CornerRadius.TopLeft,
                request.CornerRadius.TopRight,
                request.CornerRadius.BottomRight,
                request.CornerRadius.BottomLeft),
            Padding = new WinUIThickness(16, 12, 16, 12),
            MaxWidth = 560,
            Child = panel
        };
    }

    private static XamlRoot? ResolveXamlRoot()
    {
        var platformWindow = Microsoft.Maui.Controls.Application.Current?
            .Windows.FirstOrDefault()?
            .Handler?.PlatformView as WinUIWindow;

        return (platformWindow?.Content as FrameworkElement)?.XamlRoot;
    }
}
