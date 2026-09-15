#if !ANDROID && !IOS && !MACCATALYST && !WINDOWS

namespace MauiPersianToolkit.Handlers;

/// <summary>Neutral TFM stub — unit tests never attach this handler to a window.</summary>
public partial class PersianAutoSuggestHandler
{
    private const string NotSupported =
        "PersianAutoSuggest requires Android, iOS, MacCatalyst or Windows.";

    protected override partial object CreatePlatformView() =>
        throw new PlatformNotSupportedException(NotSupported);

    protected override partial void ConnectHandler(object platformView)
    {
    }

    protected override partial void DisconnectHandler(object platformView)
    {
    }

    public static partial void MapText(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view) { }
    public static partial void MapPlaceholder(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view) { }
    public static partial void MapTextColor(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view) { }
    public static partial void MapPlaceholderColor(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view) { }
    public static partial void MapFont(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view) { }
    public static partial void MapIsEnabled(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view) { }
    public static partial void MapThreshold(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view) { }
    public static partial void MapSuggestions(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view) { }
    public static partial void MapUseNativeSuggestions(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view) { }
    public static partial void MapFlowDirection(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view) { }

    public partial void DismissSuggestions()
    {
    }
}

#endif
