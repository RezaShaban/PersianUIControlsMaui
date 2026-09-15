namespace MauiPersianToolkit.Hosting;

/// <summary>
/// Optional light/dark palette overrides for toolkit controls, dialogs and alerts.
/// Only non-null properties replace the default <c>Pdt*</c> resource values.
/// </summary>
/// <example>
/// <code>
/// builder.UseMauiPersianToolkit(o =>
/// {
///     o.Theme.SurfaceDark = Color.FromArgb("#0D1117");
///     o.Theme.Accent = Colors.Teal;
/// });
/// </code>
/// </example>
public sealed class PersianThemeOptions
{
    public Color? PageLight { get; set; }
    public Color? PageDark { get; set; }
    public Color? SurfaceLight { get; set; }
    public Color? SurfaceDark { get; set; }
    public Color? InputFillLight { get; set; }
    public Color? InputFillDark { get; set; }
    public Color? OutlineLight { get; set; }
    public Color? OutlineDark { get; set; }
    public Color? OnSurfaceLight { get; set; }
    public Color? OnSurfaceDark { get; set; }
    public Color? MutedLight { get; set; }
    public Color? MutedDark { get; set; }
    public Color? FooterLight { get; set; }
    public Color? FooterDark { get; set; }
    public Color? DisabledLight { get; set; }
    public Color? DisabledDark { get; set; }
    public Color? DayButtonLight { get; set; }
    public Color? DayButtonDark { get; set; }
    public Color? Accept { get; set; }
    public Color? Cancel { get; set; }
    public Color? Accent { get; set; }
    public Color? AlertBackgroundLight { get; set; }
    public Color? AlertBackgroundDark { get; set; }
    public Color? AlertForegroundLight { get; set; }
    public Color? AlertForegroundDark { get; set; }

    /// <summary>
    /// Writes non-null overrides into <paramref name="resources"/> under
    /// <see cref="PersianThemeKeys"/>.
    /// </summary>
    public void Apply(ResourceDictionary resources)
    {
        ArgumentNullException.ThrowIfNull(resources);

        Set(resources, PersianThemeKeys.PageLight, PageLight);
        Set(resources, PersianThemeKeys.PageDark, PageDark);
        Set(resources, PersianThemeKeys.SurfaceLight, SurfaceLight);
        Set(resources, PersianThemeKeys.SurfaceDark, SurfaceDark);
        Set(resources, PersianThemeKeys.InputFillLight, InputFillLight);
        Set(resources, PersianThemeKeys.InputFillDark, InputFillDark);
        Set(resources, PersianThemeKeys.OutlineLight, OutlineLight);
        Set(resources, PersianThemeKeys.OutlineDark, OutlineDark);
        Set(resources, PersianThemeKeys.OnSurfaceLight, OnSurfaceLight);
        Set(resources, PersianThemeKeys.OnSurfaceDark, OnSurfaceDark);
        Set(resources, PersianThemeKeys.MutedLight, MutedLight);
        Set(resources, PersianThemeKeys.MutedDark, MutedDark);
        Set(resources, PersianThemeKeys.FooterLight, FooterLight);
        Set(resources, PersianThemeKeys.FooterDark, FooterDark);
        Set(resources, PersianThemeKeys.DisabledLight, DisabledLight);
        Set(resources, PersianThemeKeys.DisabledDark, DisabledDark);
        Set(resources, PersianThemeKeys.DayButtonLight, DayButtonLight);
        Set(resources, PersianThemeKeys.DayButtonDark, DayButtonDark);
        Set(resources, PersianThemeKeys.Accept, Accept);
        Set(resources, PersianThemeKeys.Cancel, Cancel);
        Set(resources, PersianThemeKeys.Accent, Accent);
        Set(resources, PersianThemeKeys.AlertBgLight, AlertBackgroundLight);
        Set(resources, PersianThemeKeys.AlertBgDark, AlertBackgroundDark);
        Set(resources, PersianThemeKeys.AlertFgLight, AlertForegroundLight);
        Set(resources, PersianThemeKeys.AlertFgDark, AlertForegroundDark);
    }

    private static void Set(ResourceDictionary resources, string key, Color? color)
    {
        if (color is null)
            return;

        resources[key] = color;
    }
}
