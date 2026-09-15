using MauiPersianToolkit.Controls;
using MauiPersianToolkit.Models;

namespace MauiPersianToolkit.Dialogs;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class AlertPage : Popup
{
    public AlertPage(AlertConfig config)
    {
        InitializeComponent();
        SetDialogProperties();
        BindingContext = config;
    }

    private void SetDialogProperties()
    {
        double width = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        double height = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
        this.container.WidthRequest = width;
        scrollView.WidthRequest = width - 40;
        lblMessage.WidthRequest = scrollView.WidthRequest;
        lblTitle.WidthRequest = width;
        //this.container.MaximumHeightRequest = (height * 0.8) - 50;
    }

    private async void btnAccept_Clicked(object sender, EventArgs e) =>
        await this.CloseAsync();
}