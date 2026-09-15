using PersianUISamples.Localization;
using PersianUISamples.Services;

namespace PersianUISamples;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        ApplyLocalization();
    }

    private void ApplyLocalization()
    {
        var settings = ServiceHelper.TryGetService<DemoSettingsService>();
        FlowDirection = settings?.IsRtl == false
            ? FlowDirection.LeftToRight
            : FlowDirection.RightToLeft;

        FlyoutSubtitleLabel.Text = DemoStrings.FlyoutSubtitle;

        SetTitles(HomeItem, HomeContent, DemoStrings.Home);
        SetTitles(InputsItem, InputsContent, DemoStrings.NavInputs);
        SetTitles(AutoCompleteItem, AutoCompleteContent, DemoStrings.NavAutoComplete);
        SetTitles(PickersItem, PickersContent, DemoStrings.NavPickers);
        SetTitles(DateTimeItem, DateTimeContent, DemoStrings.NavDateTime);
        SetTitles(LayoutItem, LayoutContent, DemoStrings.NavLayout);
        SetTitles(TreeItem, TreeContent, DemoStrings.NavTree);
        SetTitles(DialogsItem, DialogsContent, DemoStrings.NavDialogs);
        SetTitles(SettingsItem, SettingsContent, DemoStrings.Settings);
    }

    private static void SetTitles(FlyoutItem item, ShellContent content, string title)
    {
        item.Title = title;
        content.Title = title;
    }
}
