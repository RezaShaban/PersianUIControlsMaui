using MauiPersianToolkit.Controls;
using MauiPersianToolkit.Models;

namespace MauiPersianToolkit.Dialogs;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CustomDialogPage : Popup
{
    CustomDialogConfig _config;
    public CustomDialogPage(CustomDialogConfig config)
    {
        InitializeComponent();
        _config = config;
        BindingContext = _config;

        if (config.Message != null && config.Message.Length > 0 && config.Message.Contains("#"))
        {
            lblMessage.FormattedText = new FormattedString();
            foreach (var str in config.Message.Split('#'))
            {
                lblMessage.FormattedText.Spans.Add(new Span()
                {
                    Text = str.Replace("#", ""),
                    TextColor = config.Message.Contains('#' + str.Trim() + '#') ? Color.FromArgb("#ff7800") : Colors.Gray,
                    FontAttributes = FontAttributes.Bold,
                    FontFamily = "IranianSans"
                });
            }
        }
        SetDialogProperties();
    }

    private void SetDialogProperties()
    {
        double width = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        double height = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
        container.MaximumWidthRequest = width;
        scrollView.WidthRequest = width - 40;
        lblMessage.WidthRequest = scrollView.WidthRequest;
        lblTitle.WidthRequest = width;
        //this.container.MaximumHeightRequest = (height * 0.8) - 50;
    }

    private async void btnCancel_Clicked(object sender, EventArgs e)
    {
        _config.OnAction?.Invoke(false);
        await this.CloseAsync();
    }

    private async void btnAccept_Clicked(object sender, EventArgs e)
    {
        _config.OnAction?.Invoke(true);
        if (_config.CloseAfterAccept)
            await this.CloseAsync();
    }
}