using MauiPersianToolkit.Core;

namespace MauiPersianToolkit.Controls;

/// <summary>
/// Full-window layout the platform popup hosts render. It paints the dimmed backdrop,
/// positions the popup according to its own layout options, and turns a tap on the
/// backdrop into a dismissal.
/// </summary>
/// <remarks>
/// Keeping the backdrop as a sibling of the popup rather than as the container's own
/// background means taps on the popup content never reach the dismiss handler.
/// The popup is always wrapped in a host view so <see cref="PlaceSurface"/> can set
/// VerticalOptions on the wrapper without overwriting the popup's own End/Start/Center
/// (required for reused bottom sheets such as <c>PickerView</c>).
/// </remarks>
internal sealed class PopupContainer : Grid
{
    private readonly Popup _popup;
    private readonly View _surface;

    public PopupContainer(Popup popup, PopupOptions options)
    {
        _popup = popup;

        IgnoreSafeArea = true;
        BackgroundColor = Colors.Transparent;
        HorizontalOptions = LayoutOptions.Fill;
        VerticalOptions = LayoutOptions.Fill;

        var backdrop = new BoxView
        {
            Color = options.BackgroundColor,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill
        };
        backdrop.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(OnBackdropTapped) });

        _surface = BuildSurface(popup, options);
        PlaceSurface(popup.HorizontalOptions, popup.VerticalOptions);

        Children.Add(backdrop);
        Children.Add(_surface);

        // Dim the whole window, including the area behind a bottom sheet.
        if (RowDefinitions.Count > 1)
            SetRowSpan((BindableObject)backdrop, RowDefinitions.Count);
    }

    /// <summary>
    /// Anchors the surface with a real grid row for End/Start so bottom sheets sit flush
    /// against the window edge. Center/Fill keep a single full-window cell.
    /// </summary>
    private void PlaceSurface(LayoutOptions horizontal, LayoutOptions vertical)
    {
        _surface.HorizontalOptions = horizontal.Alignment == LayoutAlignment.Fill
            ? LayoutOptions.Fill
            : horizontal;

        switch (vertical.Alignment)
        {
            case LayoutAlignment.End:
                RowDefinitions =
                [
                    new RowDefinition(GridLength.Star),
                    new RowDefinition(GridLength.Auto)
                ];
                // Hug content inside the bottom Auto row; do not mutate popup.VerticalOptions.
                _surface.VerticalOptions = LayoutOptions.Start;
                SetRow((BindableObject)_surface, 1);
                break;

            case LayoutAlignment.Start:
                RowDefinitions =
                [
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(GridLength.Star)
                ];
                _surface.VerticalOptions = LayoutOptions.Start;
                SetRow((BindableObject)_surface, 0);
                break;

            default:
                // Center or Fill: one cell covering the window; the surface's own
                // VerticalOptions places it inside that cell.
                _surface.VerticalOptions = vertical;
                break;
        }
    }

    /// <summary>
    /// Wraps the popup so placement options can be applied without clobbering
    /// <see cref="Popup.VerticalOptions"/> on reused instances.
    /// </summary>
    private static View BuildSurface(Popup popup, PopupOptions options)
    {
        if (options.Shape is null && options.Shadow is null)
        {
            return new ContentView
            {
                Content = popup,
                Padding = 0,
                BackgroundColor = Colors.Transparent,
                HorizontalOptions = LayoutOptions.Fill
            };
        }

        return new Border
        {
            Content = popup,
            Padding = 0,
            StrokeThickness = 0,
            BackgroundColor = Colors.Transparent,
            StrokeShape = options.Shape,
            Shadow = options.Shadow,
            HorizontalOptions = LayoutOptions.Fill
        };
    }

    private void OnBackdropTapped()
    {
        if (!_popup.ResolveDismissOnTappingOutside())
            return;

        _ = _popup.DismissAsync();
    }

    /// <summary>
    /// Removes the popup from this container so the same popup instance can be shown again.
    /// </summary>
    public void Release(Popup popup)
    {
        if (_surface is Border border)
            border.Content = null;
        else if (_surface is ContentView host)
            host.Content = null;
        else
            Children.Remove(popup);

        Parent = null;
    }
}
