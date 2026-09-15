using PersianUISamples.Services;

namespace PersianUISamples.Localization;

/// <summary>
/// Gallery chrome + page copy for fa / en / ar. Toolkit chrome uses PersianToolkitStrings.
/// </summary>
public static class DemoStrings
{
    public static string Get(string id, string? language = null)
    {
        language ??= ServiceHelper.TryGetService<DemoSettingsService>()?.Language ?? "fa";
        if (Catalogs.TryGetValue(language, out var catalog) && catalog.TryGetValue(id, out var value))
            return value;
        if (Catalogs["fa"].TryGetValue(id, out value))
            return value;
        return id;
    }

    public static string ThemeLabel(DemoThemeMode mode) => mode switch
    {
        DemoThemeMode.Light => ThemeLight,
        DemoThemeMode.Dark => ThemeDark,
        _ => ThemeSystem
    };

    // Shell / home / settings
    public static string Home => Get(Ids.Home);
    public static string Settings => Get(Ids.Settings);
    public static string GalleryTitle => Get(Ids.GalleryTitle);
    public static string FlyoutSubtitle => Get(Ids.FlyoutSubtitle);
    public static string Categories => Get(Ids.Categories);
    public static string CategoriesLead => Get(Ids.CategoriesLead);
    public static string Intro => Get(Ids.Intro);
    public static string ControlCountFormat => Get(Ids.ControlCountFormat);
    public static string HowToTitle => Get(Ids.HowToTitle);
    public static string HowToHint => Get(Ids.HowToHint);
    public static string HowTo1 => Get(Ids.HowTo1);
    public static string HowTo2 => Get(Ids.HowTo2);
    public static string HowTo3 => Get(Ids.HowTo3);
    public static string ThemeTitle => Get(Ids.ThemeTitle);
    public static string ThemeHint => Get(Ids.ThemeHint);
    public static string LanguageTitle => Get(Ids.LanguageTitle);
    public static string LanguageHint => Get(Ids.LanguageHint);
    public static string ThemeSystem => Get(Ids.ThemeSystem);
    public static string ThemeLight => Get(Ids.ThemeLight);
    public static string ThemeDark => Get(Ids.ThemeDark);
    public static string SettingsLead => Get(Ids.SettingsLead);
    public static string ToolkitChromeNote => Get(Ids.ToolkitChromeNote);

    public static string NavInputs => Get(Ids.NavInputs);
    public static string NavAutoComplete => Get(Ids.NavAutoComplete);
    public static string NavPickers => Get(Ids.NavPickers);
    public static string NavDateTime => Get(Ids.NavDateTime);
    public static string NavLayout => Get(Ids.NavLayout);
    public static string NavTree => Get(Ids.NavTree);
    public static string NavDialogs => Get(Ids.NavDialogs);
    public static string CatInputsSub => Get(Ids.CatInputsSub);
    public static string CatAutoCompleteSub => Get(Ids.CatAutoCompleteSub);
    public static string CatPickersSub => Get(Ids.CatPickersSub);
    public static string CatDateTimeSub => Get(Ids.CatDateTimeSub);
    public static string CatLayoutSub => Get(Ids.CatLayoutSub);
    public static string CatTreeSub => Get(Ids.CatTreeSub);
    public static string CatDialogsSub => Get(Ids.CatDialogsSub);

    // Page leads
    public static string InputsLead => Get(Ids.InputsLead);
    public static string AutoCompleteLead => Get(Ids.AutoCompleteLead);
    public static string PickersLead => Get(Ids.PickersLead);
    public static string DateTimeLead => Get(Ids.DateTimeLead);
    public static string LayoutLead => Get(Ids.LayoutLead);
    public static string TreeLead => Get(Ids.TreeLead);
    public static string DialogsLead => Get(Ids.DialogsLead);

    // Inputs
    public static string EntrySectionTitle => Get(Ids.EntrySectionTitle);
    public static string EntrySectionSub => Get(Ids.EntrySectionSub);
    public static string EntrySectionHint => Get(Ids.EntrySectionHint);
    public static string EntryPlaceholder => Get(Ids.EntryPlaceholder);
    public static string EditorSectionTitle => Get(Ids.EditorSectionTitle);
    public static string EditorSectionSub => Get(Ids.EditorSectionSub);
    public static string EditorSectionHint => Get(Ids.EditorSectionHint);
    public static string EditorTitle => Get(Ids.EditorTitle);
    public static string EditorPlaceholder => Get(Ids.EditorPlaceholder);
    public static string LabelSectionTitle => Get(Ids.LabelSectionTitle);
    public static string LabelSectionHint => Get(Ids.LabelSectionHint);
    public static string LabelSample => Get(Ids.LabelSample);
    public static string CheckSectionTitle => Get(Ids.CheckSectionTitle);
    public static string CheckSectionHint => Get(Ids.CheckSectionHint);
    public static string CheckAgree => Get(Ids.CheckAgree);
    public static string ButtonSectionTitle => Get(Ids.ButtonSectionTitle);
    public static string ButtonSectionHint => Get(Ids.ButtonSectionHint);
    public static string ButtonSave => Get(Ids.ButtonSave);
    public static string ToggleSectionTitle => Get(Ids.ToggleSectionTitle);
    public static string ToggleSectionHint => Get(Ids.ToggleSectionHint);
    public static string ToggleStatusFormat => Get(Ids.ToggleStatusFormat);

    // AutoComplete
    public static string CitySuggestTitle => Get(Ids.CitySuggestTitle);
    public static string CitySuggestSub => Get(Ids.CitySuggestSub);
    public static string CitySuggestHint => Get(Ids.CitySuggestHint);
    public static string CityPlaceholder => Get(Ids.CityPlaceholder);
    public static string SelectedFormat => Get(Ids.SelectedFormat);
    public static string SelectedNone => Get(Ids.SelectedNone);
    public static string TextFormat => Get(Ids.TextFormat);

    // Pickers
    public static string PickerSingleTitle => Get(Ids.PickerSingleTitle);
    public static string PickerSingleHint => Get(Ids.PickerSingleHint);
    public static string PickerSinglePlaceholder => Get(Ids.PickerSinglePlaceholder);
    public static string PickerSingleDialogTitle => Get(Ids.PickerSingleDialogTitle);
    public static string PickerMultiTitle => Get(Ids.PickerMultiTitle);
    public static string PickerMultiHint => Get(Ids.PickerMultiHint);
    public static string PickerMultiPlaceholder => Get(Ids.PickerMultiPlaceholder);
    public static string PickerMultiDialogTitle => Get(Ids.PickerMultiDialogTitle);

    // DateTime
    public static string DateHijriTitle => Get(Ids.DateHijriTitle);
    public static string DateHijriHint => Get(Ids.DateHijriHint);
    public static string DateHijriPlaceholder => Get(Ids.DateHijriPlaceholder);
    public static string DateRangeTitle => Get(Ids.DateRangeTitle);
    public static string DateRangeHint => Get(Ids.DateRangeHint);
    public static string DateRangePlaceholder => Get(Ids.DateRangePlaceholder);
    public static string DateMultiTitle => Get(Ids.DateMultiTitle);
    public static string DateMultiHint => Get(Ids.DateMultiHint);
    public static string DateMultiPlaceholder => Get(Ids.DateMultiPlaceholder);
    public static string Time24Title => Get(Ids.Time24Title);
    public static string Time24Hint => Get(Ids.Time24Hint);
    public static string Time24Placeholder => Get(Ids.Time24Placeholder);
    public static string Time12Title => Get(Ids.Time12Title);
    public static string Time12Hint => Get(Ids.Time12Hint);
    public static string Time12Placeholder => Get(Ids.Time12Placeholder);
    public static string DateViewTitle => Get(Ids.DateViewTitle);
    public static string DateViewHint => Get(Ids.DateViewHint);

    // Layout
    public static string TabSectionHint => Get(Ids.TabSectionHint);
    public static string Tab1 => Get(Ids.Tab1);
    public static string Tab2 => Get(Ids.Tab2);
    public static string Tab3 => Get(Ids.Tab3);
    public static string Tab1Content => Get(Ids.Tab1Content);
    public static string Tab2Content => Get(Ids.Tab2Content);
    public static string Tab3Content => Get(Ids.Tab3Content);
    public static string ContainerSectionHint => Get(Ids.ContainerSectionHint);
    public static string ContainerTitle => Get(Ids.ContainerTitle);
    public static string ContainerLeft => Get(Ids.ContainerLeft);
    public static string ContainerBody => Get(Ids.ContainerBody);
    public static string ExpanderSectionHint => Get(Ids.ExpanderSectionHint);
    public static string ExpanderHeader => Get(Ids.ExpanderHeader);
    public static string ExpanderBody => Get(Ids.ExpanderBody);
    public static string CircleSectionHint => Get(Ids.CircleSectionHint);
    public static string SlideSectionHint => Get(Ids.SlideSectionHint);
    public static string SlideTrack => Get(Ids.SlideTrack);

    // Tree
    public static string TreeSampleTitle => Get(Ids.TreeSampleTitle);
    public static string TreeSampleHint => Get(Ids.TreeSampleHint);
    public static string TreeSelectedLabel => Get(Ids.TreeSelectedLabel);

    // Dialogs
    public static string DialogResultTitle => Get(Ids.DialogResultTitle);
    public static string DialogResultHint => Get(Ids.DialogResultHint);
    public static string DialogButtonsTitle => Get(Ids.DialogButtonsTitle);
    public static string DialogButtonsHint => Get(Ids.DialogButtonsHint);
    public static string DialogToastTitle => Get(Ids.DialogToastTitle);
    public static string DialogToastHint => Get(Ids.DialogToastHint);
    public static string DialogCustomTitle => Get(Ids.DialogCustomTitle);
    public static string DialogCustomHint => Get(Ids.DialogCustomHint);
    public static string DialogCustomButton => Get(Ids.DialogCustomButton);
    public static string DialogDash => Get(Ids.DialogDash);
    public static string AlertTitle => Get(Ids.AlertTitle);
    public static string AlertMessage => Get(Ids.AlertMessage);
    public static string AlertClosed => Get(Ids.AlertClosed);
    public static string ConfirmTitle => Get(Ids.ConfirmTitle);
    public static string ConfirmMessage => Get(Ids.ConfirmMessage);
    public static string ConfirmYes => Get(Ids.ConfirmYes);
    public static string ConfirmNo => Get(Ids.ConfirmNo);
    public static string ConfirmResultYes => Get(Ids.ConfirmResultYes);
    public static string ConfirmResultNo => Get(Ids.ConfirmResultNo);
    public static string PromptTitle => Get(Ids.PromptTitle);
    public static string PromptMessage => Get(Ids.PromptMessage);
    public static string PromptPlaceholder => Get(Ids.PromptPlaceholder);
    public static string PromptResultFormat => Get(Ids.PromptResultFormat);
    public static string PromptCancelled => Get(Ids.PromptCancelled);
    public static string ToastMessage => Get(Ids.ToastMessage);
    public static string ToastShown => Get(Ids.ToastShown);
    public static string SnackbarMessage => Get(Ids.SnackbarMessage);
    public static string SnackbarShown => Get(Ids.SnackbarShown);
    public static string CustomDialogTitle => Get(Ids.CustomDialogTitle);
    public static string CustomDialogMessage => Get(Ids.CustomDialogMessage);
    public static string CustomDialogClosed => Get(Ids.CustomDialogClosed);
    public static string CustomNamePlaceholder => Get(Ids.CustomNamePlaceholder);
    public static string CustomDatePlaceholder => Get(Ids.CustomDatePlaceholder);

    // Sample data
    public static string CityTehran => Get(Ids.CityTehran);
    public static string CityMashhad => Get(Ids.CityMashhad);
    public static string CityIsfahan => Get(Ids.CityIsfahan);
    public static string CityShiraz => Get(Ids.CityShiraz);
    public static string CityTabriz => Get(Ids.CityTabriz);
    public static string CityAhvaz => Get(Ids.CityAhvaz);
    public static string CityKaraj => Get(Ids.CityKaraj);
    public static string CityQom => Get(Ids.CityQom);
    public static string CityKerman => Get(Ids.CityKerman);
    public static string CityRasht => Get(Ids.CityRasht);
    public static string CityYazd => Get(Ids.CityYazd);
    public static string CityHamedan => Get(Ids.CityHamedan);
    public static string Option1 => Get(Ids.Option1);
    public static string Option2 => Get(Ids.Option2);
    public static string Option3 => Get(Ids.Option3);
    public static string Option4 => Get(Ids.Option4);
    public static string TreeL11 => Get(Ids.TreeL11);
    public static string TreeL21 => Get(Ids.TreeL21);
    public static string TreeL22 => Get(Ids.TreeL22);
    public static string TreeL12 => Get(Ids.TreeL12);
    public static string TreeL21b => Get(Ids.TreeL21b);
    public static string TreeL31 => Get(Ids.TreeL31);
    public static string TreeL32 => Get(Ids.TreeL32);
    public static string TreeL41 => Get(Ids.TreeL41);

    public static class Ids
    {
        public const string Home = "Home";
        public const string Settings = "Settings";
        public const string GalleryTitle = "GalleryTitle";
        public const string FlyoutSubtitle = "FlyoutSubtitle";
        public const string Categories = "Categories";
        public const string CategoriesLead = "CategoriesLead";
        public const string Intro = "Intro";
        public const string ControlCountFormat = "ControlCountFormat";
        public const string HowToTitle = "HowToTitle";
        public const string HowToHint = "HowToHint";
        public const string HowTo1 = "HowTo1";
        public const string HowTo2 = "HowTo2";
        public const string HowTo3 = "HowTo3";
        public const string ThemeTitle = "ThemeTitle";
        public const string ThemeHint = "ThemeHint";
        public const string LanguageTitle = "LanguageTitle";
        public const string LanguageHint = "LanguageHint";
        public const string ThemeSystem = "ThemeSystem";
        public const string ThemeLight = "ThemeLight";
        public const string ThemeDark = "ThemeDark";
        public const string SettingsLead = "SettingsLead";
        public const string ToolkitChromeNote = "ToolkitChromeNote";
        public const string NavInputs = "NavInputs";
        public const string NavAutoComplete = "NavAutoComplete";
        public const string NavPickers = "NavPickers";
        public const string NavDateTime = "NavDateTime";
        public const string NavLayout = "NavLayout";
        public const string NavTree = "NavTree";
        public const string NavDialogs = "NavDialogs";
        public const string CatInputsSub = "CatInputsSub";
        public const string CatAutoCompleteSub = "CatAutoCompleteSub";
        public const string CatPickersSub = "CatPickersSub";
        public const string CatDateTimeSub = "CatDateTimeSub";
        public const string CatLayoutSub = "CatLayoutSub";
        public const string CatTreeSub = "CatTreeSub";
        public const string CatDialogsSub = "CatDialogsSub";

        public const string InputsLead = "InputsLead";
        public const string AutoCompleteLead = "AutoCompleteLead";
        public const string PickersLead = "PickersLead";
        public const string DateTimeLead = "DateTimeLead";
        public const string LayoutLead = "LayoutLead";
        public const string TreeLead = "TreeLead";
        public const string DialogsLead = "DialogsLead";

        public const string EntrySectionTitle = "EntrySectionTitle";
        public const string EntrySectionSub = "EntrySectionSub";
        public const string EntrySectionHint = "EntrySectionHint";
        public const string EntryPlaceholder = "EntryPlaceholder";
        public const string EditorSectionTitle = "EditorSectionTitle";
        public const string EditorSectionSub = "EditorSectionSub";
        public const string EditorSectionHint = "EditorSectionHint";
        public const string EditorTitle = "EditorTitle";
        public const string EditorPlaceholder = "EditorPlaceholder";
        public const string LabelSectionTitle = "LabelSectionTitle";
        public const string LabelSectionHint = "LabelSectionHint";
        public const string LabelSample = "LabelSample";
        public const string CheckSectionTitle = "CheckSectionTitle";
        public const string CheckSectionHint = "CheckSectionHint";
        public const string CheckAgree = "CheckAgree";
        public const string ButtonSectionTitle = "ButtonSectionTitle";
        public const string ButtonSectionHint = "ButtonSectionHint";
        public const string ButtonSave = "ButtonSave";
        public const string ToggleSectionTitle = "ToggleSectionTitle";
        public const string ToggleSectionHint = "ToggleSectionHint";
        public const string ToggleStatusFormat = "ToggleStatusFormat";

        public const string CitySuggestTitle = "CitySuggestTitle";
        public const string CitySuggestSub = "CitySuggestSub";
        public const string CitySuggestHint = "CitySuggestHint";
        public const string CityPlaceholder = "CityPlaceholder";
        public const string SelectedFormat = "SelectedFormat";
        public const string SelectedNone = "SelectedNone";
        public const string TextFormat = "TextFormat";

        public const string PickerSingleTitle = "PickerSingleTitle";
        public const string PickerSingleHint = "PickerSingleHint";
        public const string PickerSinglePlaceholder = "PickerSinglePlaceholder";
        public const string PickerSingleDialogTitle = "PickerSingleDialogTitle";
        public const string PickerMultiTitle = "PickerMultiTitle";
        public const string PickerMultiHint = "PickerMultiHint";
        public const string PickerMultiPlaceholder = "PickerMultiPlaceholder";
        public const string PickerMultiDialogTitle = "PickerMultiDialogTitle";

        public const string DateHijriTitle = "DateHijriTitle";
        public const string DateHijriHint = "DateHijriHint";
        public const string DateHijriPlaceholder = "DateHijriPlaceholder";
        public const string DateRangeTitle = "DateRangeTitle";
        public const string DateRangeHint = "DateRangeHint";
        public const string DateRangePlaceholder = "DateRangePlaceholder";
        public const string DateMultiTitle = "DateMultiTitle";
        public const string DateMultiHint = "DateMultiHint";
        public const string DateMultiPlaceholder = "DateMultiPlaceholder";
        public const string Time24Title = "Time24Title";
        public const string Time24Hint = "Time24Hint";
        public const string Time24Placeholder = "Time24Placeholder";
        public const string Time12Title = "Time12Title";
        public const string Time12Hint = "Time12Hint";
        public const string Time12Placeholder = "Time12Placeholder";
        public const string DateViewTitle = "DateViewTitle";
        public const string DateViewHint = "DateViewHint";

        public const string TabSectionHint = "TabSectionHint";
        public const string Tab1 = "Tab1";
        public const string Tab2 = "Tab2";
        public const string Tab3 = "Tab3";
        public const string Tab1Content = "Tab1Content";
        public const string Tab2Content = "Tab2Content";
        public const string Tab3Content = "Tab3Content";
        public const string ContainerSectionHint = "ContainerSectionHint";
        public const string ContainerTitle = "ContainerTitle";
        public const string ContainerLeft = "ContainerLeft";
        public const string ContainerBody = "ContainerBody";
        public const string ExpanderSectionHint = "ExpanderSectionHint";
        public const string ExpanderHeader = "ExpanderHeader";
        public const string ExpanderBody = "ExpanderBody";
        public const string CircleSectionHint = "CircleSectionHint";
        public const string SlideSectionHint = "SlideSectionHint";
        public const string SlideTrack = "SlideTrack";

        public const string TreeSampleTitle = "TreeSampleTitle";
        public const string TreeSampleHint = "TreeSampleHint";
        public const string TreeSelectedLabel = "TreeSelectedLabel";

        public const string DialogResultTitle = "DialogResultTitle";
        public const string DialogResultHint = "DialogResultHint";
        public const string DialogButtonsTitle = "DialogButtonsTitle";
        public const string DialogButtonsHint = "DialogButtonsHint";
        public const string DialogToastTitle = "DialogToastTitle";
        public const string DialogToastHint = "DialogToastHint";
        public const string DialogCustomTitle = "DialogCustomTitle";
        public const string DialogCustomHint = "DialogCustomHint";
        public const string DialogCustomButton = "DialogCustomButton";
        public const string DialogDash = "DialogDash";
        public const string AlertTitle = "AlertTitle";
        public const string AlertMessage = "AlertMessage";
        public const string AlertClosed = "AlertClosed";
        public const string ConfirmTitle = "ConfirmTitle";
        public const string ConfirmMessage = "ConfirmMessage";
        public const string ConfirmYes = "ConfirmYes";
        public const string ConfirmNo = "ConfirmNo";
        public const string ConfirmResultYes = "ConfirmResultYes";
        public const string ConfirmResultNo = "ConfirmResultNo";
        public const string PromptTitle = "PromptTitle";
        public const string PromptMessage = "PromptMessage";
        public const string PromptPlaceholder = "PromptPlaceholder";
        public const string PromptResultFormat = "PromptResultFormat";
        public const string PromptCancelled = "PromptCancelled";
        public const string ToastMessage = "ToastMessage";
        public const string ToastShown = "ToastShown";
        public const string SnackbarMessage = "SnackbarMessage";
        public const string SnackbarShown = "SnackbarShown";
        public const string CustomDialogTitle = "CustomDialogTitle";
        public const string CustomDialogMessage = "CustomDialogMessage";
        public const string CustomDialogClosed = "CustomDialogClosed";
        public const string CustomNamePlaceholder = "CustomNamePlaceholder";
        public const string CustomDatePlaceholder = "CustomDatePlaceholder";

        public const string CityTehran = "CityTehran";
        public const string CityMashhad = "CityMashhad";
        public const string CityIsfahan = "CityIsfahan";
        public const string CityShiraz = "CityShiraz";
        public const string CityTabriz = "CityTabriz";
        public const string CityAhvaz = "CityAhvaz";
        public const string CityKaraj = "CityKaraj";
        public const string CityQom = "CityQom";
        public const string CityKerman = "CityKerman";
        public const string CityRasht = "CityRasht";
        public const string CityYazd = "CityYazd";
        public const string CityHamedan = "CityHamedan";
        public const string Option1 = "Option1";
        public const string Option2 = "Option2";
        public const string Option3 = "Option3";
        public const string Option4 = "Option4";
        public const string TreeL11 = "TreeL11";
        public const string TreeL21 = "TreeL21";
        public const string TreeL22 = "TreeL22";
        public const string TreeL12 = "TreeL12";
        public const string TreeL21b = "TreeL21b";
        public const string TreeL31 = "TreeL31";
        public const string TreeL32 = "TreeL32";
        public const string TreeL41 = "TreeL41";
    }

    private static Dictionary<string, string> Fa() => new(StringComparer.OrdinalIgnoreCase)
    {
        [Ids.Home] = "خانه", [Ids.Settings] = "تنظیمات", [Ids.GalleryTitle] = "گالری پکیج",
        [Ids.FlyoutSubtitle] = "گالری کنترل‌ها", [Ids.Categories] = "دسته‌بندی‌ها",
        [Ids.CategoriesLead] = "از منوی کناری هم می‌توانید مستقیم به هر دسته بروید.",
        [Ids.Intro] = "گالری رسمی کنترل‌های پکیج. از منوی کناری دسته را انتخاب کنید و در XAML هر صفحه، کانفیگ واقعی همان کنترل را ببینید.",
        [Ids.ControlCountFormat] = "حدود {0} کنترل و سرویس در دمو",
        [Ids.HowToTitle] = "چطور از این اپ استفاده کنم؟",
        [Ids.HowToHint] = "هر صفحه XAML همان کانفیگ‌هایی را نشان می‌دهد که در اپ واقعی می‌نویسید.",
        [Ids.HowTo1] = "۱) کنترل را در گالری امتحان کنید", [Ids.HowTo2] = "۲) Hint زیر هر بخش را بخوانید",
        [Ids.HowTo3] = "۳) همان BindablePropertyها را در پروژه خود کپی کنید",
        [Ids.ThemeTitle] = "تم روشن / تاریک",
        [Ids.ThemeHint] = "Application.Current.UserAppTheme — کنترل‌ها با AppThemeBinding و توکن‌های Pdt* واکنش نشان می‌دهند.",
        [Ids.LanguageTitle] = "زبان رابط",
        [Ids.LanguageHint] = "PersianToolkitStrings.SetCulture + CultureInfo — رشته‌های کروم پکیج (Accept، Today، …) عوض می‌شوند.",
        [Ids.ThemeSystem] = "سیستم", [Ids.ThemeLight] = "روشن", [Ids.ThemeDark] = "تاریک",
        [Ids.SettingsLead] = "تم و زبان را عوض کنید تا دموی کنترل‌ها را در هر دو حالت ببینید.",
        [Ids.ToolkitChromeNote] = "دکمه‌های Accept/Cancel تقویم و دیالوگ از کاتالوگ fa/en/ar پکیج می‌آیند.",
        [Ids.NavInputs] = "ورودی‌ها", [Ids.NavAutoComplete] = "تکمیل خودکار", [Ids.NavPickers] = "انتخاب‌گرها",
        [Ids.NavDateTime] = "تاریخ و زمان", [Ids.NavLayout] = "چیدمان و ناوبری", [Ids.NavTree] = "درختواره",
        [Ids.NavDialogs] = "دیالوگ‌ها",
        [Ids.CatInputsSub] = "Entry، Editor، Label، CheckBox، Button، Toggle",
        [Ids.CatAutoCompleteSub] = "AutoCompleteView و Options",
        [Ids.CatPickersSub] = "Picker تک‌انتخابی و چندتایی",
        [Ids.CatDateTimeSub] = "DatePicker شمسی/قمری/میلادی و TimePicker",
        [Ids.CatLayoutSub] = "TabView، Container، Expander، SlideButton",
        [Ids.CatTreeSub] = "TreeView با انتخاب چندتایی",
        [Ids.CatDialogsSub] = "Alert، Confirm، Prompt، Toast، Snackbar",

        [Ids.InputsLead] = "EntryView، EditorView، LabelView، CheckBoxView، ButtonView و ToggleButton",
        [Ids.AutoCompleteLead] = "جستجو و پیشنهاد از روی ItemsSource با AutoCompleteOptions",
        [Ids.PickersLead] = "انتخاب تک‌تایی و چندتایی با تمپلیت سفارشی و دکمه‌های اضافی",
        [Ids.DateTimeLead] = "DatePicker با CalendarOptions و TimePicker با حالت ۲۴/۱۲ ساعته",
        [Ids.LayoutLead] = "TabView، ContainerView، Expander، SlideButton و CircleImageView",
        [Ids.TreeLead] = "سلسله‌مراتب با KeyProperty / ParentChildProperty و انتخاب چندتایی",
        [Ids.DialogsLead] = "Alert، Confirm، Prompt، Toast، Snackbar و CustomDialog — همزمان و Async",

        [Ids.EntrySectionTitle] = "EntryView", [Ids.EntrySectionSub] = "فیلد تک‌خطی با آیکن و AppendText",
        [Ids.EntrySectionHint] = "PlaceHolder · Icon · AppendText · Text · از PersianInputBase",
        [Ids.EntryPlaceholder] = "نام و فامیل",
        [Ids.EditorSectionTitle] = "EditorView", [Ids.EditorSectionSub] = "متن چندخطی با عنوان",
        [Ids.EditorSectionHint] = "Title · PlaceHolder · Icon · Text",
        [Ids.EditorTitle] = "شرح کامل", [Ids.EditorPlaceholder] = "شرح کامل بنویس",
        [Ids.LabelSectionTitle] = "LabelView", [Ids.LabelSectionHint] = "Text · Icon — برچسب با آیکن FontAwesome",
        [Ids.LabelSample] = "برچسب نمونه",
        [Ids.CheckSectionTitle] = "CheckBoxView", [Ids.CheckSectionHint] = "Text · IsChecked · TextColor",
        [Ids.CheckAgree] = "قوانین را می‌پذیرم",
        [Ids.ButtonSectionTitle] = "ButtonView", [Ids.ButtonSectionHint] = "Text · Icon · Command · IsBusy · BtnBgColor · CornerRadius",
        [Ids.ButtonSave] = "ذخیره",
        [Ids.ToggleSectionTitle] = "ToggleButton", [Ids.ToggleSectionHint] = "Checked · Animate · Command",
        [Ids.ToggleStatusFormat] = "وضعیت: {0}",

        [Ids.CitySuggestTitle] = "پیشنهاد شهر",
        [Ids.CitySuggestSub] = "Contains · حداقل ۱ کاراکتر · حداکثر ۸ پیشنهاد · native",
        [Ids.CitySuggestHint] = "ItemsSource · DisplayProperty · Text · SelectedItem · Options",
        [Ids.CityPlaceholder] = "شهر",
        [Ids.SelectedFormat] = "انتخاب‌شده: {0}", [Ids.SelectedNone] = "انتخاب‌شده: —",
        [Ids.TextFormat] = "متن: {0}",

        [Ids.PickerSingleTitle] = "تک‌انتخابی",
        [Ids.PickerSingleHint] = "ItemsSource · DisplayProperty · ValueMember · SelectionMode=Single",
        [Ids.PickerSinglePlaceholder] = "انتخاب تکی", [Ids.PickerSingleDialogTitle] = "یکی را انتخاب کن",
        [Ids.PickerMultiTitle] = "چندانتخابی + ItemTemplate",
        [Ids.PickerMultiHint] = "SelectionMode=Multiple · ItemTemplate · SelectedItemColor",
        [Ids.PickerMultiPlaceholder] = "انتخاب چندتایی", [Ids.PickerMultiDialogTitle] = "چندتا را انتخاب کن",

        [Ids.DateHijriTitle] = "DatePicker — قمری (تک)",
        [Ids.DateHijriHint] = "CalendarType=Hijri · SelectionMode=Single · Min/MaxDateCanSelect",
        [Ids.DateHijriPlaceholder] = "تاریخ (قمری)",
        [Ids.DateRangeTitle] = "DatePicker — شمسی (بازه)",
        [Ids.DateRangeHint] = "SelectionMode=Range · InactiveDays · BadgeDates · OnAccept",
        [Ids.DateRangePlaceholder] = "بازه تاریخی (شمسی)",
        [Ids.DateMultiTitle] = "DatePicker — میلادی (چندتایی)",
        [Ids.DateMultiHint] = "CalendarType=Gregorian · SelectionMode=Multiple",
        [Ids.DateMultiPlaceholder] = "تاریخ‌ها (میلادی)",
        [Ids.Time24Title] = "TimePicker — ۲۴ ساعته",
        [Ids.Time24Hint] = "SelectedTime · Is24Hour · MinuteInterval · DisplayFormat",
        [Ids.Time24Placeholder] = "ساعت جلسه",
        [Ids.Time12Title] = "TimePicker — ۱۲ ساعته + ثانیه",
        [Ids.Time12Hint] = "Is24Hour=False · ShowSeconds · DisplayFormat",
        [Ids.Time12Placeholder] = "زمان با ثانیه",
        [Ids.DateViewTitle] = "DatePickerView (تقویم توکار)",
        [Ids.DateViewHint] = "بدون پاپ‌آپ؛ مستقیم CalendarOption را ببندید",

        [Ids.TabSectionHint] = "SelectedTabColor · IndicatorColor · AnimateCaptions · EnableAnimations",
        [Ids.Tab1] = "اول", [Ids.Tab2] = "دوم", [Ids.Tab3] = "سوم",
        [Ids.Tab1Content] = "محتوای تب اول", [Ids.Tab2Content] = "محتوای تب دوم", [Ids.Tab3Content] = "محتوای تب سوم",
        [Ids.ContainerSectionHint] = "Title · LeftTitle · Contents",
        [Ids.ContainerTitle] = "عنوان بخش", [Ids.ContainerLeft] = "جزئیات",
        [Ids.ContainerBody] = "محتوای داخل ContainerView",
        [Ids.ExpanderSectionHint] = "IsExpanded · Header · محتوا به صورت child",
        [Ids.ExpanderHeader] = "نمونه Expander — ضربه بزنید",
        [Ids.ExpanderBody] = "محتوای بازشونده Expander",
        [Ids.CircleSectionHint] = "ImageSource · ImageWidth/Height · BorderThickness · BorderColor",
        [Ids.SlideSectionHint] = "SlideCompletedCommand · Thumb · TrackBar · FillBar",
        [Ids.SlideTrack] = "برای ورود بکشید",

        [Ids.TreeSampleTitle] = "درخت نمونه",
        [Ids.TreeSampleHint] = "ItemsSource · KeyProperty · ParentChildProperty · SelectionMode · SelectedItems",
        [Ids.TreeSelectedLabel] = "انتخاب‌شده‌ها",

        [Ids.DialogResultTitle] = "نتیجه آخرین عمل",
        [Ids.DialogResultHint] = "Binding به LastResult برای نمایش پاسخ کاربر",
        [Ids.DialogButtonsTitle] = "Alert / Confirm / Prompt",
        [Ids.DialogButtonsHint] = "AlertAsync · ConfirmAsync · PromptAsync → PromptResult",
        [Ids.DialogToastTitle] = "Toast و Snackbar",
        [Ids.DialogToastHint] = "Toast(ToastConfig) · Snackbar(SnackbarConfig)",
        [Ids.DialogCustomTitle] = "CustomDialog",
        [Ids.DialogCustomHint] = "CustomDialogAsync با Content دلخواه",
        [Ids.DialogCustomButton] = "دیالوگ سفارشی", [Ids.DialogDash] = "—",
        [Ids.AlertTitle] = "هشدار",
        [Ids.AlertMessage] = "این یک Alert از IDialogService است. مصرف‌کننده می‌تواند از Alert یا AlertAsync استفاده کند.",
        [Ids.AlertClosed] = "Alert بسته شد",
        [Ids.ConfirmTitle] = "تأیید", [Ids.ConfirmMessage] = "آیا ادامه می‌دهید؟",
        [Ids.ConfirmYes] = "بله", [Ids.ConfirmNo] = "خیر",
        [Ids.ConfirmResultYes] = "Confirm: بله", [Ids.ConfirmResultNo] = "Confirm: خیر",
        [Ids.PromptTitle] = "ورودی", [Ids.PromptMessage] = "نام را وارد کنید",
        [Ids.PromptPlaceholder] = "نام", [Ids.PromptResultFormat] = "Prompt: {0}",
        [Ids.PromptCancelled] = "Prompt لغو شد",
        [Ids.ToastMessage] = "این یک پیام Toast است", [Ids.ToastShown] = "Toast نشان داده شد",
        [Ids.SnackbarMessage] = "این یک پیام Snackbar است", [Ids.SnackbarShown] = "Snackbar نشان داده شد",
        [Ids.CustomDialogTitle] = "دیالوگ سفارشی",
        [Ids.CustomDialogMessage] = "محتوای دلخواه داخل دیالوگ پکیج.",
        [Ids.CustomDialogClosed] = "CustomDialog بسته شد",
        [Ids.CustomNamePlaceholder] = "نام", [Ids.CustomDatePlaceholder] = "تاریخ",

        [Ids.CityTehran] = "تهران", [Ids.CityMashhad] = "مشهد", [Ids.CityIsfahan] = "اصفهان",
        [Ids.CityShiraz] = "شیراز", [Ids.CityTabriz] = "تبریز", [Ids.CityAhvaz] = "اهواز",
        [Ids.CityKaraj] = "کرج", [Ids.CityQom] = "قم", [Ids.CityKerman] = "کرمان",
        [Ids.CityRasht] = "رشت", [Ids.CityYazd] = "یزد", [Ids.CityHamedan] = "همدان",
        [Ids.Option1] = "گزینه اول", [Ids.Option2] = "گزینه دوم", [Ids.Option3] = "گزینه سوم", [Ids.Option4] = "گزینه چهارم",
        [Ids.TreeL11] = "سطح ۱-۱", [Ids.TreeL21] = "سطح ۲-۱", [Ids.TreeL22] = "سطح ۲-۲",
        [Ids.TreeL12] = "سطح ۱-۲", [Ids.TreeL21b] = "سطح ۲-۱", [Ids.TreeL31] = "سطح ۳-۱",
        [Ids.TreeL32] = "سطح ۳-۲", [Ids.TreeL41] = "سطح ۴-۱",
    };

    private static Dictionary<string, string> En() => new(StringComparer.OrdinalIgnoreCase)
    {
        [Ids.Home] = "Home", [Ids.Settings] = "Settings", [Ids.GalleryTitle] = "Package gallery",
        [Ids.FlyoutSubtitle] = "Controls gallery", [Ids.Categories] = "Categories",
        [Ids.CategoriesLead] = "You can also open any category from the flyout menu.",
        [Ids.Intro] = "Official gallery for MauiPersianToolkit. Pick a category and inspect real control configuration in each page's XAML.",
        [Ids.ControlCountFormat] = "About {0} controls and services in this demo",
        [Ids.HowToTitle] = "How to use this app",
        [Ids.HowToHint] = "Each page XAML shows the same BindableProperty configuration you would use in production.",
        [Ids.HowTo1] = "1) Try the control in the gallery", [Ids.HowTo2] = "2) Read the hint under each section",
        [Ids.HowTo3] = "3) Copy the same BindableProperties into your app",
        [Ids.ThemeTitle] = "Light / dark theme",
        [Ids.ThemeHint] = "Application.Current.UserAppTheme — controls respond via AppThemeBinding and Pdt* tokens.",
        [Ids.LanguageTitle] = "UI language",
        [Ids.LanguageHint] = "PersianToolkitStrings.SetCulture + CultureInfo — toolkit chrome strings (Accept, Today, …) update.",
        [Ids.ThemeSystem] = "System", [Ids.ThemeLight] = "Light", [Ids.ThemeDark] = "Dark",
        [Ids.SettingsLead] = "Switch theme and language to preview every control in both modes.",
        [Ids.ToolkitChromeNote] = "Calendar/dialog Accept/Cancel labels come from the package fa/en/ar catalogs.",
        [Ids.NavInputs] = "Inputs", [Ids.NavAutoComplete] = "AutoComplete", [Ids.NavPickers] = "Pickers",
        [Ids.NavDateTime] = "Date & time", [Ids.NavLayout] = "Layout & navigation", [Ids.NavTree] = "Tree",
        [Ids.NavDialogs] = "Dialogs",
        [Ids.CatInputsSub] = "Entry, Editor, Label, CheckBox, Button, Toggle",
        [Ids.CatAutoCompleteSub] = "AutoCompleteView and Options",
        [Ids.CatPickersSub] = "Single and multi PickerView",
        [Ids.CatDateTimeSub] = "Persian/Hijri/Gregorian DatePicker and TimePicker",
        [Ids.CatLayoutSub] = "TabView, Container, Expander, SlideButton",
        [Ids.CatTreeSub] = "TreeView with multi-select",
        [Ids.CatDialogsSub] = "Alert, Confirm, Prompt, Toast, Snackbar",

        [Ids.InputsLead] = "EntryView, EditorView, LabelView, CheckBoxView, ButtonView and ToggleButton",
        [Ids.AutoCompleteLead] = "Search and suggestions from ItemsSource with AutoCompleteOptions",
        [Ids.PickersLead] = "Single and multi selection with custom templates and extra buttons",
        [Ids.DateTimeLead] = "DatePicker with CalendarOptions and 24/12-hour TimePicker",
        [Ids.LayoutLead] = "TabView, ContainerView, Expander, SlideButton and CircleImageView",
        [Ids.TreeLead] = "Hierarchy with KeyProperty / ParentChildProperty and multi-select",
        [Ids.DialogsLead] = "Alert, Confirm, Prompt, Toast, Snackbar and CustomDialog — sync and async",

        [Ids.EntrySectionTitle] = "EntryView", [Ids.EntrySectionSub] = "Single-line field with icon and AppendText",
        [Ids.EntrySectionHint] = "PlaceHolder · Icon · AppendText · Text · from PersianInputBase",
        [Ids.EntryPlaceholder] = "Full name",
        [Ids.EditorSectionTitle] = "EditorView", [Ids.EditorSectionSub] = "Multi-line text with title",
        [Ids.EditorSectionHint] = "Title · PlaceHolder · Icon · Text",
        [Ids.EditorTitle] = "Description", [Ids.EditorPlaceholder] = "Write a full description",
        [Ids.LabelSectionTitle] = "LabelView", [Ids.LabelSectionHint] = "Text · Icon — label with FontAwesome icon",
        [Ids.LabelSample] = "Sample label",
        [Ids.CheckSectionTitle] = "CheckBoxView", [Ids.CheckSectionHint] = "Text · IsChecked · TextColor",
        [Ids.CheckAgree] = "I accept the terms",
        [Ids.ButtonSectionTitle] = "ButtonView", [Ids.ButtonSectionHint] = "Text · Icon · Command · IsBusy · BtnBgColor · CornerRadius",
        [Ids.ButtonSave] = "Save",
        [Ids.ToggleSectionTitle] = "ToggleButton", [Ids.ToggleSectionHint] = "Checked · Animate · Command",
        [Ids.ToggleStatusFormat] = "Status: {0}",

        [Ids.CitySuggestTitle] = "City suggestions",
        [Ids.CitySuggestSub] = "Contains · min 1 char · max 8 suggestions · native",
        [Ids.CitySuggestHint] = "ItemsSource · DisplayProperty · Text · SelectedItem · Options",
        [Ids.CityPlaceholder] = "City",
        [Ids.SelectedFormat] = "Selected: {0}", [Ids.SelectedNone] = "Selected: —",
        [Ids.TextFormat] = "Text: {0}",

        [Ids.PickerSingleTitle] = "Single select",
        [Ids.PickerSingleHint] = "ItemsSource · DisplayProperty · ValueMember · SelectionMode=Single",
        [Ids.PickerSinglePlaceholder] = "Pick one", [Ids.PickerSingleDialogTitle] = "Choose one item",
        [Ids.PickerMultiTitle] = "Multi select + ItemTemplate",
        [Ids.PickerMultiHint] = "SelectionMode=Multiple · ItemTemplate · SelectedItemColor",
        [Ids.PickerMultiPlaceholder] = "Pick several", [Ids.PickerMultiDialogTitle] = "Choose multiple items",

        [Ids.DateHijriTitle] = "DatePicker — Hijri (single)",
        [Ids.DateHijriHint] = "CalendarType=Hijri · SelectionMode=Single · Min/MaxDateCanSelect",
        [Ids.DateHijriPlaceholder] = "Date (Hijri)",
        [Ids.DateRangeTitle] = "DatePicker — Persian (range)",
        [Ids.DateRangeHint] = "SelectionMode=Range · InactiveDays · BadgeDates · OnAccept",
        [Ids.DateRangePlaceholder] = "Date range (Persian)",
        [Ids.DateMultiTitle] = "DatePicker — Gregorian (multi)",
        [Ids.DateMultiHint] = "CalendarType=Gregorian · SelectionMode=Multiple",
        [Ids.DateMultiPlaceholder] = "Dates (Gregorian)",
        [Ids.Time24Title] = "TimePicker — 24-hour",
        [Ids.Time24Hint] = "SelectedTime · Is24Hour · MinuteInterval · DisplayFormat",
        [Ids.Time24Placeholder] = "Meeting time",
        [Ids.Time12Title] = "TimePicker — 12-hour + seconds",
        [Ids.Time12Hint] = "Is24Hour=False · ShowSeconds · DisplayFormat",
        [Ids.Time12Placeholder] = "Time with seconds",
        [Ids.DateViewTitle] = "DatePickerView (inline calendar)",
        [Ids.DateViewHint] = "No popup; bind CalendarOption directly",

        [Ids.TabSectionHint] = "SelectedTabColor · IndicatorColor · AnimateCaptions · EnableAnimations",
        [Ids.Tab1] = "First", [Ids.Tab2] = "Second", [Ids.Tab3] = "Third",
        [Ids.Tab1Content] = "First tab content", [Ids.Tab2Content] = "Second tab content", [Ids.Tab3Content] = "Third tab content",
        [Ids.ContainerSectionHint] = "Title · LeftTitle · Contents",
        [Ids.ContainerTitle] = "Section title", [Ids.ContainerLeft] = "Details",
        [Ids.ContainerBody] = "Content inside ContainerView",
        [Ids.ExpanderSectionHint] = "IsExpanded · Header · content as child",
        [Ids.ExpanderHeader] = "Expander sample — tap",
        [Ids.ExpanderBody] = "Expander collapsible body",
        [Ids.CircleSectionHint] = "ImageSource · ImageWidth/Height · BorderThickness · BorderColor",
        [Ids.SlideSectionHint] = "SlideCompletedCommand · Thumb · TrackBar · FillBar",
        [Ids.SlideTrack] = "Slide to sign in",

        [Ids.TreeSampleTitle] = "Sample tree",
        [Ids.TreeSampleHint] = "ItemsSource · KeyProperty · ParentChildProperty · SelectionMode · SelectedItems",
        [Ids.TreeSelectedLabel] = "Selected",

        [Ids.DialogResultTitle] = "Last action result",
        [Ids.DialogResultHint] = "Bound to LastResult to show the user response",
        [Ids.DialogButtonsTitle] = "Alert / Confirm / Prompt",
        [Ids.DialogButtonsHint] = "AlertAsync · ConfirmAsync · PromptAsync → PromptResult",
        [Ids.DialogToastTitle] = "Toast & Snackbar",
        [Ids.DialogToastHint] = "Toast(ToastConfig) · Snackbar(SnackbarConfig)",
        [Ids.DialogCustomTitle] = "CustomDialog",
        [Ids.DialogCustomHint] = "CustomDialogAsync with custom Content",
        [Ids.DialogCustomButton] = "Custom dialog", [Ids.DialogDash] = "—",
        [Ids.AlertTitle] = "Alert",
        [Ids.AlertMessage] = "This is an Alert from IDialogService. Consumers can use Alert or AlertAsync.",
        [Ids.AlertClosed] = "Alert closed",
        [Ids.ConfirmTitle] = "Confirm", [Ids.ConfirmMessage] = "Do you want to continue?",
        [Ids.ConfirmYes] = "Yes", [Ids.ConfirmNo] = "No",
        [Ids.ConfirmResultYes] = "Confirm: Yes", [Ids.ConfirmResultNo] = "Confirm: No",
        [Ids.PromptTitle] = "Input", [Ids.PromptMessage] = "Enter a name",
        [Ids.PromptPlaceholder] = "Name", [Ids.PromptResultFormat] = "Prompt: {0}",
        [Ids.PromptCancelled] = "Prompt cancelled",
        [Ids.ToastMessage] = "This is a Toast message", [Ids.ToastShown] = "Toast shown",
        [Ids.SnackbarMessage] = "This is a Snackbar message", [Ids.SnackbarShown] = "Snackbar shown",
        [Ids.CustomDialogTitle] = "Custom dialog",
        [Ids.CustomDialogMessage] = "Custom content inside the toolkit dialog.",
        [Ids.CustomDialogClosed] = "CustomDialog closed",
        [Ids.CustomNamePlaceholder] = "Name", [Ids.CustomDatePlaceholder] = "Date",

        [Ids.CityTehran] = "Tehran", [Ids.CityMashhad] = "Mashhad", [Ids.CityIsfahan] = "Isfahan",
        [Ids.CityShiraz] = "Shiraz", [Ids.CityTabriz] = "Tabriz", [Ids.CityAhvaz] = "Ahvaz",
        [Ids.CityKaraj] = "Karaj", [Ids.CityQom] = "Qom", [Ids.CityKerman] = "Kerman",
        [Ids.CityRasht] = "Rasht", [Ids.CityYazd] = "Yazd", [Ids.CityHamedan] = "Hamedan",
        [Ids.Option1] = "Option 1", [Ids.Option2] = "Option 2", [Ids.Option3] = "Option 3", [Ids.Option4] = "Option 4",
        [Ids.TreeL11] = "Level 1-1", [Ids.TreeL21] = "Level 2-1", [Ids.TreeL22] = "Level 2-2",
        [Ids.TreeL12] = "Level 1-2", [Ids.TreeL21b] = "Level 2-1", [Ids.TreeL31] = "Level 3-1",
        [Ids.TreeL32] = "Level 3-2", [Ids.TreeL41] = "Level 4-1",
    };

    private static Dictionary<string, string> Ar() => new(StringComparer.OrdinalIgnoreCase)
    {
        [Ids.Home] = "الرئيسية", [Ids.Settings] = "الإعدادات", [Ids.GalleryTitle] = "معرض الحزمة",
        [Ids.FlyoutSubtitle] = "معرض عناصر التحكم", [Ids.Categories] = "التصنيفات",
        [Ids.CategoriesLead] = "يمكنك أيضاً فتح أي تصنيف من القائمة الجانبية.",
        [Ids.Intro] = "المعرض الرسمي لعناصر MauiPersianToolkit. اختر تصنيفاً واطلع على إعدادات التحكم الحقيقية في XAML لكل صفحة.",
        [Ids.ControlCountFormat] = "حوالي {0} عنصراً وخدمة في هذا العرض",
        [Ids.HowToTitle] = "كيف تستخدم هذا التطبيق؟",
        [Ids.HowToHint] = "يعرض XAML كل صفحة نفس إعدادات BindableProperty التي تستخدمها في تطبيقك.",
        [Ids.HowTo1] = "١) جرّب العنصر في المعرض", [Ids.HowTo2] = "٢) اقرأ التلميح أسفل كل قسم",
        [Ids.HowTo3] = "٣) انسخ نفس الخصائص إلى مشروعك",
        [Ids.ThemeTitle] = "السمة الفاتحة / الداكنة",
        [Ids.ThemeHint] = "Application.Current.UserAppTheme — تتفاعل العناصر عبر AppThemeBinding ورموز Pdt*.",
        [Ids.LanguageTitle] = "لغة الواجهة",
        [Ids.LanguageHint] = "PersianToolkitStrings.SetCulture + CultureInfo — تتحدث نصوص الحزمة (Accept، Today، …).",
        [Ids.ThemeSystem] = "النظام", [Ids.ThemeLight] = "فاتح", [Ids.ThemeDark] = "داكن",
        [Ids.SettingsLead] = "بدّل السمة واللغة لمعاينة كل العناصر في الحالتين.",
        [Ids.ToolkitChromeNote] = "تسميات Accept/Cancel للتقويم والحوارات تأتي من كتالوجات fa/en/ar في الحزمة.",
        [Ids.NavInputs] = "الإدخالات", [Ids.NavAutoComplete] = "الإكمال التلقائي", [Ids.NavPickers] = "مربعات الاختيار",
        [Ids.NavDateTime] = "التاريخ والوقت", [Ids.NavLayout] = "التخطيط والتنقل", [Ids.NavTree] = "الشجرة",
        [Ids.NavDialogs] = "الحوارات",
        [Ids.CatInputsSub] = "Entry و Editor و Label و CheckBox و Button و Toggle",
        [Ids.CatAutoCompleteSub] = "AutoCompleteView والخيارات",
        [Ids.CatPickersSub] = "Picker فردي ومتعدد",
        [Ids.CatDateTimeSub] = "DatePicker فارسي/هجري/ميلادي و TimePicker",
        [Ids.CatLayoutSub] = "TabView و Container و Expander و SlideButton",
        [Ids.CatTreeSub] = "TreeView مع اختيار متعدد",
        [Ids.CatDialogsSub] = "Alert و Confirm و Prompt و Toast و Snackbar",

        [Ids.InputsLead] = "EntryView و EditorView و LabelView و CheckBoxView و ButtonView و ToggleButton",
        [Ids.AutoCompleteLead] = "البحث والاقتراحات من ItemsSource مع AutoCompleteOptions",
        [Ids.PickersLead] = "اختيار فردي ومتعدد مع قوالب مخصصة وأزرار إضافية",
        [Ids.DateTimeLead] = "DatePicker مع CalendarOptions و TimePicker بنمط ٢٤/١٢ ساعة",
        [Ids.LayoutLead] = "TabView و ContainerView و Expander و SlideButton و CircleImageView",
        [Ids.TreeLead] = "تسلسل هرمي مع KeyProperty / ParentChildProperty واختيار متعدد",
        [Ids.DialogsLead] = "Alert و Confirm و Prompt و Toast و Snackbar و CustomDialog — متزامن وغير متزامن",

        [Ids.EntrySectionTitle] = "EntryView", [Ids.EntrySectionSub] = "حقل سطر واحد مع أيقونة و AppendText",
        [Ids.EntrySectionHint] = "PlaceHolder · Icon · AppendText · Text · من PersianInputBase",
        [Ids.EntryPlaceholder] = "الاسم الكامل",
        [Ids.EditorSectionTitle] = "EditorView", [Ids.EditorSectionSub] = "نص متعدد الأسطر مع عنوان",
        [Ids.EditorSectionHint] = "Title · PlaceHolder · Icon · Text",
        [Ids.EditorTitle] = "الوصف", [Ids.EditorPlaceholder] = "اكتب وصفاً كاملاً",
        [Ids.LabelSectionTitle] = "LabelView", [Ids.LabelSectionHint] = "Text · Icon — تسمية مع أيقونة FontAwesome",
        [Ids.LabelSample] = "تسمية نموذجية",
        [Ids.CheckSectionTitle] = "CheckBoxView", [Ids.CheckSectionHint] = "Text · IsChecked · TextColor",
        [Ids.CheckAgree] = "أوافق على الشروط",
        [Ids.ButtonSectionTitle] = "ButtonView", [Ids.ButtonSectionHint] = "Text · Icon · Command · IsBusy · BtnBgColor · CornerRadius",
        [Ids.ButtonSave] = "حفظ",
        [Ids.ToggleSectionTitle] = "ToggleButton", [Ids.ToggleSectionHint] = "Checked · Animate · Command",
        [Ids.ToggleStatusFormat] = "الحالة: {0}",

        [Ids.CitySuggestTitle] = "اقتراح المدينة",
        [Ids.CitySuggestSub] = "Contains · حرف واحد على الأقل · ٨ اقتراحات كحد أقصى · native",
        [Ids.CitySuggestHint] = "ItemsSource · DisplayProperty · Text · SelectedItem · Options",
        [Ids.CityPlaceholder] = "المدينة",
        [Ids.SelectedFormat] = "المحدد: {0}", [Ids.SelectedNone] = "المحدد: —",
        [Ids.TextFormat] = "النص: {0}",

        [Ids.PickerSingleTitle] = "اختيار فردي",
        [Ids.PickerSingleHint] = "ItemsSource · DisplayProperty · ValueMember · SelectionMode=Single",
        [Ids.PickerSinglePlaceholder] = "اختر واحداً", [Ids.PickerSingleDialogTitle] = "اختر عنصراً واحداً",
        [Ids.PickerMultiTitle] = "اختيار متعدد + ItemTemplate",
        [Ids.PickerMultiHint] = "SelectionMode=Multiple · ItemTemplate · SelectedItemColor",
        [Ids.PickerMultiPlaceholder] = "اختر عدة عناصر", [Ids.PickerMultiDialogTitle] = "اختر عدة عناصر",

        [Ids.DateHijriTitle] = "DatePicker — هجري (فردي)",
        [Ids.DateHijriHint] = "CalendarType=Hijri · SelectionMode=Single · Min/MaxDateCanSelect",
        [Ids.DateHijriPlaceholder] = "التاريخ (هجري)",
        [Ids.DateRangeTitle] = "DatePicker — فارسي (نطاق)",
        [Ids.DateRangeHint] = "SelectionMode=Range · InactiveDays · BadgeDates · OnAccept",
        [Ids.DateRangePlaceholder] = "نطاق التاريخ (فارسي)",
        [Ids.DateMultiTitle] = "DatePicker — ميلادي (متعدد)",
        [Ids.DateMultiHint] = "CalendarType=Gregorian · SelectionMode=Multiple",
        [Ids.DateMultiPlaceholder] = "التواريخ (ميلادي)",
        [Ids.Time24Title] = "TimePicker — ٢٤ ساعة",
        [Ids.Time24Hint] = "SelectedTime · Is24Hour · MinuteInterval · DisplayFormat",
        [Ids.Time24Placeholder] = "وقت الاجتماع",
        [Ids.Time12Title] = "TimePicker — ١٢ ساعة + ثوانٍ",
        [Ids.Time12Hint] = "Is24Hour=False · ShowSeconds · DisplayFormat",
        [Ids.Time12Placeholder] = "الوقت مع الثواني",
        [Ids.DateViewTitle] = "DatePickerView (تقويم مضمّن)",
        [Ids.DateViewHint] = "بدون نافذة منبثقة؛ اربط CalendarOption مباشرة",

        [Ids.TabSectionHint] = "SelectedTabColor · IndicatorColor · AnimateCaptions · EnableAnimations",
        [Ids.Tab1] = "الأول", [Ids.Tab2] = "الثاني", [Ids.Tab3] = "الثالث",
        [Ids.Tab1Content] = "محتوى التبويب الأول", [Ids.Tab2Content] = "محتوى التبويب الثاني", [Ids.Tab3Content] = "محتوى التبويب الثالث",
        [Ids.ContainerSectionHint] = "Title · LeftTitle · Contents",
        [Ids.ContainerTitle] = "عنوان القسم", [Ids.ContainerLeft] = "التفاصيل",
        [Ids.ContainerBody] = "المحتوى داخل ContainerView",
        [Ids.ExpanderSectionHint] = "IsExpanded · Header · المحتوى كـ child",
        [Ids.ExpanderHeader] = "عينة Expander — اضغط",
        [Ids.ExpanderBody] = "محتوى Expander القابل للطي",
        [Ids.CircleSectionHint] = "ImageSource · ImageWidth/Height · BorderThickness · BorderColor",
        [Ids.SlideSectionHint] = "SlideCompletedCommand · Thumb · TrackBar · FillBar",
        [Ids.SlideTrack] = "اسحب لتسجيل الدخول",

        [Ids.TreeSampleTitle] = "شجرة نموذجية",
        [Ids.TreeSampleHint] = "ItemsSource · KeyProperty · ParentChildProperty · SelectionMode · SelectedItems",
        [Ids.TreeSelectedLabel] = "المحدد",

        [Ids.DialogResultTitle] = "نتيجة آخر إجراء",
        [Ids.DialogResultHint] = "مرتبط بـ LastResult لعرض رد المستخدم",
        [Ids.DialogButtonsTitle] = "Alert / Confirm / Prompt",
        [Ids.DialogButtonsHint] = "AlertAsync · ConfirmAsync · PromptAsync → PromptResult",
        [Ids.DialogToastTitle] = "Toast و Snackbar",
        [Ids.DialogToastHint] = "Toast(ToastConfig) · Snackbar(SnackbarConfig)",
        [Ids.DialogCustomTitle] = "CustomDialog",
        [Ids.DialogCustomHint] = "CustomDialogAsync مع محتوى مخصص",
        [Ids.DialogCustomButton] = "حوار مخصص", [Ids.DialogDash] = "—",
        [Ids.AlertTitle] = "تنبيه",
        [Ids.AlertMessage] = "هذا تنبيه من IDialogService. يمكن للمستهلك استخدام Alert أو AlertAsync.",
        [Ids.AlertClosed] = "أُغلق Alert",
        [Ids.ConfirmTitle] = "تأكيد", [Ids.ConfirmMessage] = "هل تريد المتابعة؟",
        [Ids.ConfirmYes] = "نعم", [Ids.ConfirmNo] = "لا",
        [Ids.ConfirmResultYes] = "Confirm: نعم", [Ids.ConfirmResultNo] = "Confirm: لا",
        [Ids.PromptTitle] = "إدخال", [Ids.PromptMessage] = "أدخل اسماً",
        [Ids.PromptPlaceholder] = "الاسم", [Ids.PromptResultFormat] = "Prompt: {0}",
        [Ids.PromptCancelled] = "أُلغي Prompt",
        [Ids.ToastMessage] = "هذه رسالة Toast", [Ids.ToastShown] = "ظهر Toast",
        [Ids.SnackbarMessage] = "هذه رسالة Snackbar", [Ids.SnackbarShown] = "ظهر Snackbar",
        [Ids.CustomDialogTitle] = "حوار مخصص",
        [Ids.CustomDialogMessage] = "محتوى مخصص داخل حوار الحزمة.",
        [Ids.CustomDialogClosed] = "أُغلق CustomDialog",
        [Ids.CustomNamePlaceholder] = "الاسم", [Ids.CustomDatePlaceholder] = "التاريخ",

        [Ids.CityTehran] = "طهران", [Ids.CityMashhad] = "مشهد", [Ids.CityIsfahan] = "أصفهان",
        [Ids.CityShiraz] = "شيراز", [Ids.CityTabriz] = "تبريز", [Ids.CityAhvaz] = "الأهواز",
        [Ids.CityKaraj] = "كرج", [Ids.CityQom] = "قم", [Ids.CityKerman] = "كرمان",
        [Ids.CityRasht] = "رشت", [Ids.CityYazd] = "يزد", [Ids.CityHamedan] = "همدان",
        [Ids.Option1] = "الخيار ١", [Ids.Option2] = "الخيار ٢", [Ids.Option3] = "الخيار ٣", [Ids.Option4] = "الخيار ٤",
        [Ids.TreeL11] = "المستوى ١-١", [Ids.TreeL21] = "المستوى ٢-١", [Ids.TreeL22] = "المستوى ٢-٢",
        [Ids.TreeL12] = "المستوى ١-٢", [Ids.TreeL21b] = "المستوى ٢-١", [Ids.TreeL31] = "المستوى ٣-١",
        [Ids.TreeL32] = "المستوى ٣-٢", [Ids.TreeL41] = "المستوى ٤-١",
    };

    private static readonly Dictionary<string, Dictionary<string, string>> Catalogs =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["fa"] = Fa(),
            ["en"] = En(),
            ["ar"] = Ar(),
        };
}
