#if ANDROID
using Android.Graphics;
using Android.Text;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Platform;
using AColor = Android.Graphics.Color;
using AView = Android.Views.View;

namespace MauiPersianToolkit.Handlers;

public partial class PersianAutoSuggestHandler
{
    private FontAwareArrayAdapter? _adapter;
    private bool _suppressTextCallback;
    private Typeface? _typeface;

    protected override partial AutoCompleteTextView CreatePlatformView()
    {
        var context = MauiContext?.Context ?? throw new InvalidOperationException("MauiContext.Context is null.");
        var view = new AutoCompleteTextView(context)
        {
            Background = null,
            Threshold = 1
        };
        view.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(AColor.Transparent);
        ApplyVerticalOptics(view);
        return view;
    }

    protected override partial void ConnectHandler(AutoCompleteTextView platformView)
    {
        ResolveTypeface();
        _adapter = new FontAwareArrayAdapter(
            platformView.Context!,
            Android.Resource.Layout.SimpleDropDownItem1Line,
            _typeface);
        platformView.Adapter = _adapter;

        platformView.TextChanged += OnPlatformTextChanged;
        platformView.ItemClick += OnItemClick;
        platformView.FocusChange += OnFocusChange;

        VirtualView.HasNativeDropdown = VirtualView.UseNativeSuggestions;
        ApplyVerticalOptics(platformView);
        ApplyTypeface(platformView);
    }

    protected override partial void DisconnectHandler(AutoCompleteTextView platformView)
    {
        platformView.TextChanged -= OnPlatformTextChanged;
        platformView.ItemClick -= OnItemClick;
        platformView.FocusChange -= OnFocusChange;
        platformView.Adapter = null;
        _adapter = null;
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
            {
                platform.Text = next;
                platform.SetSelection(next.Length);
            }
        }
        finally
        {
            handler._suppressTextCallback = false;
        }
    }

    public static partial void MapPlaceholder(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        if (handler.PlatformView is not null)
            handler.PlatformView.Hint = view.Placeholder ?? string.Empty;
    }

    public static partial void MapTextColor(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        if (handler.PlatformView is not null && view.TextColor is not null)
            handler.PlatformView.SetTextColor(view.TextColor.ToPlatform());
    }

    public static partial void MapPlaceholderColor(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        if (handler.PlatformView is not null && view.PlaceholderColor is not null)
            handler.PlatformView.SetHintTextColor(view.PlaceholderColor.ToPlatform());
    }

    public static partial void MapFont(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        var platform = handler.PlatformView;
        if (platform is null)
            return;

        platform.TextSize = (float)view.FontSize;
        handler.ResolveTypeface();
        handler.ApplyTypeface(platform);
        handler._adapter?.SetTypeface(handler._typeface);
    }

    public static partial void MapIsEnabled(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        if (handler.PlatformView is not null)
            handler.PlatformView.Enabled = view.IsEnabled;
    }

    public static partial void MapThreshold(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        if (handler.PlatformView is not null)
            handler.PlatformView.Threshold = Math.Max(1, view.Threshold);
    }

    public static partial void MapSuggestions(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        var adapter = handler._adapter;
        if (adapter is null)
            return;

        adapter.Clear();
        if (view.Suggestions is not null)
        {
            foreach (var s in view.Suggestions)
                adapter.Add(s);
        }

        adapter.NotifyDataSetChanged();
        // Do NOT call ShowDropDown here — that re-opens the list after a selection sets Text.
    }

    public static partial void MapUseNativeSuggestions(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        view.HasNativeDropdown = view.UseNativeSuggestions;
        if (handler.PlatformView is not null && !view.UseNativeSuggestions)
            handler.PlatformView.DismissDropDown();
    }

    public static partial void MapFlowDirection(PersianAutoSuggestHandler handler, Controls.PersianAutoSuggest view)
    {
        if (handler.PlatformView is null)
            return;

        handler.PlatformView.LayoutDirection = view.FlowDirection == Microsoft.Maui.FlowDirection.RightToLeft
            ? Android.Views.LayoutDirection.Rtl
            : Android.Views.LayoutDirection.Ltr;
        handler.PlatformView.TextDirection = view.FlowDirection == Microsoft.Maui.FlowDirection.RightToLeft
            ? Android.Views.TextDirection.Rtl
            : Android.Views.TextDirection.Ltr;

        ApplyVerticalOptics(handler.PlatformView);
    }

    private void OnPlatformTextChanged(object? sender, Android.Text.TextChangedEventArgs e)
    {
        if (_suppressTextCallback || VirtualView is null)
            return;

        VirtualView.RaiseTextChangedFromPlatform(PlatformView.Text ?? string.Empty);
    }

    private void OnItemClick(object? sender, AdapterView.ItemClickEventArgs e)
    {
        if (VirtualView is null || _adapter is null)
            return;

        var value = _adapter.GetItem(e.Position) ?? string.Empty;
        PlatformView.DismissDropDown();
        _adapter.Clear();
        _adapter.NotifyDataSetChanged();
        // Let AutoCompleteView set Text with suppress — avoid TextChanged→refresh→reopen.
        VirtualView.RaiseSuggestionChosen(value);
    }

    private void OnFocusChange(object? sender, AView.FocusChangeEventArgs e)
    {
        if (VirtualView is null)
            return;

        if (e.HasFocus)
            VirtualView.RaiseFocused();
        else
        {
            PlatformView.DismissDropDown();
            VirtualView.RaiseUnfocused();
        }
    }

    public partial void DismissSuggestions()
    {
        PlatformView?.DismissDropDown();
        _adapter?.Clear();
        _adapter?.NotifyDataSetChanged();
    }

    private void ResolveTypeface()
    {
        if (MauiContext is null || VirtualView is null)
            return;

        try
        {
            var fontManager = MauiContext.Services.GetService(typeof(IFontManager)) as IFontManager;
            if (fontManager is null)
            {
                _typeface = Typeface.Default;
                return;
            }

            var font = Microsoft.Maui.Font.OfSize(VirtualView.FontFamily ?? "IranianSans", VirtualView.FontSize);
            _typeface = fontManager.GetTypeface(font);
        }
        catch
        {
            _typeface = Typeface.Default;
        }
    }

    private void ApplyTypeface(AutoCompleteTextView platform)
    {
        if (_typeface is not null)
            platform.Typeface = _typeface;
    }

    private static void ApplyVerticalOptics(AutoCompleteTextView view)
    {
        // Mirror EntryView optics: IranianSans sits high in the em box.
        view.SetIncludeFontPadding(true);
        var density = view.Resources?.DisplayMetrics?.Density ?? 1f;
        var top = (int)(10 * density);
        var bottom = (int)(2 * density);
        view.SetPadding(view.PaddingLeft, top, view.PaddingRight, bottom);
        view.Gravity = (view.Gravity & GravityFlags.HorizontalGravityMask) | GravityFlags.CenterVertical;
    }

    /// <summary>ArrayAdapter that paints dropdown rows with the toolkit font.</summary>
    private sealed class FontAwareArrayAdapter : ArrayAdapter<string>
    {
        private Typeface? _typeface;

        public FontAwareArrayAdapter(Android.Content.Context context, int resource, Typeface? typeface)
            : base(context, resource)
        {
            _typeface = typeface;
        }

        public void SetTypeface(Typeface? typeface)
        {
            _typeface = typeface;
            NotifyDataSetChanged();
        }

        public override AView GetView(int position, AView? convertView, ViewGroup parent)
        {
            var view = base.GetView(position, convertView, parent);
            Apply(view);
            return view;
        }

        public override AView GetDropDownView(int position, AView? convertView, ViewGroup parent)
        {
            var view = base.GetDropDownView(position, convertView, parent);
            Apply(view);
            return view;
        }

        private void Apply(AView view)
        {
            if (view is TextView textView && _typeface is not null)
            {
                textView.Typeface = _typeface;
                textView.TextSize = 15f;
                textView.Gravity = GravityFlags.CenterVertical | GravityFlags.Right;
            }
        }
    }
}

#endif
