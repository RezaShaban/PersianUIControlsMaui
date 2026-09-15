using PersianUISamples.Localization;
using PersianUISamples.Services;
using PersianUISamples.ViewModels;

namespace PersianUISamples.Pages;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        Title = DemoStrings.Settings;
    }

    public SettingsPage() : this(ServiceHelper.GetRequiredService<SettingsViewModel>())
    {
    }
}
