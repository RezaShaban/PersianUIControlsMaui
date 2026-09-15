using System.Collections.ObjectModel;
using MauiPersianToolkit.ViewModels;
using PersianUISamples.Localization;
using PersianUISamples.Services;

namespace PersianUISamples.ViewModels;

public class SettingsViewModel : ObservableObject
{
    private readonly DemoSettingsService _settings;

    public SettingsViewModel(DemoSettingsService settings)
    {
        _settings = settings;
        Refresh();
        SelectThemeCommand = new Command<DemoThemeMode>(mode =>
        {
            _settings.SetTheme(mode);
            Refresh();
        });
        SelectLanguageCommand = new Command<string>(code =>
        {
            if (string.IsNullOrWhiteSpace(code))
                return;
            _settings.SetLanguage(code);
            // Shell is recreated; no need to refresh this instance.
        });
    }

    public string PageTitle => DemoStrings.Settings;
    public string PageLead => DemoStrings.SettingsLead;
    public string ThemeTitle => DemoStrings.ThemeTitle;
    public string ThemeHint => DemoStrings.ThemeHint;
    public string LanguageTitle => DemoStrings.LanguageTitle;
    public string LanguageHint => DemoStrings.LanguageHint;
    public string ToolkitNoteTitle => "MauiPersianToolkit";
    public string ToolkitNote => DemoStrings.ToolkitChromeNote;

    public string ActiveSummary =>
        $"{DemoStrings.ThemeLabel(_settings.ThemeMode)} · {_settings.Language.ToUpperInvariant()}";

    public ObservableCollection<ThemeChoice> Themes { get; } = [];
    public ObservableCollection<LanguageChoice> Languages { get; } = [];

    public Command<DemoThemeMode> SelectThemeCommand { get; }
    public Command<string> SelectLanguageCommand { get; }

    private void Refresh()
    {
        Themes.Clear();
        foreach (var theme in _settings.Themes)
        {
            Themes.Add(new ThemeChoice(
                theme.Mode,
                DemoStrings.ThemeLabel(theme.Mode),
                theme.Mode == _settings.ThemeMode));
        }

        Languages.Clear();
        foreach (var lang in _settings.Languages)
        {
            Languages.Add(new LanguageChoice(
                lang.Code,
                lang.NativeName,
                lang.EnglishName,
                string.Equals(lang.Code, _settings.Language, StringComparison.OrdinalIgnoreCase)));
        }

        OnPropertyChanged(nameof(PageTitle));
        OnPropertyChanged(nameof(PageLead));
        OnPropertyChanged(nameof(ThemeTitle));
        OnPropertyChanged(nameof(ThemeHint));
        OnPropertyChanged(nameof(LanguageTitle));
        OnPropertyChanged(nameof(LanguageHint));
        OnPropertyChanged(nameof(ToolkitNote));
        OnPropertyChanged(nameof(ActiveSummary));
    }
}

public record ThemeChoice(DemoThemeMode Mode, string Label, bool IsSelected);
public record LanguageChoice(string Code, string NativeName, string EnglishName, bool IsSelected);
