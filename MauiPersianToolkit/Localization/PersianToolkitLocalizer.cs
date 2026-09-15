using System.Collections.Concurrent;
using System.Globalization;

namespace MauiPersianToolkit.Localization;

/// <summary>
/// Built-in localizer with Persian and English catalogs. Hosts can switch culture,
/// register extra languages, or override individual strings.
/// </summary>
public sealed class PersianToolkitLocalizer : IPersianToolkitLocalizer
{
    private readonly ConcurrentDictionary<string, Dictionary<string, string>> _catalogs = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, string> _overrides = new(StringComparer.OrdinalIgnoreCase);
    private string _cultureName = "fa";

    /// <summary>
    /// Shared instance used by controls and dialogs.
    /// </summary>
    public static PersianToolkitLocalizer Current { get; private set; } = CreateDefault();

    /// <inheritdoc/>
    public string CultureName => _cultureName;

    /// <inheritdoc/>
    public string this[string id] => Get(id);

    /// <summary>
    /// Creates a localizer preloaded with <c>fa</c>, <c>en</c>, and <c>ar</c>.
    /// </summary>
    public static PersianToolkitLocalizer CreateDefault()
    {
        var localizer = new PersianToolkitLocalizer();
        localizer.RegisterCatalog("fa", BuiltInCatalogs.Persian);
        localizer.RegisterCatalog("en", BuiltInCatalogs.English);
        localizer.RegisterCatalog("ar", BuiltInCatalogs.Arabic);
        localizer.SetCulture("fa");
        return localizer;
    }

    /// <summary>
    /// Replaces the shared <see cref="Current"/> instance (e.g. after options configure).
    /// </summary>
    public static void SetCurrent(PersianToolkitLocalizer localizer)
    {
        Current = localizer ?? throw new ArgumentNullException(nameof(localizer));
    }

    /// <summary>
    /// Sets the active culture. Accepts <c>fa</c>, <c>fa-IR</c>, <c>en</c>, <c>en-US</c>, etc.
    /// Matching uses the language part first (<c>fa</c> from <c>fa-IR</c>).
    /// </summary>
    public void SetCulture(string cultureName)
    {
        if (string.IsNullOrWhiteSpace(cultureName))
            throw new ArgumentException("Culture name is required.", nameof(cultureName));

        _cultureName = cultureName.Trim();
    }

    /// <summary>
    /// Sets the active culture from a <see cref="CultureInfo"/>.
    /// </summary>
    public void SetCulture(CultureInfo culture) =>
        SetCulture(culture?.Name ?? throw new ArgumentNullException(nameof(culture)));

    /// <summary>
    /// Adds or replaces a full language catalog.
    /// </summary>
    public void RegisterCatalog(string cultureName, IReadOnlyDictionary<string, string> strings)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cultureName);
        ArgumentNullException.ThrowIfNull(strings);

        _catalogs[Normalize(cultureName)] = new Dictionary<string, string>(strings, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Overrides a single string for every culture until cleared.
    /// Useful for app-wide wording without shipping a full catalog.
    /// </summary>
    public void SetOverride(string id, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentNullException.ThrowIfNull(value);
        _overrides[id] = value;
    }

    /// <summary>
    /// Applies several overrides at once.
    /// </summary>
    public void SetOverrides(IReadOnlyDictionary<string, string> values)
    {
        ArgumentNullException.ThrowIfNull(values);
        foreach (var pair in values)
            SetOverride(pair.Key, pair.Value);
    }

    /// <summary>
    /// Removes a previously set override.
    /// </summary>
    public void ClearOverride(string id) => _overrides.TryRemove(id, out _);

    /// <inheritdoc/>
    public string Get(string id)
    {
        if (string.IsNullOrEmpty(id))
            return string.Empty;

        if (_overrides.TryGetValue(id, out var over))
            return over;

        if (TryGetFromCulture(_cultureName, id, out var value))
            return value;

        var language = LanguageOf(_cultureName);
        if (!string.Equals(language, _cultureName, StringComparison.OrdinalIgnoreCase)
            && TryGetFromCulture(language, id, out value))
            return value;

        if (TryGetFromCulture("fa", id, out value))
            return value;

        return id;
    }

    private bool TryGetFromCulture(string culture, string id, out string value)
    {
        value = string.Empty;
        if (_catalogs.TryGetValue(Normalize(culture), out var catalog)
            && catalog.TryGetValue(id, out value!))
            return true;

        return false;
    }

    private static string Normalize(string cultureName) => cultureName.Trim();

    private static string LanguageOf(string cultureName)
    {
        var dash = cultureName.IndexOf('-');
        return dash > 0 ? cultureName[..dash] : cultureName;
    }

    private static class BuiltInCatalogs
    {
        public static IReadOnlyDictionary<string, string> Persian { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            [PersianToolkitStringId.Accept] = "ثبت",
            [PersianToolkitStringId.Cancel] = "انصراف",
            [PersianToolkitStringId.Confirm] = "تایید",
            [PersianToolkitStringId.Ok] = "باشه",
            [PersianToolkitStringId.SystemErrorTitle] = "خطای سیستمی",
            [PersianToolkitStringId.SelectDate] = "انتخاب تاریخ",
            [PersianToolkitStringId.SelectTime] = "انتخاب زمان",
            [PersianToolkitStringId.Today] = "امروز",
            [PersianToolkitStringId.Now] = "الان",
            [PersianToolkitStringId.Hour] = "ساعت",
            [PersianToolkitStringId.Minute] = "دقیقه",
            [PersianToolkitStringId.Second] = "ثانیه",
            [PersianToolkitStringId.Am] = "ق.ظ",
            [PersianToolkitStringId.Pm] = "ب.ظ",
            [PersianToolkitStringId.NoResults] = "نتیجه‌ای یافت نشد",
            [PersianToolkitStringId.TypeToSearch] = "برای جستجو تایپ کنید",
        };

        public static IReadOnlyDictionary<string, string> English { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            [PersianToolkitStringId.Accept] = "Save",
            [PersianToolkitStringId.Cancel] = "Cancel",
            [PersianToolkitStringId.Confirm] = "OK",
            [PersianToolkitStringId.Ok] = "OK",
            [PersianToolkitStringId.SystemErrorTitle] = "System error",
            [PersianToolkitStringId.SelectDate] = "Select date",
            [PersianToolkitStringId.SelectTime] = "Select time",
            [PersianToolkitStringId.Today] = "Today",
            [PersianToolkitStringId.Now] = "Now",
            [PersianToolkitStringId.Hour] = "Hour",
            [PersianToolkitStringId.Minute] = "Minute",
            [PersianToolkitStringId.Second] = "Second",
            [PersianToolkitStringId.Am] = "AM",
            [PersianToolkitStringId.Pm] = "PM",
            [PersianToolkitStringId.NoResults] = "No results",
            [PersianToolkitStringId.TypeToSearch] = "Type to search",
        };

        public static IReadOnlyDictionary<string, string> Arabic { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            [PersianToolkitStringId.Accept] = "حفظ",
            [PersianToolkitStringId.Cancel] = "إلغاء",
            [PersianToolkitStringId.Confirm] = "تأكيد",
            [PersianToolkitStringId.Ok] = "حسناً",
            [PersianToolkitStringId.SystemErrorTitle] = "خطأ في النظام",
            [PersianToolkitStringId.SelectDate] = "اختر التاريخ",
            [PersianToolkitStringId.SelectTime] = "اختر الوقت",
            [PersianToolkitStringId.Today] = "اليوم",
            [PersianToolkitStringId.Now] = "الآن",
            [PersianToolkitStringId.Hour] = "ساعة",
            [PersianToolkitStringId.Minute] = "دقيقة",
            [PersianToolkitStringId.Second] = "ثانية",
            [PersianToolkitStringId.Am] = "ص",
            [PersianToolkitStringId.Pm] = "م",
            [PersianToolkitStringId.NoResults] = "لا توجد نتائج",
            [PersianToolkitStringId.TypeToSearch] = "اكتب للبحث",
        };
    }
}
