using Microsoft.Maui.Controls.Shapes;

namespace MauiPersianToolkit.Core;

/// <summary>
/// Per-display customization passed to <c>ShowPopupAsync</c>.
/// </summary>
public class PopupOptions
{
    /// <summary>
    /// Options used when a caller does not supply any.
    /// </summary>
    /// <remarks>
    /// A new instance every time: <see cref="Shape"/> holds a live element that cannot be
    /// shared between two popups on screen at once.
    /// </remarks>
    public static PopupOptions Default => new();

    /// <summary>
    /// Outline drawn around the popup surface. <see langword="null"/> leaves the
    /// surface unshaped, letting the popup content define its own borders.
    /// </summary>
    public IShape? Shape { get; set; } = new RoundRectangle
    {
        CornerRadius = new CornerRadius(20),
        Stroke = ThemeColors.Outline,
        StrokeThickness = 1
    };

    /// <summary>
    /// Shadow cast by the popup surface. <see langword="null"/> disables the shadow.
    /// </summary>
    public Shadow? Shadow { get; set; }

    /// <summary>
    /// Color painted over the rest of the window while the popup is visible.
    /// </summary>
    public Color BackgroundColor { get; set; } = Color.FromRgba(0, 0, 0, 102);

    /// <summary>
    /// Overrides <see cref="IPopup.CanBeDismissedByTappingOutsideOfPopup"/> for this
    /// display when set.
    /// </summary>
    public bool? CanBeDismissedByTappingOutsideOfPopup { get; set; }

    /// <summary>
    /// Invoked when the user dismisses the popup by tapping outside of it.
    /// </summary>
    public Action? OnTappingOutsideOfPopup { get; set; }
}
