#if IOS || MACCATALYST
using Microsoft.Maui.Platform;
using UIKit;

namespace MauiPersianToolkit.Handlers;

/// <summary>
/// iOS has no first-class AutoCompleteTextView; the text field is native and the
/// suggestion list is hosted by <see cref="Controls.AutoCompleteView"/> (managed panel).
/// </summary>
public partial class PersianAutoSuggestHandler
{
    private bool _suppressTextCallback;

    protected override partial UITextField CreatePlatformView()
    {
        var field = new UITextField
        {
            BorderStyle = UITextBorderStyle.None,
            BackgroundColor = UIColor.Clear,
            AutocorrectionType = UITextAutocorrectionType.No,
            AutocapitalizationType = UITextAutocapitalizationType.None,
            ClearButtonMode = UITextFieldViewMode.Never,
            VerticalAlignment = UIControlContentVerticalAlignment.Center
        };
        return field;
    }

    protected override partial void ConnectHandler(UITextField platformView)
    {
        platformView.EditingChanged += OnEditingChanged;
        platformView.EditingDidBegin += OnEditingBegan;
        platformView.EditingDidEnd += OnEditingEnded;
        VirtualView.HasNativeDropdown = false;
    }

    protected override partial void DisconnectHandler(UITextField platformView)
    {
        platformView.EditingChanged -= OnEditingChanged;
        platformView.EditingDidBegin -= OnEditingBegan;
        platformView.EditingDidEnd -= OnEditingEnded;
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
            if (platform.Text != next)
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
            handler.PlatformView.Placeholder = view.Placeholder ?? string.Empty;
    }

    public static partial void MapTextColor(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        if (handler.PlatformView is not null && view.TextColor is not null)
            handler.PlatformView.TextColor = view.TextColor.ToPlatform();
    }

    public static partial void MapPlaceholderColor(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        if (handler.PlatformView is null || view.PlaceholderColor is null)
            return;

        handler.PlatformView.AttributedPlaceholder = new Foundation.NSAttributedString(
            view.Placeholder ?? string.Empty,
            foregroundColor: view.PlaceholderColor.ToPlatform());
    }

    public static partial void MapFont(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        var platform = handler.PlatformView;
        if (platform is null)
            return;

        platform.Font = UIFont.SystemFontOfSize((nfloat)view.FontSize);
    }

    public static partial void MapIsEnabled(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        if (handler.PlatformView is not null)
            handler.PlatformView.Enabled = view.IsEnabled;
    }

    public static partial void MapThreshold(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        // Managed dropdown owns the threshold on iOS.
    }

    public static partial void MapSuggestions(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        // Managed dropdown.
    }

    public static partial void MapUseNativeSuggestions(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        view.HasNativeDropdown = false;
    }

    public static partial void MapFlowDirection(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        if (handler.PlatformView is null)
            return;

        handler.PlatformView.SemanticContentAttribute = view.FlowDirection == FlowDirection.RightToLeft
            ? UISemanticContentAttribute.ForceRightToLeft
            : UISemanticContentAttribute.ForceLeftToRight;
        handler.PlatformView.TextAlignment = view.FlowDirection == FlowDirection.RightToLeft
            ? UITextAlignment.Right
            : UITextAlignment.Left;
    }

    private void OnEditingChanged(object? sender, EventArgs e)
    {
        if (_suppressTextCallback || VirtualView is null)
            return;
        VirtualView.RaiseTextChangedFromPlatform(PlatformView.Text ?? string.Empty);
    }

    private void OnEditingBegan(object? sender, EventArgs e) => VirtualView?.RaiseFocused();

    private void OnEditingEnded(object? sender, EventArgs e) => VirtualView?.RaiseUnfocused();

    public partial void DismissSuggestions()
    {
        // Managed panel is owned by AutoCompleteView on iOS.
    }
}

#endif
