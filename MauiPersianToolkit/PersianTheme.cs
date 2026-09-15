using MauiPersianToolkit.Hosting;
using MauiPersianToolkit.Resources;

namespace MauiPersianToolkit;

/// <summary>
/// Applies or refreshes toolkit theme resources on the running application.
/// </summary>
public static class PersianTheme
{
    private static readonly object Sync = new();
    private static bool _optionsApplied;

    /// <summary>
    /// Built-in palette used when the host has not overridden <c>Pdt*</c> keys.
    /// </summary>
    private static readonly (string Key, Color Value)[] DefaultColors =
    [
        (PersianThemeKeys.PageLight, Color.FromArgb("#F5F5F5")),
        (PersianThemeKeys.PageDark, Color.FromArgb("#121212")),
        (PersianThemeKeys.SurfaceLight, Colors.White),
        (PersianThemeKeys.SurfaceDark, Color.FromArgb("#1E1E1E")),
        (PersianThemeKeys.InputFillLight, Color.FromArgb("#FDFDFD")),
        (PersianThemeKeys.InputFillDark, Color.FromArgb("#2C2C2C")),
        (PersianThemeKeys.OutlineLight, Color.FromArgb("#DCDCDC")),
        (PersianThemeKeys.OutlineDark, Color.FromArgb("#555555")),
        (PersianThemeKeys.OnSurfaceLight, Color.FromArgb("#121212")),
        (PersianThemeKeys.OnSurfaceDark, Color.FromArgb("#F2F2F2")),
        (PersianThemeKeys.MutedLight, Color.FromArgb("#666666")),
        (PersianThemeKeys.MutedDark, Color.FromArgb("#AAAAAA")),
        (PersianThemeKeys.FooterLight, Color.FromArgb("#F5F5F5")),
        (PersianThemeKeys.FooterDark, Color.FromArgb("#2A2A2A")),
        (PersianThemeKeys.DisabledLight, Color.FromArgb("#999999")),
        (PersianThemeKeys.DisabledDark, Color.FromArgb("#777777")),
        (PersianThemeKeys.DayButtonLight, Colors.White),
        (PersianThemeKeys.DayButtonDark, Color.FromArgb("#2C2C2C")),
        (PersianThemeKeys.Accept, Color.FromArgb("#1F883D")),
        (PersianThemeKeys.Cancel, Color.FromArgb("#FF4500")),
        (PersianThemeKeys.Accent, Color.FromArgb("#5B2BDF")),
        (PersianThemeKeys.AlertBgLight, Color.FromArgb("#CC323232")),
        (PersianThemeKeys.AlertBgDark, Color.FromArgb("#E8E8E8")),
        (PersianThemeKeys.AlertFgLight, Colors.White),
        (PersianThemeKeys.AlertFgDark, Color.FromArgb("#121212")),
    ];

    /// <summary>
    /// Ensures default <see cref="PersianStyles"/> tokens exist, then applies
    /// <paramref name="configure"/> overrides on <see cref="Application.Current"/> resources.
    /// </summary>
    public static void Configure(Action<PersianThemeOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);

        var app = Application.Current
            ?? throw new InvalidOperationException("Application.Current is not available yet.");

        lock (Sync)
        {
            EnsureStylesCore(app.Resources);

            var options = new PersianThemeOptions();
            configure(options);
            options.Apply(app.Resources);
            _optionsApplied = true;
        }
    }

    /// <summary>
    /// Applies an already-built options instance.
    /// </summary>
    public static void Apply(PersianThemeOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var app = Application.Current
            ?? throw new InvalidOperationException("Application.Current is not available yet.");

        lock (Sync)
        {
            EnsureStylesCore(app.Resources);
            options.Apply(app.Resources);
            _optionsApplied = true;
        }
    }

    /// <summary>
    /// Ensures toolkit defaults (and one-time <see cref="PersianToolkitOptions.Theme"/> overrides)
    /// are present on <see cref="Application.Current"/>.
    /// </summary>
    public static void EnsureApplicationStyles()
    {
        var app = Application.Current;
        if (app?.Resources is null)
            return;

        lock (Sync)
        {
            EnsureStylesCore(app.Resources);

            if (_optionsApplied)
                return;

            PersianToolkitOptions.Current.Theme.Apply(app.Resources);
            _optionsApplied = true;
        }
    }

    /// <summary>
    /// Writes effective <c>Pdt*</c> colors onto a control's own <see cref="ResourceDictionary"/>
    /// so <c>StaticResource</c> resolves during library XAML load (Application merged
    /// dictionaries are not always visible to nested <c>AppThemeBinding</c> lookups).
    /// Call from constructors before <c>InitializeComponent</c>.
    /// </summary>
    public static void SeedControlResources(ResourceDictionary resources)
    {
        ArgumentNullException.ThrowIfNull(resources);

        EnsureApplicationStyles();

        lock (Sync)
        {
            var appResources = Application.Current?.Resources;

            foreach (var (key, fallback) in DefaultColors)
            {
                // Flat local keys beat MergedDictionaries for StaticResource on this control.
                if (resources.ContainsKey(key))
                    continue;

                if (appResources is not null
                    && appResources.TryGetValue(key, out var value)
                    && value is Color color)
                {
                    resources[key] = color;
                }
                else
                {
                    resources[key] = fallback;
                }
            }
        }
    }

    /// <summary>
    /// Makes sure the toolkit's default color keys are present in <paramref name="resources"/>.
    /// </summary>
    public static void EnsureStyles(ResourceDictionary resources)
    {
        ArgumentNullException.ThrowIfNull(resources);

        lock (Sync)
            EnsureStylesCore(resources);
    }

    private static void EnsureStylesCore(ResourceDictionary resources)
    {
        // Nested AppThemeBinding+StaticResource often cannot see MergedDictionary keys.
        // Always materialize flat Pdt* entries on the dictionary itself.
        foreach (var (key, fallback) in DefaultColors)
        {
            if (resources.TryGetValue(key, out var value) && value is Color color)
                resources[key] = color;
            else
                resources[key] = fallback;
        }

        if (resources.MergedDictionaries.OfType<PersianStyles>().Any())
            return;

        resources.MergedDictionaries.Add(new PersianStyles());
    }
}
