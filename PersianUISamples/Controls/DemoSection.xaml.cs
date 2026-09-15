using MauiPersianToolkit;
using Microsoft.Maui.Controls.Shapes;

namespace PersianUISamples.Controls;

/// <summary>
/// Gallery card: title, optional subtitle, demo body, and a config hint consumers can skim.
/// Built in code so <see cref="ContentPropertyAttribute"/> does not fight ContentView XAML load.
/// </summary>
[ContentProperty(nameof(DemoContent))]
public class DemoSection : ContentView
{
    private readonly Label _titleLabel;
    private readonly Label _subtitleLabel;
    private readonly ContentView _bodyHost;
    private readonly Border _hintBorder;
    private readonly Label _hintLabel;

    public DemoSection()
    {
        PersianTheme.SeedControlResources(Resources);

        _titleLabel = new Label
        {
            FontFamily = "IranianSans",
            FontAttributes = FontAttributes.Bold,
            FontSize = 16
        };
        _titleLabel.SetAppThemeColor(
            Label.TextColorProperty,
            Color.FromArgb("#121212"),
            Color.FromArgb("#F2F2F2"));

        _subtitleLabel = new Label
        {
            FontFamily = "IranianSans",
            FontSize = 12,
            LineBreakMode = LineBreakMode.WordWrap,
            IsVisible = false
        };
        _subtitleLabel.SetAppThemeColor(
            Label.TextColorProperty,
            Color.FromArgb("#666666"),
            Color.FromArgb("#AAAAAA"));

        _bodyHost = new ContentView { Margin = new Thickness(0, 2, 0, 0) };

        _hintLabel = new Label
        {
            FontFamily = "IranianSans",
            FontSize = 11,
            LineBreakMode = LineBreakMode.WordWrap,
            TextColor = Color.FromArgb("#5B2BDF")
        };

        _hintBorder = new Border
        {
            StrokeThickness = 0,
            Padding = new Thickness(10, 8),
            IsVisible = false,
            StrokeShape = new RoundRectangle { CornerRadius = 8 },
            Content = _hintLabel
        };
        _hintBorder.SetAppThemeColor(
            Border.BackgroundColorProperty,
            Color.FromArgb("#145B2BDF"),
            Color.FromArgb("#335B2BDF"));

        var stack = new VerticalStackLayout
        {
            Spacing = 10,
            Children = { _titleLabel, _subtitleLabel, _bodyHost, _hintBorder }
        };

        var card = new Border
        {
            StrokeThickness = 1,
            Padding = 16,
            Margin = new Thickness(0, 0, 0, 14),
            StrokeShape = new RoundRectangle { CornerRadius = 14 },
            Shadow = new Shadow
            {
                Brush = Color.FromArgb("#30000000"),
                Offset = new Point(0, 2),
                Radius = 10,
                Opacity = 0.12f
            },
            Content = stack
        };
        card.SetAppTheme(
            Border.StrokeProperty,
            new SolidColorBrush(Color.FromArgb("#DCDCDC")),
            new SolidColorBrush(Color.FromArgb("#555555")));
        card.SetAppThemeColor(
            Border.BackgroundColorProperty,
            Colors.White,
            Color.FromArgb("#1E1E1E"));

        Content = card;
    }

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(DemoSection), string.Empty,
            propertyChanged: static (b, _, n) => ((DemoSection)b)._titleLabel.Text = n as string ?? string.Empty);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly BindableProperty SubtitleProperty =
        BindableProperty.Create(nameof(Subtitle), typeof(string), typeof(DemoSection), string.Empty,
            propertyChanged: static (b, _, n) =>
            {
                var section = (DemoSection)b;
                var text = n as string ?? string.Empty;
                section._subtitleLabel.Text = text;
                section._subtitleLabel.IsVisible = !string.IsNullOrWhiteSpace(text);
            });

    public string Subtitle
    {
        get => (string)GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    public static readonly BindableProperty HintProperty =
        BindableProperty.Create(nameof(Hint), typeof(string), typeof(DemoSection), string.Empty,
            propertyChanged: static (b, _, n) =>
            {
                var section = (DemoSection)b;
                var text = n as string ?? string.Empty;
                section._hintLabel.Text = text;
                section.HasHint = !string.IsNullOrWhiteSpace(text);
                section._hintBorder.IsVisible = section.HasHint;
            });

    public string Hint
    {
        get => (string)GetValue(HintProperty);
        set => SetValue(HintProperty, value);
    }

    public static readonly BindableProperty HasHintProperty =
        BindableProperty.Create(nameof(HasHint), typeof(bool), typeof(DemoSection), false);

    public bool HasHint
    {
        get => (bool)GetValue(HasHintProperty);
        private set => SetValue(HasHintProperty, value);
    }

    public static readonly BindableProperty DemoContentProperty =
        BindableProperty.Create(nameof(DemoContent), typeof(View), typeof(DemoSection),
            propertyChanged: static (b, _, n) => ((DemoSection)b)._bodyHost.Content = n as View);

    public View? DemoContent
    {
        get => (View?)GetValue(DemoContentProperty);
        set => SetValue(DemoContentProperty, value);
    }
}
