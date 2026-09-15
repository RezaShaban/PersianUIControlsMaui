using MauiPersianToolkit.Enums;
using MauiPersianToolkit.Hosting;
using MauiPersianToolkit.Localization;
using MauiPersianToolkit.Services.Calendar;
using MauiPersianToolkit.Services.Dialog;

namespace MauiPersianToolkit;

/// <summary>
/// Registers the toolkit with a <see cref="MauiAppBuilder"/>.
/// </summary>
public static class AppBuilderExtensions
{
    /// <summary>
    /// Configures the application to use Persian UI controls. This method is obsolete; use <see cref="UseMauiPersianToolkit(MauiAppBuilder)"/>
    /// instead.
    /// </summary>
    /// <param name="builder">The Maui application builder to configure with Persian UI controls.</param>
    /// <returns>The same MauiAppBuilder instance, configured to use Persian UI controls.</returns>
    [Obsolete("Use UseMauiPersianToolkit instead.")]
    public static MauiAppBuilder UsePersianUIControls(this MauiAppBuilder builder)
        => UseMauiPersianToolkit(builder);

    /// <summary>
    /// Registers the toolkit's fonts, handlers and services.
    /// </summary>
    public static MauiAppBuilder UseMauiPersianToolkit(this MauiAppBuilder builder) =>
        builder.UseMauiPersianToolkit(configure: null);

    /// <summary>
    /// Registers the toolkit's fonts, handlers and services, overriding the application-wide
    /// defaults.
    /// </summary>
    /// <param name="builder">Builder to configure.</param>
    /// <param name="configure">Callback used to adjust <see cref="PersianToolkitOptions"/>.</param>
    public static MauiAppBuilder UseMauiPersianToolkit(
        this MauiAppBuilder builder,
        Action<PersianToolkitOptions>? configure)
    {
        var options = new PersianToolkitOptions();
        configure?.Invoke(options);
        options.Localization.Apply();
        PersianToolkitOptions.Current = options;

        if (options.RegisterEmbeddedFonts)
        {
            builder.ConfigureFonts(fonts =>
            {
                var assembly = typeof(AppBuilderExtensions).Assembly;
                fonts.AddEmbeddedResourceFont(assembly, "IranianSans.ttf", "IranianSans");
                fonts.AddEmbeddedResourceFont(assembly, "FontAwesome.ttf", "FontAwesome");
            });
        }

        builder.ConfigureMauiHandlers(handlers => handlers.AddPersianToolkitHandlers());

        builder.Services.AddSingleton(options);
        builder.Services.AddSingleton<IPersianToolkitLocalizer>(_ => PersianToolkitStrings.Current);
        builder.Services.AddSingleton<IDialogService, DialogService>();
        builder.Services.AddTransient<IMauiInitializeService, PersianToolkitResourceInitializer>();
        builder.Services.AddTransient<IMauiInitializeScopedService, PersianToolkitResourceInitializer>();
        builder.Services.AddCalendarServices();

        ApplyPlatformTweaks();

        return builder;
    }

    /// <summary>
    /// Exposes the calendar services through DI. Resolution goes through
    /// <see cref="CalendarServiceFactory"/> rather than registering the implementations
    /// directly, so a service swapped in with
    /// <see cref="CalendarServiceFactory.RegisterService"/> is what the container hands out
    /// too. That keeps the container and the static extension methods in agreement.
    /// </summary>
    private static IServiceCollection AddCalendarServices(this IServiceCollection services)
    {
        foreach (var calendarType in Enum.GetValues<CalendarType>())
        {
            services.AddKeyedSingleton<ICalendarService>(
                calendarType,
                (_, key) => CalendarServiceFactory.GetService((CalendarType)key!));
        }

        services.AddSingleton<ICalendarServiceResolver, CalendarServiceResolver>();
        services.AddSingleton(sp => sp.GetRequiredKeyedService<ICalendarService>(
            PersianToolkitOptions.Current.DefaultCalendarType));

        return services;
    }

    /// <summary>
    /// Strips the platform chrome MAUI's entry and editor handlers render by default, so the
    /// toolkit's own underline styling is the only thing the user sees.
    /// </summary>
    private static void ApplyPlatformTweaks()
    {
        Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping("PersianToolkit.Editor", (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.Bottom = 0;
            var gd = new Android.Graphics.Drawables.GradientDrawable();
            gd.SetColor(global::Android.Graphics.Color.Transparent);
            handler.PlatformView.SetBackgroundDrawable(gd);
            handler.PlatformView.BackgroundTintList =
                Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif IOS || MACCATALYST
            handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
            handler.PlatformView.Layer.BorderWidth = 0;
            handler.PlatformView.Layer.BorderColor = UIKit.UIColor.Clear.CGColor;
#elif WINDOWS
            handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
            handler.PlatformView.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
#endif
        });

        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("PersianToolkit.Entry", (handler, view) =>
        {
#if IOS || MACCATALYST
            handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
            handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
            handler.PlatformView.VerticalAlignment = UIKit.UIControlContentVerticalAlignment.Center;
#elif ANDROID
            var edit = handler.PlatformView;
            edit.BackgroundTintList =
                Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
            edit.SetSelectAllOnFocus(true);
            // Keep font padding; IranianSans sits high in the em box — bias padding downward.
            edit.SetIncludeFontPadding(true);
            var density = edit.Resources?.DisplayMetrics?.Density ?? 1f;
            var top = (int)(10 * density);
            var bottom = (int)(2 * density);
            edit.SetPadding(edit.PaddingLeft, top, edit.PaddingRight, bottom);
            edit.Gravity = (edit.Gravity & Android.Views.GravityFlags.HorizontalGravityMask)
                | Android.Views.GravityFlags.CenterVertical;
#elif WINDOWS
            // WinUI TextBox draws its own underline/border that covers the toolkit outline.
            handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
            handler.PlatformView.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
            handler.PlatformView.Resources["TextControlBorderThemeThickness"] = new Microsoft.UI.Xaml.Thickness(0);
            handler.PlatformView.Resources["TextControlBorderThemeThicknessFocused"] = new Microsoft.UI.Xaml.Thickness(0);
            handler.PlatformView.VerticalContentAlignment = Microsoft.UI.Xaml.VerticalAlignment.Center;
            // Slight top bias so IranianSans reads optically centered.
            handler.PlatformView.Padding = new Microsoft.UI.Xaml.Thickness(0, 8, 0, 2);
            handler.PlatformView.GotFocus += (_, _) => handler.PlatformView.SelectAll();
#endif
        });
    }
}
