using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reflection;
using System.Windows.Input;
using MauiPersianToolkit.Enums;
using MauiPersianToolkit.Helpers;
using MauiPersianToolkit.Localization;
using MauiPersianToolkit.Models;

namespace MauiPersianToolkit.Controls;

/// <summary>
/// Persian autocomplete field with native suggest surfaces (Android/Windows) and a
/// polished managed dropdown (iOS / when native suggestions are disabled).
/// </summary>
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class AutoCompleteView : PersianInputBase
{
    private readonly ObservableCollection<AutoCompleteSuggestion> _suggestions = [];
    private CancellationTokenSource? _filterCts;
    private PropertyInfo? _displayInfo;
    private bool _suppressTextSync;
    private bool _selecting;
    private bool _panelOpen;
    private long _suppressRefreshUntil;

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text), typeof(string), typeof(AutoCompleteView), string.Empty, BindingMode.TwoWay,
        propertyChanged: static (b, _, _) => ((AutoCompleteView)b).OnTextBoundChanged());

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
        nameof(ItemsSource), typeof(IEnumerable), typeof(AutoCompleteView), null,
        propertyChanged: static (b, o, n) => ((AutoCompleteView)b).OnItemsSourceChanged(o as IEnumerable, n as IEnumerable));

    public IEnumerable? ItemsSource
    {
        get => (IEnumerable?)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly BindableProperty DisplayPropertyProperty = BindableProperty.Create(
        nameof(DisplayProperty), typeof(string), typeof(AutoCompleteView), default(string));

    public string DisplayProperty
    {
        get => (string)GetValue(DisplayPropertyProperty);
        set => SetValue(DisplayPropertyProperty, value);
    }

    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
        nameof(SelectedItem), typeof(object), typeof(AutoCompleteView), null, BindingMode.TwoWay);

    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public static readonly BindableProperty OptionsProperty = BindableProperty.Create(
        nameof(Options), typeof(AutoCompleteOptions), typeof(AutoCompleteView),
        defaultValueCreator: static _ => new AutoCompleteOptions(),
        propertyChanged: static (b, _, _) => ((AutoCompleteView)b).ApplyOptions());

    public AutoCompleteOptions Options
    {
        get => (AutoCompleteOptions)GetValue(OptionsProperty);
        set => SetValue(OptionsProperty, value);
    }

    public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
        nameof(ItemTemplate), typeof(DataTemplate), typeof(AutoCompleteView), null,
        propertyChanged: static (b, _, n) =>
        {
            if (n is DataTemplate template)
                ((AutoCompleteView)b).suggestionsList.ItemTemplate = template;
        });

    public DataTemplate? ItemTemplate
    {
        get => (DataTemplate?)GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    public static readonly BindableProperty KeyboardProperty = BindableProperty.Create(
        nameof(Keyboard), typeof(Keyboard), typeof(AutoCompleteView), Keyboard.Default);

    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }

    public static readonly BindableProperty ReturnTypeProperty = BindableProperty.Create(
        nameof(ReturnType), typeof(ReturnType), typeof(AutoCompleteView), ReturnType.Done);

    public ReturnType ReturnType
    {
        get => (ReturnType)GetValue(ReturnTypeProperty);
        set => SetValue(ReturnTypeProperty, value);
    }

    public static readonly BindableProperty ReturnCommandProperty = BindableProperty.Create(
        nameof(ReturnCommand), typeof(ICommand), typeof(AutoCompleteView));

    public ICommand? ReturnCommand
    {
        get => (ICommand?)GetValue(ReturnCommandProperty);
        set => SetValue(ReturnCommandProperty, value);
    }

    public static readonly BindableProperty EntryFlowDirectionProperty = BindableProperty.Create(
        nameof(EntryFlowDirection), typeof(FlowDirection), typeof(AutoCompleteView), FlowDirection.RightToLeft);

    public FlowDirection EntryFlowDirection
    {
        get => (FlowDirection)GetValue(EntryFlowDirectionProperty);
        set => SetValue(EntryFlowDirectionProperty, value);
    }

    public static readonly BindableProperty HorizontalTextAlignmentProperty = BindableProperty.Create(
        nameof(HorizontalTextAlignment), typeof(TextAlignment), typeof(AutoCompleteView), TextAlignment.Start);

    public TextAlignment HorizontalTextAlignment
    {
        get => (TextAlignment)GetValue(HorizontalTextAlignmentProperty);
        set => SetValue(HorizontalTextAlignmentProperty, value);
    }

    public static readonly BindableProperty ErrorMessageColorProperty = BindableProperty.Create(
        nameof(ErrorMessageColor), typeof(Color), typeof(AutoCompleteView), Colors.OrangeRed);

    public Color ErrorMessageColor
    {
        get => (Color)GetValue(ErrorMessageColorProperty);
        set => SetValue(ErrorMessageColorProperty, value);
    }

    public static readonly BindableProperty SelectionChangedCommandProperty = BindableProperty.Create(
        nameof(SelectionChangedCommand), typeof(ICommand), typeof(AutoCompleteView));

    public ICommand? SelectionChangedCommand
    {
        get => (ICommand?)GetValue(SelectionChangedCommandProperty);
        set => SetValue(SelectionChangedCommandProperty, value);
    }

    public static readonly BindableProperty TextChangedCommandProperty = BindableProperty.Create(
        nameof(TextChangedCommand), typeof(ICommand), typeof(AutoCompleteView));

    public ICommand? TextChangedCommand
    {
        get => (ICommand?)GetValue(TextChangedCommandProperty);
        set => SetValue(TextChangedCommandProperty, value);
    }

    public event EventHandler<object?>? SelectionChanged;
    public event EventHandler<string>? QueryChanged;

    public AutoCompleteView()
    {
        InitializeComponent();
        AttachInputChrome(outline, suggest);
        suggestionsList.ItemsSource = _suggestions;

        suggest.TextChanged += OnSuggestTextChanged;
        suggest.SuggestionChosen += OnNativeSuggestionChosen;
        suggest.FocusedNative += OnSuggestFocused;
        suggest.UnfocusedNative += OnSuggestUnfocused;

        if (string.IsNullOrEmpty(Icon))
            Icon = "\uf002";

        ApplyOptions();
    }

    private void ApplyOptions()
    {
        var o = Options ?? new AutoCompleteOptions();
        suggest.Threshold = Math.Max(1, o.MinimumPrefixLength);
        suggest.UseNativeSuggestions = o.UseNativeSuggestions;
        suggest.Placeholder = o.FieldPlaceholder ?? PlaceHolder ?? PersianToolkitStrings.TypeToSearch;
        suggestionsList.MaximumHeightRequest = o.MaxDropdownHeight;
        btnClear.IsVisible = o.ShowClearButton && !string.IsNullOrEmpty(Text);
        lblEmpty.Text = o.EmptyText ?? PersianToolkitStrings.NoResults;
    }

    private void OnItemsSourceChanged(IEnumerable? oldValue, IEnumerable? newValue)
    {
        if (oldValue is INotifyCollectionChanged oldObs)
            oldObs.CollectionChanged -= OnItemsCollectionChanged;
        if (newValue is INotifyCollectionChanged newObs)
            newObs.CollectionChanged += OnItemsCollectionChanged;

        _displayInfo = null;
        _ = RefreshSuggestionsAsync();
    }

    private void OnItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) =>
        _ = RefreshSuggestionsAsync();

    private void OnTextBoundChanged()
    {
        if (_suppressTextSync || _selecting || Environment.TickCount64 < _suppressRefreshUntil)
            return;

        if (!string.Equals(suggest.Text, Text, StringComparison.Ordinal))
            suggest.Text = Text ?? string.Empty;

        btnClear.IsVisible = (Options?.ShowClearButton ?? true) && !string.IsNullOrEmpty(Text);
        TextChangedCommand?.Execute(Text);
        QueryChanged?.Invoke(this, Text ?? string.Empty);
        _ = RefreshSuggestionsAsync();
    }

    private void OnSuggestTextChanged(object? sender, EventArgs e)
    {
        if (_suppressTextSync || _selecting || Environment.TickCount64 < _suppressRefreshUntil)
            return;

        _suppressTextSync = true;
        try
        {
            Text = suggest.Text ?? string.Empty;
        }
        finally
        {
            _suppressTextSync = false;
        }

        btnClear.IsVisible = (Options?.ShowClearButton ?? true) && !string.IsNullOrEmpty(Text);
        TextChangedCommand?.Execute(Text);
        QueryChanged?.Invoke(this, Text ?? string.Empty);
        _ = RefreshSuggestionsAsync();
    }

    private void OnNativeSuggestionChosen(object? sender, string value)
    {
        SelectDisplay(value);
    }

    private Color? _idlePlaceholder;

    private async void OnSuggestFocused(object? sender, EventArgs e)
    {
        _idlePlaceholder ??= PlaceHolderColor;
        PlaceHolderColor = ActivePlaceHolderColor;

        if (Options?.OpenOnFocus == true)
            await RefreshSuggestionsAsync(forceOpen: true);
    }

    private async void OnSuggestUnfocused(object? sender, EventArgs e)
    {
        if (_idlePlaceholder is not null)
            PlaceHolderColor = _idlePlaceholder;

        // Allow tap on managed list to register before closing.
        await Task.Delay(160);
        if (!_selecting)
            await SetPanelOpenAsync(false);
    }

    private async void OnClearTapped(object? sender, TappedEventArgs e)
    {
        SelectedItem = null;
        Text = string.Empty;
        suggest.Text = string.Empty;
        _suggestions.Clear();
        PushNativeSuggestionStrings();
        await SetPanelOpenAsync(false);
        suggest.Focus();
    }

    private void OnSuggestionSelected(object? sender, SelectionChangedEventArgs e)
    {
        if (_selecting || e.CurrentSelection.FirstOrDefault() is not AutoCompleteSuggestion row)
            return;

        SelectItem(row.Item, row.Display);
    }

    private void SelectDisplay(string display)
    {
        object? match = null;
        if (ItemsSource is not null)
        {
            foreach (var item in ItemsSource)
            {
                if (item is null)
                    continue;
                var text = AutoCompleteFilter.ResolveDisplay(item, DisplayProperty, ref _displayInfo);
                if (string.Equals(text, display, StringComparison.OrdinalIgnoreCase))
                {
                    match = item;
                    break;
                }
            }
        }

        SelectItem(match ?? display, display);
    }

    private void SelectItem(object? item, string display)
    {
        _selecting = true;
        _suppressRefreshUntil = Environment.TickCount64 + 450;
        _filterCts?.Cancel();
        try
        {
            SelectedItem = item;

            _suggestions.Clear();
            PushNativeSuggestionStrings();
            suggest.DismissSuggestions();

            _suppressTextSync = true;
            Text = display;
            suggest.Text = display;
            _suppressTextSync = false;

            SelectionChanged?.Invoke(this, item);
            SelectionChangedCommand?.Execute(item);

            if (Options?.CloseOnSelect != false)
                _ = SetPanelOpenAsync(false);
        }
        finally
        {
            _selecting = false;
            btnClear.IsVisible = (Options?.ShowClearButton ?? true) && !string.IsNullOrEmpty(Text);
        }
    }

    private async Task RefreshSuggestionsAsync(bool forceOpen = false)
    {
        if (_selecting || Environment.TickCount64 < _suppressRefreshUntil)
            return;

        _filterCts?.Cancel();
        var cts = new CancellationTokenSource();
        _filterCts = cts;
        var token = cts.Token;

        var options = Options ?? new AutoCompleteOptions();
        var query = (Text ?? string.Empty).Trim();

        if (query.Length < Math.Max(0, options.MinimumPrefixLength) && !forceOpen)
        {
            _suggestions.Clear();
            PushNativeSuggestionStrings();
            await SetPanelOpenAsync(false);
            return;
        }

        if (forceOpen && query.Length < options.MinimumPrefixLength)
            query = Text ?? string.Empty;

        var debounce = Math.Max(0, options.FilterDebounceMs);
        if (debounce > 0)
        {
            try { await Task.Delay(debounce, token); }
            catch (TaskCanceledException) { return; }
        }

        SetBusy(true);
        try
        {
            IReadOnlyList<object> matches;
            if (options.SuggestionProvider is not null)
            {
                matches = await options.SuggestionProvider(query, token).ConfigureAwait(true);
            }
            else
            {
                var comparison = options.IgnoreCase
                    ? StringComparison.OrdinalIgnoreCase
                    : StringComparison.Ordinal;
                matches = AutoCompleteFilter.Filter(
                    ItemsSource,
                    query,
                    DisplayProperty,
                    options.FilterMode,
                    options.MaxSuggestions,
                    comparison,
                    options.CustomFilter);
            }

            if (token.IsCancellationRequested)
                return;

            _suggestions.Clear();
            foreach (var item in matches)
            {
                var display = AutoCompleteFilter.ResolveDisplay(item, DisplayProperty, ref _displayInfo);
                _suggestions.Add(new AutoCompleteSuggestion(item, display));
            }

            PushNativeSuggestionStrings();

            var useManaged = !suggest.HasNativeDropdown || !options.UseNativeSuggestions;
            lblEmpty.IsVisible = useManaged && _suggestions.Count == 0 && query.Length >= options.MinimumPrefixLength;
            suggestionsList.IsVisible = _suggestions.Count > 0;
            lblEmpty.Text = options.EmptyText ?? PersianToolkitStrings.NoResults;

            if (useManaged && (_suggestions.Count > 0 || lblEmpty.IsVisible))
                await SetPanelOpenAsync(true);
            else if (!useManaged)
                await SetPanelOpenAsync(false);
            else
                await SetPanelOpenAsync(false);
        }
        catch (TaskCanceledException)
        {
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void PushNativeSuggestionStrings()
    {
        var list = _suggestions.Select(s => s.Display).ToList();
        suggest.Suggestions = list;
    }

    private void SetBusy(bool busy)
    {
        busyIndicator.IsVisible = busy;
        busyIndicator.IsRunning = busy;
    }

    private async Task SetPanelOpenAsync(bool open)
    {
        if (_panelOpen == open)
        {
            suggestionsPanel.IsVisible = open || suggestionsPanel.Opacity > 0;
            return;
        }

        _panelOpen = open;
        if (open)
        {
            suggestionsPanel.IsVisible = true;
            suggestionsPanel.Opacity = 0;
            suggestionsPanel.TranslationY = -6;
            await Task.WhenAll(
                suggestionsPanel.FadeToAsync(1, 140, Easing.CubicOut),
                suggestionsPanel.TranslateToAsync(0, 0, 160, Easing.CubicOut));
        }
        else
        {
            await Task.WhenAll(
                suggestionsPanel.FadeToAsync(0, 100, Easing.CubicIn),
                suggestionsPanel.TranslateToAsync(0, -4, 100, Easing.CubicIn));
            suggestionsPanel.IsVisible = false;
            suggestionsList.SelectedItem = null;
        }
    }
}
