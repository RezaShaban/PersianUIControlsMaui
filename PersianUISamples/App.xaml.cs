using MauiPersianToolkit;
using PersianUISamples.Services;

namespace PersianUISamples;

public partial class App : Application
{
    public App(DemoSettingsService settings)
    {
        InitializeComponent();
        PersianTheme.EnsureApplicationStyles();
        settings.ApplyOnStartup();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}
