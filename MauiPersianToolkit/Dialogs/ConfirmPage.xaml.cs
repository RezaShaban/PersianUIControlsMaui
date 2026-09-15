using MauiPersianToolkit.Controls;
using MauiPersianToolkit.Models;

namespace MauiPersianToolkit.Dialogs;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ConfirmPage : Popup
{
    ConfirmConfig config;
    public ConfirmPage(ConfirmConfig _config)
    {
        InitializeComponent();
        config = _config;
        SetDialogProperties();
        BindingContext = config;

    }

    private void SetDialogProperties()
    {
        double width = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        double height = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
        this.container.WidthRequest = width;
        scrollView.WidthRequest = width - 40;
        //lblMessage.WidthRequest = scrollView.WidthRequest;
        lblTitle.WidthRequest = width;
        this.container.MaximumHeightRequest = (height * 0.8) - 50;
    }

    private async void btnCancel_Clicked(object sender, EventArgs e)
    {
        config.OnAction?.Invoke(false);
        await this.CloseAsync();
    }

    private async void btnAccept_Clicked(object sender, EventArgs e)
    {
        config.OnAction?.Invoke(true);
        await this.CloseAsync();
    }
}