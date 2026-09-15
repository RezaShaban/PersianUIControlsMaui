namespace MauiPersianToolkit.Core;

/// <summary>
/// Appearance of a snackbar surface.
/// </summary>
public class SnackbarOptions
{
    /// <summary>
    /// Color of the message text.
    /// </summary>
    public Color TextColor { get; set; } = ThemeColors.AlertForeground;

    /// <summary>
    /// Color of the snackbar surface.
    /// </summary>
    public Color BackgroundColor { get; set; } = ThemeColors.AlertBackground;

    /// <summary>
    /// Color of the action button text.
    /// </summary>
    public Color ActionButtonTextColor { get; set; } = ThemeColors.AlertForeground;

    /// <summary>
    /// Corner rounding of the snackbar surface.
    /// </summary>
    public CornerRadius CornerRadius { get; set; } = new(4);

    /// <summary>
    /// Size of the message text.
    /// </summary>
    public double CharacterSpacing { get; set; } = 0d;

    /// <summary>
    /// Font applied to the message and the action button.
    /// </summary>
    public string? FontFamily { get; set; }

    /// <summary>
    /// Size of the message text.
    /// </summary>
    public double FontSize { get; set; } = 14d;

    /// <summary>
    /// Size of the action button text.
    /// </summary>
    public double ActionButtonFontSize { get; set; } = 14d;
}
