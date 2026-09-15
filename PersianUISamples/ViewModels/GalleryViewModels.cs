using MauiPersianToolkit.Controls;
using MauiPersianToolkit.Models;
using MauiPersianToolkit.Services.Dialog;
using MauiPersianToolkit.ViewModels;
using PersianUISamples.Localization;
using PersianUISamples.Models;
using PersianUISamples.Services;
using System.Collections.ObjectModel;

namespace PersianUISamples.ViewModels;

public class HomeViewModel : ObservableObject
{
    private readonly DemoSettingsService _settings;

    public HomeViewModel(DemoSettingsService settings)
    {
        _settings = settings;
        OpenSettingsCommand = new Command(async () => await Shell.Current.GoToAsync("//settings"));
        Refresh();
    }

    public string VersionLabel => "MauiPersianToolkit · .NET 10";
    public int ControlCount => 20;
    public string PageTitle => DemoStrings.GalleryTitle;
    public string Intro => DemoStrings.Intro;
    public string CategoriesTitle => DemoStrings.Categories;
    public string CategoriesLead => DemoStrings.CategoriesLead;
    public string ControlCountText => string.Format(DemoStrings.ControlCountFormat, ControlCount);
    public string HowToTitle => DemoStrings.HowToTitle;
    public string HowToHint => DemoStrings.HowToHint;
    public string HowTo1 => DemoStrings.HowTo1;
    public string HowTo2 => DemoStrings.HowTo2;
    public string HowTo3 => DemoStrings.HowTo3;
    public string ThemeChip => DemoStrings.ThemeLabel(_settings.ThemeMode);
    public string LanguageChip => _settings.Language.ToUpperInvariant();
    public Command OpenSettingsCommand { get; }
    public ObservableCollection<CategoryCard> Categories { get; } = [];

    public void Refresh()
    {
        Categories.Clear();
        Categories.Add(new(DemoStrings.NavInputs, DemoStrings.CatInputsSub, "inputs", "\uf040"));
        Categories.Add(new(DemoStrings.NavAutoComplete, DemoStrings.CatAutoCompleteSub, "autocomplete", "\uf002"));
        Categories.Add(new(DemoStrings.NavPickers, DemoStrings.CatPickersSub, "pickers", "\uf0d7"));
        Categories.Add(new(DemoStrings.NavDateTime, DemoStrings.CatDateTimeSub, "datetime", "\uf073"));
        Categories.Add(new(DemoStrings.NavLayout, DemoStrings.CatLayoutSub, "layout", "\uf0c9"));
        Categories.Add(new(DemoStrings.NavTree, DemoStrings.CatTreeSub, "tree", "\uf0e8"));
        Categories.Add(new(DemoStrings.NavDialogs, DemoStrings.CatDialogsSub, "dialogs", "\uf075"));
    }
}

public record CategoryCard(string Title, string Subtitle, string Route, string Icon);

public class InputsViewModel : ObservableObject
{
    private string _name = string.Empty;
    private string _notes = string.Empty;
    private bool _agree;
    private bool _toggleOn;

    public string PageTitle => DemoStrings.NavInputs;
    public string PageLead => DemoStrings.InputsLead;
    public string EntrySectionTitle => DemoStrings.EntrySectionTitle;
    public string EntrySectionSub => DemoStrings.EntrySectionSub;
    public string EntrySectionHint => DemoStrings.EntrySectionHint;
    public string EntryPlaceholder => DemoStrings.EntryPlaceholder;
    public string EditorSectionTitle => DemoStrings.EditorSectionTitle;
    public string EditorSectionSub => DemoStrings.EditorSectionSub;
    public string EditorSectionHint => DemoStrings.EditorSectionHint;
    public string EditorTitle => DemoStrings.EditorTitle;
    public string EditorPlaceholder => DemoStrings.EditorPlaceholder;
    public string LabelSectionTitle => DemoStrings.LabelSectionTitle;
    public string LabelSectionHint => DemoStrings.LabelSectionHint;
    public string LabelSample => DemoStrings.LabelSample;
    public string CheckSectionTitle => DemoStrings.CheckSectionTitle;
    public string CheckSectionHint => DemoStrings.CheckSectionHint;
    public string CheckAgree => DemoStrings.CheckAgree;
    public string ButtonSectionTitle => DemoStrings.ButtonSectionTitle;
    public string ButtonSectionHint => DemoStrings.ButtonSectionHint;
    public string ButtonSave => DemoStrings.ButtonSave;
    public string ToggleSectionTitle => DemoStrings.ToggleSectionTitle;
    public string ToggleSectionHint => DemoStrings.ToggleSectionHint;
    public string ToggleStatusText => string.Format(DemoStrings.ToggleStatusFormat, ToggleOn);

    public string Name { get => _name; set => SetProperty(ref _name, value); }
    public string Notes { get => _notes; set => SetProperty(ref _notes, value); }
    public bool Agree { get => _agree; set => SetProperty(ref _agree, value); }
    public bool ToggleOn
    {
        get => _toggleOn;
        set
        {
            if (SetProperty(ref _toggleOn, value))
                OnPropertyChanged(nameof(ToggleStatusText));
        }
    }
    public Command PrimaryCommand { get; } = new(() => { });
}

public class AutoCompleteGalleryViewModel : ObservableObject
{
    private string _cityText = string.Empty;
    private PersianUISamples.Models.PickerItem _selectedCity;

    public string PageTitle => DemoStrings.NavAutoComplete;
    public string PageLead => DemoStrings.AutoCompleteLead;
    public string SectionTitle => DemoStrings.CitySuggestTitle;
    public string SectionSub => DemoStrings.CitySuggestSub;
    public string SectionHint => DemoStrings.CitySuggestHint;
    public string CityPlaceholder => DemoStrings.CityPlaceholder;
    public string SelectedText => SelectedCity is null
        ? DemoStrings.SelectedNone
        : string.Format(DemoStrings.SelectedFormat, SelectedCity.Title);
    public string TypedText => string.Format(DemoStrings.TextFormat, CityText);

    public ObservableCollection<PersianUISamples.Models.PickerItem> Cities { get; } = SampleData.CreateCities();
    public string CityText
    {
        get => _cityText;
        set
        {
            if (SetProperty(ref _cityText, value))
                OnPropertyChanged(nameof(TypedText));
        }
    }
    public PersianUISamples.Models.PickerItem SelectedCity
    {
        get => _selectedCity;
        set
        {
            if (SetProperty(ref _selectedCity, value))
                OnPropertyChanged(nameof(SelectedText));
        }
    }

    public AutoCompleteOptions Options { get; } = new()
    {
        FilterMode = MauiPersianToolkit.Enums.AutoCompleteFilterMode.Contains,
        MinimumPrefixLength = 1,
        MaxSuggestions = 8,
        UseNativeSuggestions = true,
        ShowClearButton = true,
        OpenOnFocus = true,
        FilterDebounceMs = 100
    };
}

public class PickersGalleryViewModel : ObservableObject
{
    public string PageTitle => DemoStrings.NavPickers;
    public string PageLead => DemoStrings.PickersLead;
    public string SingleTitle => DemoStrings.PickerSingleTitle;
    public string SingleHint => DemoStrings.PickerSingleHint;
    public string SinglePlaceholder => DemoStrings.PickerSinglePlaceholder;
    public string SingleDialogTitle => DemoStrings.PickerSingleDialogTitle;
    public string MultiTitle => DemoStrings.PickerMultiTitle;
    public string MultiHint => DemoStrings.PickerMultiHint;
    public string MultiPlaceholder => DemoStrings.PickerMultiPlaceholder;
    public string MultiDialogTitle => DemoStrings.PickerMultiDialogTitle;

    public ObservableCollection<PersianUISamples.Models.PickerItem> Items { get; } = SampleData.CreatePickerItems();
    public ObservableCollection<PersianUISamples.Models.PickerItem> MultiItems { get; } = SampleData.CreatePickerItems();

    public ObservableCollection<PickerButton> ExtraButtons { get; } =
    [
        new() { Text = "\uf067" },
        new() { Text = "\uf057" }
    ];
}

public class DateTimeGalleryViewModel : ObservableObject
{
    private string _singleDate = string.Empty;
    private string _rangeDate = string.Empty;
    private string _multiDate = string.Empty;
    private TimeSpan _meeting = new(14, 30, 0);
    private TimeSpan _alarm;
    private List<string> _rangeBadges = [];
    private List<string> _multiBadges = [];

    public string PageTitle => DemoStrings.NavDateTime;
    public string PageLead => DemoStrings.DateTimeLead;
    public string HijriTitle => DemoStrings.DateHijriTitle;
    public string HijriHint => DemoStrings.DateHijriHint;
    public string HijriPlaceholder => DemoStrings.DateHijriPlaceholder;
    public string RangeTitle => DemoStrings.DateRangeTitle;
    public string RangeHint => DemoStrings.DateRangeHint;
    public string RangePlaceholder => DemoStrings.DateRangePlaceholder;
    public string MultiTitle => DemoStrings.DateMultiTitle;
    public string MultiHint => DemoStrings.DateMultiHint;
    public string MultiPlaceholder => DemoStrings.DateMultiPlaceholder;
    public string Time24Title => DemoStrings.Time24Title;
    public string Time24Hint => DemoStrings.Time24Hint;
    public string Time24Placeholder => DemoStrings.Time24Placeholder;
    public string Time12Title => DemoStrings.Time12Title;
    public string Time12Hint => DemoStrings.Time12Hint;
    public string Time12Placeholder => DemoStrings.Time12Placeholder;
    public string DateViewTitle => DemoStrings.DateViewTitle;
    public string DateViewHint => DemoStrings.DateViewHint;

    public string SingleDate { get => _singleDate; set => SetProperty(ref _singleDate, value); }
    public string RangeDate { get => _rangeDate; set => SetProperty(ref _rangeDate, value); }
    public string MultiDate { get => _multiDate; set => SetProperty(ref _multiDate, value); }
    public List<string> RangeBadges { get => _rangeBadges; set => SetProperty(ref _rangeBadges, value); }
    public List<string> MultiBadges { get => _multiBadges; set => SetProperty(ref _multiBadges, value); }
    public TimeSpan MeetingTime { get => _meeting; set => SetProperty(ref _meeting, value); }
    public TimeSpan AlarmTime { get => _alarm; set => SetProperty(ref _alarm, value); }

    public CalendarOptions HijriSingle { get; } = new()
    {
        CalendarType = MauiPersianToolkit.Enums.CalendarType.Hijri,
        SelectionMode = MauiPersianToolkit.Enums.SelectionMode.Single,
        SelectDayColor = Color.FromArgb("#5B2BDF"),
        MinDateCanSelect = DateTime.Now.AddDays(-10),
        MaxDateCanSelect = DateTime.Now.AddDays(10),
        AutoCloseAfterSelectDate = true
    };

    public CalendarOptions PersianRange { get; } = new()
    {
        CalendarType = MauiPersianToolkit.Enums.CalendarType.Persian,
        SelectionMode = MauiPersianToolkit.Enums.SelectionMode.Range,
        SelectDayColor = Color.FromArgb("#5B2BDF"),
        AutoCloseAfterSelectDate = false,
        MinDateCanSelect = DateTime.Now.Date,
        InactiveDays =
        [
            DateTime.Now.AddDays(2),
            DateTime.Now.AddDays(5),
            DateTime.Now.AddDays(7)
        ],
        CanSelectHolidays = true
    };

    public CalendarOptions GregorianMulti { get; } = new()
    {
        CalendarType = MauiPersianToolkit.Enums.CalendarType.Gregorian,
        SelectionMode = MauiPersianToolkit.Enums.SelectionMode.Multiple,
        SelectDayColor = Color.FromArgb("#5B2BDF"),
        AutoCloseAfterSelectDate = false,
        CanSelectHolidays = true
    };

    public DateTimeGalleryViewModel()
    {
        PersianRange.OnAccept = OnRangeAccept;
        GregorianMulti.OnAccept = OnMultiAccept;
    }

    private void OnRangeAccept(object obj)
    {
        if (obj is not List<DayOfMonth> dates)
            return;
        RangeBadges = dates.Select(x => x.PersianDate).ToList();
        RangeDate = dates.FirstOrDefault()?.PersianDate ?? string.Empty;
    }

    private void OnMultiAccept(object obj)
    {
        if (obj is not List<DayOfMonth> dates)
            return;
        MultiBadges = dates.Select(x => x.PersianDate).ToList();
        MultiDate = dates.FirstOrDefault()?.PersianDate ?? string.Empty;
    }
}

public class LayoutGalleryViewModel : ObservableObject
{
    public string PageTitle => DemoStrings.NavLayout;
    public string PageLead => DemoStrings.LayoutLead;
    public string TabHint => DemoStrings.TabSectionHint;
    public string Tab1 => DemoStrings.Tab1;
    public string Tab2 => DemoStrings.Tab2;
    public string Tab3 => DemoStrings.Tab3;
    public string Tab1Content => DemoStrings.Tab1Content;
    public string Tab2Content => DemoStrings.Tab2Content;
    public string Tab3Content => DemoStrings.Tab3Content;
    public string ContainerHint => DemoStrings.ContainerSectionHint;
    public string ContainerTitle => DemoStrings.ContainerTitle;
    public string ContainerLeft => DemoStrings.ContainerLeft;
    public string ContainerBody => DemoStrings.ContainerBody;
    public string ExpanderHint => DemoStrings.ExpanderSectionHint;
    public string ExpanderHeader => DemoStrings.ExpanderHeader;
    public string ExpanderBody => DemoStrings.ExpanderBody;
    public string CircleHint => DemoStrings.CircleSectionHint;
    public string SlideHint => DemoStrings.SlideSectionHint;
    public string SlideTrack => DemoStrings.SlideTrack;
    public Command SlideDoneCommand { get; } = new(() => { });
}

public class TreeGalleryViewModel : ObservableObject
{
    public string PageTitle => DemoStrings.NavTree;
    public string PageLead => DemoStrings.TreeLead;
    public string SectionTitle => DemoStrings.TreeSampleTitle;
    public string SectionHint => DemoStrings.TreeSampleHint;
    public string SelectedLabel => DemoStrings.TreeSelectedLabel;

    public ObservableCollection<TreeNodeModel> Items { get; } = SampleData.CreateTreeItems();

    public ObservableCollection<TreeNodeModel> Selected { get; } =
    [
        new() { Id = 1, Title = DemoStrings.TreeL11, ParentId = null },
        new() { Id = 8, Title = DemoStrings.TreeL41, ParentId = 6 }
    ];
}

public class DialogsGalleryViewModel : ObservableObject
{
    private readonly IDialogService _dialogs;
    private string _lastResult;

    public DialogsGalleryViewModel(IDialogService dialogs)
    {
        _dialogs = dialogs;
        _lastResult = DemoStrings.DialogDash;

        AlertCommand = new Command(async () =>
        {
            await _dialogs.AlertAsync(DemoStrings.AlertMessage, DemoStrings.AlertTitle, MessageIcon.INFORMATION);
            LastResult = DemoStrings.AlertClosed;
        });

        ConfirmCommand = new Command(async () =>
        {
            var ok = await _dialogs.ConfirmAsync(new ConfirmConfig
            {
                Title = DemoStrings.ConfirmTitle,
                Message = DemoStrings.ConfirmMessage,
                AcceptText = DemoStrings.ConfirmYes,
                CancelText = DemoStrings.ConfirmNo,
                Icon = MessageIcon.QUESTION
            });
            LastResult = ok ? DemoStrings.ConfirmResultYes : DemoStrings.ConfirmResultNo;
        });

        PromptCommand = new Command(async () =>
        {
            var result = await _dialogs.PromptAsync(new PromptConfig
            {
                Title = DemoStrings.PromptTitle,
                Message = DemoStrings.PromptMessage,
                Placeholder = DemoStrings.PromptPlaceholder,
                Icon = MessageIcon.QUESTION
            });
            LastResult = result.IsOk
                ? string.Format(DemoStrings.PromptResultFormat, result.Value)
                : DemoStrings.PromptCancelled;
        });

        ToastCommand = new Command(() =>
        {
            _dialogs.Toast(new ToastConfig { Message = DemoStrings.ToastMessage });
            LastResult = DemoStrings.ToastShown;
        });

        SnackbarCommand = new Command(() =>
        {
            _dialogs.Snackbar(new SnackbarConfig
            {
                Message = DemoStrings.SnackbarMessage,
                Duration = TimeSpan.FromSeconds(8)
            });
            LastResult = DemoStrings.SnackbarShown;
        });

        CustomCommand = new Command(async () =>
        {
            await _dialogs.CustomDialogAsync(new CustomDialogConfig
            {
                Title = DemoStrings.CustomDialogTitle,
                Message = DemoStrings.CustomDialogMessage,
                Icon = MessageIcon.QUESTION,
                Cancelable = true,
                Content = new VerticalStackLayout
                {
                    Spacing = 8,
                    Children =
                    {
                        new EntryView { PlaceHolder = DemoStrings.CustomNamePlaceholder },
                        new MauiPersianToolkit.Controls.DatePicker { PlaceHolder = DemoStrings.CustomDatePlaceholder }
                    }
                }
            });
            LastResult = DemoStrings.CustomDialogClosed;
        });
    }

    public string PageTitle => DemoStrings.NavDialogs;
    public string PageLead => DemoStrings.DialogsLead;
    public string ResultTitle => DemoStrings.DialogResultTitle;
    public string ResultHint => DemoStrings.DialogResultHint;
    public string ButtonsTitle => DemoStrings.DialogButtonsTitle;
    public string ButtonsHint => DemoStrings.DialogButtonsHint;
    public string ToastTitle => DemoStrings.DialogToastTitle;
    public string ToastHint => DemoStrings.DialogToastHint;
    public string CustomTitle => DemoStrings.DialogCustomTitle;
    public string CustomHint => DemoStrings.DialogCustomHint;
    public string CustomButton => DemoStrings.DialogCustomButton;

    public string LastResult { get => _lastResult; set => SetProperty(ref _lastResult, value); }
    public Command AlertCommand { get; }
    public Command ConfirmCommand { get; }
    public Command PromptCommand { get; }
    public Command ToastCommand { get; }
    public Command SnackbarCommand { get; }
    public Command CustomCommand { get; }
}
