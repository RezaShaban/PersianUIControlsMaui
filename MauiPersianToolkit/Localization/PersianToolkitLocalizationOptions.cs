namespace MauiPersianToolkit.Localization;

/// <summary>
/// Localization settings applied through <c>UseMauiPersianToolkit</c>.
/// </summary>
public sealed class PersianToolkitLocalizationOptions
{
    /// <summary>
    /// Initial UI culture for toolkit chrome strings. Defaults to <c>fa</c>.
    /// Examples: <c>fa</c>, <c>fa-IR</c>, <c>en</c>, <c>en-US</c>, <c>ar</c>, <c>ar-SA</c>.
    /// Built-in catalogs: Persian (<c>fa</c>), English (<c>en</c>), Arabic (<c>ar</c>).
    /// </summary>
    public string Culture { get; set; } = "fa";

    /// <summary>
    /// Optional per-key overrides applied on top of the active catalog
    /// (e.g. change Accept wording without translating the whole UI).
    /// </summary>
    public Dictionary<string, string> StringOverrides { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Extra language catalogs keyed by culture name. Merged after built-in <c>fa</c>/<c>en</c>/<c>ar</c>.
    /// </summary>
    public Dictionary<string, IReadOnlyDictionary<string, string>> AdditionalCatalogs { get; set; } =
        new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Optional custom localizer. When set, it replaces the built-in one entirely.
    /// </summary>
    public IPersianToolkitLocalizer? CustomLocalizer { get; set; }

    internal void Apply()
    {
        if (CustomLocalizer is not null)
        {
            PersianToolkitStrings.Current = CustomLocalizer;
            return;
        }

        var localizer = PersianToolkitLocalizer.CreateDefault();

        foreach (var catalog in AdditionalCatalogs)
            localizer.RegisterCatalog(catalog.Key, catalog.Value);

        if (StringOverrides.Count > 0)
            localizer.SetOverrides(StringOverrides);

        localizer.SetCulture(Culture);
        PersianToolkitLocalizer.SetCurrent(localizer);
        PersianToolkitStrings.Current = localizer;
    }
}
