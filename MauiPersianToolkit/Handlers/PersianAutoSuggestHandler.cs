#if ANDROID
using PlatformView = Android.Widget.AutoCompleteTextView;
#elif IOS || MACCATALYST
using PlatformView = UIKit.UITextField;
#elif WINDOWS
using PlatformView = Microsoft.UI.Xaml.Controls.AutoSuggestBox;
#else
using PlatformView = System.Object;
#endif

using Microsoft.Maui.Handlers;

namespace MauiPersianToolkit.Handlers;

/// <summary>
/// Maps <see cref="Controls.PersianAutoSuggest"/> onto the platform suggest control.
/// </summary>
public partial class PersianAutoSuggestHandler : ViewHandler<Controls.PersianAutoSuggest, PlatformView>
{
    public static IPropertyMapper<Controls.PersianAutoSuggest, PersianAutoSuggestHandler> Mapper =
        new PropertyMapper<Controls.PersianAutoSuggest, PersianAutoSuggestHandler>(ViewMapper)
        {
            [nameof(Controls.PersianAutoSuggest.Text)] = MapText,
            [nameof(Controls.PersianAutoSuggest.Placeholder)] = MapPlaceholder,
            [nameof(Controls.PersianAutoSuggest.TextColor)] = MapTextColor,
            [nameof(Controls.PersianAutoSuggest.PlaceholderColor)] = MapPlaceholderColor,
            [nameof(Controls.PersianAutoSuggest.FontFamily)] = MapFont,
            [nameof(Controls.PersianAutoSuggest.FontSize)] = MapFont,
            [nameof(Controls.PersianAutoSuggest.IsEnabled)] = MapIsEnabled,
            [nameof(Controls.PersianAutoSuggest.Threshold)] = MapThreshold,
            [nameof(Controls.PersianAutoSuggest.Suggestions)] = MapSuggestions,
            [nameof(Controls.PersianAutoSuggest.UseNativeSuggestions)] = MapUseNativeSuggestions,
            [nameof(Controls.PersianAutoSuggest.FlowDirection)] = MapFlowDirection,
        };

    public PersianAutoSuggestHandler() : base(Mapper)
    {
    }

    public PersianAutoSuggestHandler(IPropertyMapper mapper) : base(mapper ?? Mapper)
    {
    }

    protected override partial PlatformView CreatePlatformView();

    protected override partial void ConnectHandler(PlatformView platformView);

    protected override partial void DisconnectHandler(PlatformView platformView);

    public static partial void MapText(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view);
    public static partial void MapPlaceholder(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view);
    public static partial void MapTextColor(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view);
    public static partial void MapPlaceholderColor(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view);
    public static partial void MapFont(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view);
    public static partial void MapIsEnabled(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view);
    public static partial void MapThreshold(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view);
    public static partial void MapSuggestions(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view);
    public static partial void MapUseNativeSuggestions(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view);
    public static partial void MapFlowDirection(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view);

    /// <summary>Platform-specific close of the native suggestion popup.</summary>
    public partial void DismissSuggestions();
}
