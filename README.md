# Maui Persian Toolkit

[![NuGet](https://img.shields.io/nuget/v/MauiPersianToolkit.svg)](https://www.nuget.org/packages/MauiPersianToolkit/)
[![License](https://img.shields.io/github/license/RezaShaban/MauiPersianToolkit)](LICENSE)
[![Build](https://github.com/RezaShaban/MauiPersianToolkit/actions/workflows/dotnet.yml/badge.svg)](https://github.com/RezaShaban/MauiPersianToolkit/actions)
[![Tests](https://img.shields.io/badge/tests-20%2B%20passing-brightgreen)](https://github.com/RezaShaban/MauiPersianToolkit/actions)
[![.NET](https://img.shields.io/badge/.NET-10.0-blueviolet)](https://dotnet.microsoft.com/)

`MauiPersianToolkit` is a comprehensive library for **.NET MAUI** that provides Persian-language UI controls with native popups, alerts, and full light/dark theme support. It helps you build modern cross-platform apps with RTL layouts and Persian, Gregorian, and Hijri calendars — with **no CommunityToolkit dependency**.

## ✨ Key Features

### 📅 Advanced Calendar System
- **Multiple Calendar Support**: Persian (Jalali), Gregorian, and Islamic (Hijri) calendars
- **Correct week starts**: Saturday→Friday (Persian/Hijri), Sunday→Saturday (Gregorian)
- **Flexible DatePicker**: Single, Multiple, and Range selection modes
- **Calendar Service Architecture**: Strategy pattern for extensible calendar implementations
- **Unit Tests**: Calendar conversion, week layout, and Hijri display coverage

### 🎨 UI Controls
- **Persian DatePicker**: Customizable date picker with multiple selection modes
- **TreeView**: None, Single, and Multiple selection with hierarchy support (virtualized `CollectionView` rows)
- **TabView**: Bottom tabs with sliding indicator, content cross-fade, icon pulse, and lazy pages
- **SlideButton**: Interactive slideable confirmation button
- **Picker**: Single and Multiple selection with enhanced UI
- **Entry & Editor**: Persian/RTL-friendly text inputs
- **Expander**: Native expandable/collapsible container
- **CheckBox, Button, Circle Image**: Custom-styled components

### 💬 Dialogs & Alerts (native)
- **Alert / Confirm / Prompt / Custom** dialogs on a native popup surface
- **Toast** and **Snackbar** (Android, iOS, Windows)
- **Popup** API for custom overlays — no third-party popup package required

### 🌓 Light / Dark theme
- Follows the system (or app) `AppTheme` automatically
- Overridable semantic color tokens (`PdtSurfaceLight`, `PdtAccent`, …)
- Configure via `UseMauiPersianToolkit` options, `App.xaml` resources, or `PersianTheme.Configure` at runtime

### 🌐 Localization
- UI chrome (dialogs, picker, date picker buttons) via `PersianToolkitStrings`
- Built-in **fa** and **en**; add catalogs or plug in `IPersianToolkitLocalizer`
- XAML: `{mpt:Localize Today}`

### 🛠️ Developer Tools
- **Converters**: PersianDateConverter, PersianDateTimeConverter, and more
- **Extensions**: Calendar extensions for easy date manipulation
- **Custom Fonts**: Embedded IranianSans and FontAwesome
- **RTL Support**: Full right-to-left layout support for all controls

## 📋 Project Statistics

- **Version**: 3.0.0
- **Target Framework**: .NET 10.0 (`net10.0`, Android, iOS, Windows)
- **Platforms**: Windows, iOS, Android
- **License**: MIT
- **Dependencies**: none beyond `Microsoft.Maui.Controls`
- **Tests**: calendar + week-layout + Hijri display tests
- **CI/CD**: automated build and tests on every PR

## 🚀 Installation

You can install the `MauiPersianToolkit` package via NuGet Package Manager or .NET CLI:

### NuGet Package Manager
```powershell
Install-Package MauiPersianToolkit
```

### .NET CLI
```bash
dotnet add package MauiPersianToolkit
```

## 🎯 Getting Started

### 1. Startup Configuration

Register the toolkit in `MauiProgram.cs`:

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UseMauiPersianToolkit();  // registers fonts, handlers, theme resources

        return builder.Build();
    }
}
```

The toolkit ships **native** popups, toasts, snackbars and expander — no CommunityToolkit (or other UI package) is required.

Application-wide defaults (including theme colors) can be set at the same time:

```csharp
builder.UseMauiPersianToolkit(options =>
{
    options.DefaultCalendarType = CalendarType.Persian;
    options.DefaultFontFamily = "IranianSans";
    options.DefaultAcceptText = "ثبت";

    // Optional light/dark palette overrides
    options.Theme.SurfaceDark = Color.FromArgb("#0D1117");
    options.Theme.Accent = Colors.Teal;
    options.Theme.Accept = Color.FromArgb("#2EA043");
});
```

### 2. Merge styles in `App.xaml`

Merge `PersianStyles` so default theme tokens and control styles are available:

```xml
<Application xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:persian="clr-namespace:MauiPersianToolkit.Resources;assembly=MauiPersianToolkit"
             x:Class="YourApp.App">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <persian:PersianStyles/>
            </ResourceDictionary.MergedDictionaries>

            <!-- Optional: override any Pdt* token after merging PersianStyles -->
            <!-- <Color x:Key="PdtSurfaceDark">#0D1117</Color> -->
            <!-- <Color x:Key="PdtAccent">#2DD4BF</Color> -->
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

If you forget to merge `PersianStyles`, `UseMauiPersianToolkit` injects the defaults automatically when the app (or the first toolkit control) starts. Merging them in `App.xaml` is still fine if you want to override tokens in XAML:

```csharp
public App()
{
    InitializeComponent();
    PersianTheme.EnsureApplicationStyles(); // optional; also happens automatically
    MainPage = new AppShell();
}
```

### 3. Basic XAML Usage

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:persian="http://schemas.mauipersiantoolkit.com/2026/toolkit"
             x:Class="YourApp.MainPage"
             Title="Persian Toolkit Demo">

    <ScrollView>
        <StackLayout Padding="20" Spacing="15">
            
            <!-- Persian DatePicker - Single Selection -->
            <Label Text="Persian DatePicker (Single)" FontSize="16" FontAttributes="Bold"/>
            <persian:DatePicker 
                PlaceHolder="Select Date" 
                SelectedPersianDate="{Binding SelectedDate}"
                CalendarType="Persian"
                DisplayFormat="yyyy/MM/dd" />
            
            <!-- Gregorian DatePicker -->
            <Label Text="Gregorian DatePicker" FontSize="16" FontAttributes="Bold"/>
            <persian:DatePicker 
                PlaceHolder="Select Date" 
                SelectedPersianDate="{Binding SelectedGregorianDate}"
                CalendarType="Gregorian"
                DisplayFormat="yyyy/MM/dd" />
            
            <!-- Islamic (Hijri) DatePicker -->
            <Label Text="Islamic DatePicker" FontSize="16" FontAttributes="Bold"/>
            <persian:DatePicker 
                PlaceHolder="Select Date" 
                SelectedPersianDate="{Binding SelectedHijriDate}"
                CalendarType="Hijri"
                DisplayFormat="yyyy/MM/dd" />

            <!-- Entry Control -->
            <Label Text="Entry Field" FontSize="16" FontAttributes="Bold"/>
            <persian:EntryView 
                PlaceHolder="Enter your name" 
                Text="{Binding UserName}" />

            <!-- Expander Control -->
            <Label Text="Expandable Section" FontSize="16" FontAttributes="Bold"/>
            <persian:Expander IsExpanded="False" Header="Click to expand">
                <Label Text="This content is hidden until expanded" Padding="10"/>
            </persian:Expander>

        </StackLayout>
    </ScrollView>
</ContentPage>
```

### 4. Calendar System Usage

#### Using Calendar Services Directly

```csharp
using MauiPersianToolkit.Services.Calendar;
using MauiPersianToolkit.Enums;

// Get calendar service
var persianService = CalendarServiceFactory.GetService(CalendarType.Persian);
var gregorianService = CalendarServiceFactory.GetService(CalendarType.Gregorian);
var hijriService = CalendarServiceFactory.GetService(CalendarType.Hijri);

// Convert dates
var today = DateTime.Now;
string persianDate = persianService.ToCalendarDate(today);      // "1403/05/25"
string gregorianDate = gregorianService.ToCalendarDate(today);  // "2024/08/16"
string hijriDate = hijriService.ToCalendarDate(today);          // "1446/02/21"

// Parse dates back
var parsed = persianService.ToGregorianDate("1403/05/25");

// Get calendar information
int year = persianService.GetYear(today);
int month = persianService.GetMonth(today);
string monthName = persianService.GetMonthName(month);
bool isLeap = persianService.IsLeapYear(year);
DayOfWeek first = persianService.GetFirstDayOfWeek();  // Saturday
DayOfWeek holiday = persianService.GetLastDayOfWeek();  // Friday for Persian
```

#### Using Extension Methods (Backward Compatible)

```csharp
using MauiPersianToolkit;

DateTime today = DateTime.Now;

// Convert to Persian (default)
string persianDate = today.ToPersianDate();  // "1403/05/25"

// Convert to other calendars
string gregorianDate = today.ToCalendarDate(CalendarType.Gregorian);
string hijriDate = today.ToCalendarDate(CalendarType.Hijri);

// Parse back
var parsed = "1403/05/25".ToDateTime();
var gregorianParsed = "2024/08/16".ToDateTime(CalendarType.Gregorian);
```

### 5. Dialog Usage

```csharp
using MauiPersianToolkit.Services.Dialog;
using MauiPersianToolkit.Models;

public partial class MainPage : ContentPage
{
    private readonly IDialogService _dialogService;
    
    public MainPage(IDialogService dialogService)
    {
        InitializeComponent();
        _dialogService = dialogService;
    }

    // Alert Dialog
    private async void ShowAlert()
    {
        await _dialogService.Alert("This is an alert message");
    }

    // Confirm Dialog
    private async void ShowConfirm()
    {
        var config = new ConfirmConfig
        {
            Title = "Confirm Action",
            Message = "Are you sure?",
            AcceptText = "Yes",
            CancelText = "No",
            OnAction = (result) =>
            {
                if (result)
                    Debug.WriteLine("User confirmed");
                else
                    Debug.WriteLine("User cancelled");
            }
        };
        
        await _dialogService.Confirm(config);
    }

    // Prompt Dialog
    private async void ShowPrompt()
    {
        var config = new PromptConfig
        {
            Title = "Enter Name",
            Message = "Please enter your name:",
            Placeholder = "Name",
            AcceptText = "OK",
            CancelText = "Cancel",
            OnAction = (result) =>
            {
                if (result.IsOk)
                    Debug.WriteLine($"User entered: {result.Input}");
            }
        };
        
        await _dialogService.Prompt(config);
    }

    // Custom Dialog
    private async void ShowCustomDialog()
    {
        var customContent = new StackLayout
        {
            Children =
            {
                new EntryView { PlaceHolder = "Name" },
                new EntryView { PlaceHolder = "Email" },
                new persian:DatePicker { PlaceHolder = "Birth Date" }
            }
        };

        var config = new CustomDialogConfig
        {
            Title = "Register",
            Message = "Enter your information",
            Content = customContent,
            AcceptText = "Register",
            CancelText = "Cancel",
            OnAction = (result) =>
            {
                Debug.WriteLine($"Dialog result: {result}");
            }
        };
        
        await _dialogService.CustomDialog(config);
    }
}
```

### 6. Toast & Snackbar

```csharp
using MauiPersianToolkit.Alerts;
using MauiPersianToolkit.Core;

// Toast — short non-interactive message
await Toast.Make("Saved successfully", ToastDuration.Short).Show();

// Snackbar — message with an optional action
await Snackbar.Make(
    "Item deleted",
    action: () => { /* undo */ },
    actionButtonText: "Undo",
    duration: TimeSpan.FromSeconds(3)).Show();
```

Snackbar colors follow the current theme (`PdtAlertBg*` / `PdtAlertFg*`) and can be overridden through `SnackbarOptions` or theme tokens.

## 🌓 Light / Dark theme customization

Controls, dialogs and alerts use semantic `Pdt*` color keys and `AppThemeBinding`, so they follow the system (or app) light/dark theme. Consumers can replace any token.

### Option A — `UseMauiPersianToolkit` (C#)

```csharp
builder.UseMauiPersianToolkit(options =>
{
    options.Theme.PageDark = Color.FromArgb("#0B0F14");
    options.Theme.SurfaceDark = Color.FromArgb("#161B22");
    options.Theme.InputFillDark = Color.FromArgb("#1F2937");
    options.Theme.OutlineDark = Color.FromArgb("#374151");
    options.Theme.OnSurfaceDark = Color.FromArgb("#F3F4F6");
    options.Theme.Accent = Color.FromArgb("#2DD4BF");
    options.Theme.Accept = Color.FromArgb("#22C55E");
    options.Theme.Cancel = Color.FromArgb("#EF4444");
});
```

### Option B — `App.xaml` resources

Define the same keys **after** merging `PersianStyles` so your values win:

```xml
<ResourceDictionary.MergedDictionaries>
    <persian:PersianStyles/>
</ResourceDictionary.MergedDictionaries>

<Color x:Key="PdtSurfaceDark">#0D1117</Color>
<Color x:Key="PdtOnSurfaceDark">#E6EDF3</Color>
<Color x:Key="PdtAccent">#58A6FF</Color>
<Color x:Key="PdtAlertBgDark">#F0F6FC</Color>
<Color x:Key="PdtAlertFgDark">#0D1117</Color>
```

### Option C — runtime

```csharp
PersianTheme.Configure(theme =>
{
    theme.SurfaceDark = Color.FromArgb("#111827");
    theme.Accent = Colors.Orange;
});
```

### Theme token reference

| Key | Role |
|-----|------|
| `PdtPageLight` / `PdtPageDark` | Page background |
| `PdtSurfaceLight` / `PdtSurfaceDark` | Cards, dialogs, containers |
| `PdtInputFillLight` / `PdtInputFillDark` | Entry / picker / date field fill |
| `PdtOutlineLight` / `PdtOutlineDark` | Borders |
| `PdtOnSurfaceLight` / `PdtOnSurfaceDark` | Primary text / icons |
| `PdtMutedLight` / `PdtMutedDark` | Secondary text |
| `PdtFooterLight` / `PdtFooterDark` | Dialog footers |
| `PdtDisabledLight` / `PdtDisabledDark` | Disabled text |
| `PdtDayButtonLight` / `PdtDayButtonDark` | Calendar day cells |
| `PdtAccept` / `PdtCancel` / `PdtAccent` | Actions & accent (shared) |
| `PdtAlertBgLight` / `PdtAlertBgDark` | Toast / snackbar background |
| `PdtAlertFgLight` / `PdtAlertFgDark` | Toast / snackbar text |

Constants live in `PersianThemeKeys`. Defaults are defined in `PersianStyles`.

## 🌐 Localization (multi-language UI chrome)

Toolkit button labels and placeholders (Accept, Cancel, Today, Select date, …) are **not hardcoded** anymore.
Built-in catalogs: **Persian (`fa`)** and **English (`en`)**. Calendar month/day names stay with each calendar service (next localization step).

### Switch language at startup

```csharp
builder.UseMauiPersianToolkit(options =>
{
    options.Localization.Culture = "en"; // or "fa", "fa-IR", "en-US"
});
```

### Override individual strings

```csharp
builder.UseMauiPersianToolkit(options =>
{
    options.Localization.Culture = "fa";
    options.Localization.StringOverrides[PersianToolkitStringId.Accept] = "تایید";
    options.Localization.StringOverrides[PersianToolkitStringId.Ok] = "متوجه شدم";
});
```

### Add another language

```csharp
builder.UseMauiPersianToolkit(options =>
{
    options.Localization.AdditionalCatalogs["ar"] = new Dictionary<string, string>
    {
        [PersianToolkitStringId.Accept] = "حفظ",
        [PersianToolkitStringId.Cancel] = "إلغاء",
        [PersianToolkitStringId.Confirm] = "موافق",
        [PersianToolkitStringId.Ok] = "حسناً",
        [PersianToolkitStringId.SelectDate] = "اختر التاريخ",
        [PersianToolkitStringId.Today] = "اليوم",
        [PersianToolkitStringId.SystemErrorTitle] = "خطأ في النظام",
    };
    options.Localization.Culture = "ar";
});
```

### Change culture at runtime

```csharp
PersianToolkitStrings.SetCulture("en");
```

> Note: XAML `{mpt:Localize Today}` is resolved when the view is created. Recreate the page (or navigate away/back) after a runtime culture change so labels refresh.

### Keys

| Key | Persian | English |
|-----|---------|---------|
| `Accept` | ثبت | Save |
| `Cancel` | انصراف | Cancel |
| `Confirm` | تایید | OK |
| `Ok` | باشه | OK |
| `SelectDate` | انتخاب تاریخ | Select date |
| `Today` | امروز | Today |
| `SystemErrorTitle` | خطای سیستمی | System error |

Use `PersianToolkitStringId.*` in code and `{mpt:Localize Today}` (or `Key=SelectDate`) in XAML.

## 📚 Examples and Documentation

### Calendar Service Examples
See `PersianUISamples/Examples/CalendarServiceExamples.cs` for:
- Extension methods usage
- Calendar service factory usage
- Calendar conversions
- DatePicker control usage
- Custom calendar registration

### Calendar Tests
The project includes comprehensive unit tests:

**CalendarServiceTests** - 7 tests covering:
- Date conversion roundtrip
- Month boundaries
- Holiday detection
- Month names
- Leap year validation
- DatePickerViewModel integration
- Date formatting

**CalendarWeekLayoutTests** - 13+ tests covering:
- Consecutive days alignment
- Week structure (7 columns per week)
- Persian calendar day positioning
- Gregorian calendar day positioning
- Empty cells before first day
- End of month positioning
- Multiple month validation
- Holiday day placement
- Specific date column placement

Run tests:
```bash
dotnet test ./MauiPersianToolkit.Test/MauiPersianToolkit.Test.csproj
```

## 🔧 Customization

### Custom Calendar Type

Implement `ICalendarService` for custom calendar support:

```csharp
public class MyCustomCalendarService : ICalendarService
{
    public DayOfWeek GetFirstDayOfWeek() => DayOfWeek.Saturday;
    public DayOfWeek GetLastDayOfWeek() => DayOfWeek.Friday;
    public string ToCalendarDate(DateTime gregorianDate) { /* ... */ }
    public DateTime ToGregorianDate(string calendarDate) { /* ... */ }
    // ... other methods
}

// Register with factory
CalendarServiceFactory.RegisterService(
    CalendarType.Custom,
    new MyCustomCalendarService()
);
```

### Control style customization

```xml
<persian:DatePicker
    PlaceHolder="Select Date"
    SelectDayColor="Blue"
    CanSelectHolidays="False"
    DisplayFormat="yyyy/MM/dd" />
```

For app-wide light/dark colors, prefer the [theme tokens](#-light--dark-theme-customization) above rather than hard-coding colors on each control.

## 🧪 Testing

The project includes a comprehensive test suite:

```
MauiPersianToolkit.Test/
├── CalendarServiceTests.cs (7 tests)
├── CalendarWeekLayoutTests.cs (13+ tests)
└── CalendarWeekLayoutTests.README.md (documentation)
```

### GitHub Actions CI/CD

Automated testing on every commit:
- ✅ Build validation
- ✅ Unit tests execution
- ✅ Calendar-specific tests
- ✅ Code coverage reports

See `.github/workflows/` for CI/CD configuration.

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

## 🤝 Contributing

We welcome contributions! Follow these steps:

1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/YourFeature`)
3. **Commit** your changes (`git commit -m 'Add YourFeature'`)
4. **Push** to the branch (`git push origin feature/YourFeature`)
5. **Open** a Pull Request

### Contribution Guidelines

- Follow existing code style and conventions
- Add unit tests for new features
- Update documentation as needed
- Ensure all tests pass locally
- Provide clear PR description

## 🙏 Acknowledgments

- .NET MAUI Community for excellent framework and tools
- All contributors and users for feedback and suggestions

## 📞 Support

- **Issues**: [GitHub Issues](https://github.com/RezaShaban/MauiPersianToolkit/issues)
- **Discussions**: [GitHub Discussions](https://github.com/RezaShaban/MauiPersianToolkit/discussions)
- **Website**: [Project Homepage](https://rezashaban.github.io/MauiPersianToolkit)

## 🔗 Quick Links

- [NuGet Package](https://www.nuget.org/packages/MauiPersianToolkit/)
- [GitHub Repository](https://github.com/RezaShaban/MauiPersianToolkit)
- [Documentation](https://github.com/RezaShaban/MauiPersianToolkit/wiki)
- [Issues & Bugs](https://github.com/RezaShaban/MauiPersianToolkit/issues)

## 📊 Project Status

| Component | Status | Notes |
|-----------|--------|--------|
| Core Library | ✅ Stable | .NET 10, no CommunityToolkit |
| Calendar System | ✅ Stable | Persian / Gregorian / Hijri |
| UI Controls | ✅ Stable | Inputs, picker, tabs, tree, … |
| Native Popup / Toast / Snackbar | ✅ Stable | Android, iOS, Windows |
| Light / Dark theme | ✅ Stable | Overridable `Pdt*` tokens |
| Dialog System | ✅ Stable | Alert, Confirm, Prompt, Custom |
| Unit Tests | ✅ Active | Calendar + layout + Hijri |
| Documentation | ✅ Updated | README + samples |

---

**Happy Coding! 🚀**

Start building beautiful Persian-enabled applications with MauiPersianToolkit today!