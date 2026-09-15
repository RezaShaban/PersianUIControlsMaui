#if WINDOWS
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WFontFamily = Microsoft.UI.Xaml.Media.FontFamily;

namespace MauiPersianToolkit.Handlers;

public partial class PersianAutoSuggestHandler
{
    private bool _suppressTextCallback;

    protected override partial AutoSuggestBox CreatePlatformView()
    {
        var box = new AutoSuggestBox
        {
            BorderThickness = new Microsoft.UI.Xaml.Thickness(0),
            Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent),
            Padding = new Microsoft.UI.Xaml.Thickness(0),
            VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Center,
            VerticalContentAlignment = Microsoft.UI.Xaml.VerticalAlignment.Center
        };
        return box;
    }

    protected override partial void ConnectHandler(AutoSuggestBox platformView)
    {
        platformView.TextChanged += OnTextChanged;
        platformView.SuggestionChosen += OnSuggestionChosen;
        platformView.GotFocus += OnGotFocus;
        platformView.LostFocus += OnLostFocus;
        VirtualView.HasNativeDropdown = VirtualView.UseNativeSuggestions;
    }

    protected override partial void DisconnectHandler(AutoSuggestBox platformView)
    {
        platformView.TextChanged -= OnTextChanged;
        platformView.SuggestionChosen -= OnSuggestionChosen;
        platformView.GotFocus -= OnGotFocus;
        platformView.LostFocus -= OnLostFocus;
    }

    public static partial void MapText(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        var platform = handler.PlatformView;
        if (platform is null)
            return;

        handler._suppressTextCallback = true;
        try
        {
            var next = view.Text ?? string.Empty;
            if (!string.Equals(platform.Text, next, StringComparison.Ordinal))
                platform.Text = next;
        }
        finally
        {
            handler._suppressTextCallback = false;
        }
    }

    public static partial void MapPlaceholder(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        if (handler.PlatformView is not null)
            handler.PlatformView.PlaceholderText = view.Placeholder ?? string.Empty;
    }

    public static partial void MapTextColor(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        if (handler.PlatformView is not null && view.TextColor is not null)
            handler.PlatformView.Foreground = view.TextColor.ToPlatform();
    }

    public static partial void MapPlaceholderColor(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        // WinUI AutoSuggestBox placeholder brush is theme-driven; keep transparent chrome.
    }

    public static partial void MapFont(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        var platform = handler.PlatformView;
        if (platform is null)
            return;

        platform.FontSize = view.FontSize;
        if (!string.IsNullOrWhiteSpace(view.FontFamily))
            platform.FontFamily = new WFontFamily(view.FontFamily);
    }

    public static partial void MapIsEnabled(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        if (handler.PlatformView is not null)
            handler.PlatformView.IsEnabled = view.IsEnabled;
    }

    public static partial void MapThreshold(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        // AutoSuggestBox has no threshold; filtering is driven by the control.
    }

    public static partial void MapSuggestions(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        var platform = handler.PlatformView;
        if (platform is null)
            return;

        if (!view.UseNativeSuggestions)
        {
            platform.ItemsSource = null;
            return;
        }

        platform.ItemsSource = view.Suggestions?.ToList() ?? [];
        // Do not force IsSuggestionListOpen — AutoSuggestBox opens while typing;
        // forcing it after SelectItem re-opens the list.
    }

    public static partial void MapUseNativeSuggestions(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        view.HasNativeDropdown = view.UseNativeSuggestions;
        if (handler.PlatformView is not null && !view.UseNativeSuggestions)
        {
            handler.PlatformView.IsSuggestionListOpen = false;
            handler.PlatformView.ItemsSource = null;
        }
    }

    public static partial void MapFlowDirection(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        if (handler.PlatformView is null)
            return;

        handler.PlatformView.FlowDirection = view.FlowDirection == Microsoft.Maui.FlowDirection.RightToLeft
            ? Microsoft.UI.Xaml.FlowDirection.RightToLeft
            : Microsoft.UI.Xaml.FlowDirection.LeftToRight;
    }

    private void OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (_suppressTextCallback || VirtualView is null)
            return;

        if (args.Reason == AutoSuggestionBoxTextChangeReason.SuggestionChosen)
            return;

        VirtualView.RaiseTextChangedFromPlatform(sender.Text ?? string.Empty);
    }

    private void OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        if (VirtualView is null)
            return;

        var value = args.SelectedItem?.ToString() ?? sender.Text ?? string.Empty;
        VirtualView.RaiseSuggestionChosen(value);
    }

    private void OnGotFocus(object sender, RoutedEventArgs e) => VirtualView?.RaiseFocused();

    private void OnLostFocus(object sender, RoutedEventArgs e) => VirtualView?.RaiseUnfocused();

    public partial void DismissSuggestions()
    {
        if (PlatformView is null)
            return;

        PlatformView.IsSuggestionListOpen = false;
        PlatformView.ItemsSource = null;
    }
}

#endif
