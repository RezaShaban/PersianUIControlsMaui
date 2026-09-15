using MauiPersianToolkit.Enums;
using MauiPersianToolkit.Localization;

namespace MauiPersianToolkit.Hosting;

/// <summary>
/// Application-wide defaults for the toolkit, configured through
/// <c>UseMauiPersianToolkit(options => ...)</c>.
/// </summary>
public class PersianToolkitOptions
{
    /// <summary>
    /// Calendar used by date controls that do not specify one. Defaults to
    /// <see cref="CalendarType.Persian"/>.
    /// </summary>
    public CalendarType DefaultCalendarType { get; set; } = CalendarType.Persian;

    /// <summary>
    /// Font applied to toolkit controls that do not specify one.
    /// </summary>
    public string DefaultFontFamily { get; set; } = "IranianSans";

    /// <summary>
    /// Font supplying the toolkit's glyphs.
    /// </summary>
    public string IconFontFamily { get; set; } = "FontAwesome";

    /// <summary>
    /// Label of the accept button in dialogs that do not specify one.
    /// When null, the active localizer's Accept string is used.
    /// </summary>
    public string? DefaultAcceptText { get; set; }

    /// <summary>
    /// Label of the cancel button in dialogs that do not specify one.
    /// When null, the active localizer's Cancel string is used.
    /// </summary>
    public string? DefaultCancelText { get; set; }

    /// <summary>
    /// Label of the acknowledge button in alerts that do not specify one.
    /// When null, the active localizer's Ok string is used.
    /// </summary>
    public string? DefaultConfirmText { get; set; }

    /// <summary>
    /// Whether the toolkit registers its embedded fonts. Turn this off to supply your own.
    /// </summary>
    public bool RegisterEmbeddedFonts { get; set; } = true;

    /// <summary>
    /// Optional light/dark color overrides for toolkit surfaces, inputs, dialogs and alerts.
    /// Unset properties keep the defaults from <c>PersianStyles</c>.
    /// </summary>
    public PersianThemeOptions Theme { get; set; } = new();

    /// <summary>
    /// UI culture and string catalogs for toolkit chrome (buttons, placeholders, dialogs).
    /// Defaults to Persian (<c>fa</c>); set Culture to <c>en</c> for English, or register more languages.
    /// </summary>
    public PersianToolkitLocalizationOptions Localization { get; set; } = new();

    /// <summary>
    /// Resolved accept label (options override or localized default).
    /// </summary>
    public string ResolveAcceptText() => DefaultAcceptText ?? PersianToolkitStrings.Accept;

    /// <summary>
    /// Resolved cancel label (options override or localized default).
    /// </summary>
    public string ResolveCancelText() => DefaultCancelText ?? PersianToolkitStrings.Cancel;

    /// <summary>
    /// Resolved OK / acknowledge label (options override or localized default).
    /// </summary>
    public string ResolveConfirmText() => DefaultConfirmText ?? PersianToolkitStrings.Ok;

    /// <summary>
    /// Currently configured options. Set when the toolkit is initialized.
    /// </summary>
    public static PersianToolkitOptions Current { get; internal set; } = new();
}
