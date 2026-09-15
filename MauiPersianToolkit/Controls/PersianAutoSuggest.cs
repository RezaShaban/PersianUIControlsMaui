using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace MauiPersianToolkit.Controls;

/// <summary>
/// Native suggest field: Android <c>AutoCompleteTextView</c>, Windows <c>AutoSuggestBox</c>,
/// iOS <c>UITextField</c> (managed dropdown hosted by <see cref="AutoCompleteView"/>).
/// </summary>
public class PersianAutoSuggest : View
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text), typeof(string), typeof(PersianAutoSuggest), string.Empty, BindingMode.TwoWay,
        propertyChanged: static (b, _, _) => ((PersianAutoSuggest)b).TextChanged?.Invoke(b, EventArgs.Empty));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
        nameof(Placeholder), typeof(string), typeof(PersianAutoSuggest), string.Empty);

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
        nameof(TextColor), typeof(Color), typeof(PersianAutoSuggest), null,
        defaultValueCreator: static _ => ThemeColors.OnSurface);

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(
        nameof(PlaceholderColor), typeof(Color), typeof(PersianAutoSuggest), null,
        defaultValueCreator: static _ => ThemeColors.Muted);

    public Color PlaceholderColor
    {
        get => (Color)GetValue(PlaceholderColorProperty);
        set => SetValue(PlaceholderColorProperty, value);
    }

    public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
        nameof(FontFamily), typeof(string), typeof(PersianAutoSuggest), "IranianSans");

    public string FontFamily
    {
        get => (string)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
        nameof(FontSize), typeof(double), typeof(PersianAutoSuggest), 14d);

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public static readonly BindableProperty ThresholdProperty = BindableProperty.Create(
        nameof(Threshold), typeof(int), typeof(PersianAutoSuggest), 1);

    /// <summary>Native drop-down threshold (Android/Windows).</summary>
    public int Threshold
    {
        get => (int)GetValue(ThresholdProperty);
        set => SetValue(ThresholdProperty, value);
    }

    public static readonly BindableProperty UseNativeSuggestionsProperty = BindableProperty.Create(
        nameof(UseNativeSuggestions), typeof(bool), typeof(PersianAutoSuggest), true);

    public bool UseNativeSuggestions
    {
        get => (bool)GetValue(UseNativeSuggestionsProperty);
        set => SetValue(UseNativeSuggestionsProperty, value);
    }

    public static readonly BindableProperty SuggestionsProperty = BindableProperty.Create(
        nameof(Suggestions), typeof(IList<string>), typeof(PersianAutoSuggest),
        defaultValueCreator: static _ => new ObservableCollection<string>());

    /// <summary>Display strings pushed to the native adapter / AutoSuggestBox.</summary>
    public IList<string> Suggestions
    {
        get => (IList<string>)GetValue(SuggestionsProperty);
        set => SetValue(SuggestionsProperty, value);
    }

    /// <summary>Raised when the user edits text (native or managed).</summary>
    public event EventHandler? TextChanged;

    /// <summary>Raised when a native suggestion row is chosen.</summary>
    public event EventHandler<string>? SuggestionChosen;

    /// <summary>Raised when the native field gains focus.</summary>
    public event EventHandler? FocusedNative;

    /// <summary>Raised when the native field loses focus.</summary>
    public event EventHandler? UnfocusedNative;

    /// <summary>True when the platform handler hosts its own dropdown UI.</summary>
    public bool HasNativeDropdown { get; internal set; }

    public void RaiseSuggestionChosen(string value)
    {
        // Do not assign Text here — AutoCompleteView.SelectItem owns that with suppress flags.
        // Assigning Text would raise TextChanged and re-open the dropdown.
        SuggestionChosen?.Invoke(this, value);
    }

    /// <summary>Closes the platform suggestion list if one is open.</summary>
    public void DismissSuggestions()
    {
        if (Handler is Handlers.PersianAutoSuggestHandler handler)
            handler.DismissSuggestions();
    }

    public void RaiseFocused() => FocusedNative?.Invoke(this, EventArgs.Empty);

    public void RaiseUnfocused() => UnfocusedNative?.Invoke(this, EventArgs.Empty);

    public void RaiseTextChangedFromPlatform(string value)
    {
        if (string.Equals(Text, value, StringComparison.Ordinal))
            return;
        SetValue(TextProperty, value);
        TextChanged?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        Handler?.UpdateValue(propertyName ?? string.Empty);
    }
}
