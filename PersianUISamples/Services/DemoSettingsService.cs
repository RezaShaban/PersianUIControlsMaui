using System.Globalization;
using MauiPersianToolkit.Localization;

namespace PersianUISamples.Services;

public enum DemoThemeMode
{
    System = 0,
    Light = 1,
    Dark = 2
}

/// <summary>
/// Persists and applies gallery theme + language (fa / en / ar).
/// </summary>
public sealed class DemoSettingsService
{
    private const string ThemeKey = "demo.theme";
    private const string LanguageKey = "demo.language";

    public event EventHandler? Changed;

    public DemoThemeMode ThemeMode
    {
        get => (DemoThemeMode)Preferences.Default.Get(ThemeKey, (int)DemoThemeMode.System);
        private set => Preferences.Default.Set(ThemeKey, (int)value);
    }

    public string Language
    {
        get => Preferences.Default.Get(LanguageKey, "fa");
        private set => Preferences.Default.Set(LanguageKey, value);
    }

    public bool IsRtl => !string.Equals(Language, "en", StringComparison.OrdinalIgnoreCase);

    public IReadOnlyList<LanguageOption> Languages { get; } =
    [
        new("fa", "فارسی", "Persian"),
        new("en", "English", "English"),
        new("ar", "العربية", "Arabic"),
    ];

    public IReadOnlyList<ThemeOption> Themes { get; } =
    [
        new(DemoThemeMode.System, "system"),
        new(DemoThemeMode.Light, "light"),
        new(DemoThemeMode.Dark, "dark"),
    ];

    public void ApplyOnStartup()
    {
        ApplyTheme(ThemeMode, raise: false);
        ApplyLanguage(Language, recreateShell: false, raise: false);
    }

    public void SetTheme(DemoThemeMode mode)
    {
        if (ThemeMode == mode)
        {
            ApplyTheme(mode, raise: false);
            return;
        }

        ThemeMode = mode;
        ApplyTheme(mode, raise: true);
    }

    public void SetLanguage(string language)
    {
        language = NormalizeLanguage(language);
        if (string.Equals(Language, language, StringComparison.OrdinalIgnoreCase))
        {
            ApplyLanguage(language, recreateShell: false, raise: false);
            return;
        }

        Language = language;
        ApplyLanguage(language, recreateShell: true, raise: true);
    }

    private void ApplyTheme(DemoThemeMode mode, bool raise)
    {
        if (Application.Current is null)
            return;

        Application.Current.UserAppTheme = mode switch
        {
            DemoThemeMode.Light => AppTheme.Light,
            DemoThemeMode.Dark => AppTheme.Dark,
            _ => AppTheme.Unspecified
        };

        if (raise)
            Changed?.Invoke(this, EventArgs.Empty);
    }

    private void ApplyLanguage(string language, bool recreateShell, bool raise)
    {
        language = NormalizeLanguage(language);

        try
        {
            var culture = CultureInfo.GetCultureInfo(language switch
            {
                "fa" => "fa-IR",
                "ar" => "ar-SA",
                _ => "en-US"
            });
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
        catch (CultureNotFoundException)
        {
            // Keep previous culture if the OS lacks the pack.
        }

        PersianToolkitStrings.SetCulture(language);

        if (Shell.Current is not null)
            Shell.Current.FlowDirection = IsRtl ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

        if (recreateShell && Application.Current?.Windows.Count > 0)
        {
            var window = Application.Current.Windows[0];
            window.Page = new AppShell();
        }

        if (raise)
            Changed?.Invoke(this, EventArgs.Empty);
    }

    private static string NormalizeLanguage(string language)
    {
        language = (language ?? "fa").Trim().ToLowerInvariant();
        if (language.StartsWith("fa", StringComparison.Ordinal))
            return "fa";
        if (language.StartsWith("ar", StringComparison.Ordinal))
            return "ar";
        return "en";
    }
}

public record LanguageOption(string Code, string NativeName, string EnglishName);
public record ThemeOption(DemoThemeMode Mode, string Key);
