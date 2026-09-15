using MauiPersianToolkit;
using PersianUISamples.Pages;
using PersianUISamples.Services;
using PersianUISamples.ViewModels;

namespace PersianUISamples;

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
            .UseMauiPersianToolkit(options =>
            {
                // Built-in catalogs: fa, en, ar. Initial culture follows demo settings at runtime.
                options.Localization.Culture = Preferences.Default.Get("demo.language", "fa");
            });

        builder.Services.AddSingleton<DemoSettingsService>();

        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<InputsPage>();
        builder.Services.AddTransient<AutoCompletePage>();
        builder.Services.AddTransient<PickersPage>();
        builder.Services.AddTransient<DateTimePage>();
        builder.Services.AddTransient<LayoutPage>();
        builder.Services.AddTransient<TreePage>();
        builder.Services.AddTransient<DialogsPage>();
        builder.Services.AddTransient<SettingsPage>();

        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<InputsViewModel>();
        builder.Services.AddTransient<AutoCompleteGalleryViewModel>();
        builder.Services.AddTransient<PickersGalleryViewModel>();
        builder.Services.AddTransient<DateTimeGalleryViewModel>();
        builder.Services.AddTransient<LayoutGalleryViewModel>();
        builder.Services.AddTransient<TreeGalleryViewModel>();
        builder.Services.AddTransient<DialogsGalleryViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();

        return builder.Build();
    }
}
