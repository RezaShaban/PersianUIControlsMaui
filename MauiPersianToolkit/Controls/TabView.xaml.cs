using System.Runtime.CompilerServices;

namespace MauiPersianToolkit.Controls;

/// <summary>
/// Bottom tab strip with lazy page content, a sliding indicator, swipe navigation, and light motion.
/// </summary>
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class TabView : ContentView
{
    private sealed class TabChrome
    {
        public required TabItemView Item { get; init; }
        public required Button IconButton { get; init; }
        public required Label Caption { get; init; }
        public int Column { get; init; }
    }

    private readonly List<TabChrome> _tabs = [];
    private readonly HashSet<TabItemView> _materializedTabs = [];
    private bool _isGeneratedTabs;
    private bool _isAnimating;
    private int _currentIndex = -1;
    private double _panTotalX;
    private bool _panHandled;

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
        nameof(ItemsSource), typeof(List<TabItemView>), typeof(TabView),
        defaultBindingMode: BindingMode.TwoWay,
        defaultValueCreator: static _ => new List<TabItemView>());

    public List<TabItemView> ItemsSource
    {
        get => (List<TabItemView>)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly BindableProperty UnSelectedTabColorProperty = BindableProperty.Create(
        nameof(UnSelectedTabColor), typeof(Color), typeof(TabView), Color.FromArgb("#9AA0A6"), BindingMode.TwoWay);

    public Color UnSelectedTabColor
    {
        get => (Color)GetValue(UnSelectedTabColorProperty);
        set => SetValue(UnSelectedTabColorProperty, value);
    }

    public static readonly BindableProperty SelectedTabColorProperty = BindableProperty.Create(
        nameof(SelectedTabColor), typeof(Color), typeof(TabView), null, BindingMode.TwoWay,
        defaultValueCreator: static _ => ThemeColors.Accent);

    public Color SelectedTabColor
    {
        get => (Color)GetValue(SelectedTabColorProperty);
        set => SetValue(SelectedTabColorProperty, value);
    }

    public static readonly BindableProperty IndicatorColorProperty = BindableProperty.Create(
        nameof(IndicatorColor), typeof(Color), typeof(TabView), null, BindingMode.TwoWay,
        defaultValueCreator: static _ => ThemeColors.Accent);

    /// <summary>Color of the sliding pill under the selected tab.</summary>
    public Color IndicatorColor
    {
        get => (Color)GetValue(IndicatorColorProperty);
        set => SetValue(IndicatorColorProperty, value);
    }

    public static readonly BindableProperty SelectedTabProperty = BindableProperty.Create(
        nameof(SelectedTab), typeof(int), typeof(TabView), default(int), BindingMode.TwoWay,
        propertyChanged: static (b, o, n) => ((TabView)b).OnSelectedTabChanged(o, n));

    public int SelectedTab
    {
        get => (int)GetValue(SelectedTabProperty);
        set => SetValue(SelectedTabProperty, value);
    }

    public static readonly BindableProperty AnimateCaptionsProperty = BindableProperty.Create(
        nameof(AnimateCaptions), typeof(bool), typeof(TabView), true, BindingMode.TwoWay);

    /// <summary>When true, captions fade/scale with selection (Material-style labeled tabs).</summary>
    public bool AnimateCaptions
    {
        get => (bool)GetValue(AnimateCaptionsProperty);
        set => SetValue(AnimateCaptionsProperty, value);
    }

    public static readonly BindableProperty EnableAnimationsProperty = BindableProperty.Create(
        nameof(EnableAnimations), typeof(bool), typeof(TabView), true, BindingMode.TwoWay);

    /// <summary>Master switch for indicator slide, content cross-fade, and icon pulse.</summary>
    public bool EnableAnimations
    {
        get => (bool)GetValue(EnableAnimationsProperty);
        set => SetValue(EnableAnimationsProperty, value);
    }

    public static readonly BindableProperty EnableSwipeProperty = BindableProperty.Create(
        nameof(EnableSwipe), typeof(bool), typeof(TabView), true, BindingMode.OneWay);

    /// <summary>When true, horizontal swipe on tab content changes the selected tab.</summary>
    public bool EnableSwipe
    {
        get => (bool)GetValue(EnableSwipeProperty);
        set => SetValue(EnableSwipeProperty, value);
    }

    public static readonly BindableProperty BarHeightProperty = BindableProperty.Create(
        nameof(BarHeight), typeof(double), typeof(TabView), 64d, BindingMode.OneWay);

    public double BarHeight
    {
        get => (double)GetValue(BarHeightProperty);
        set => SetValue(BarHeightProperty, value);
    }

    public static readonly BindableProperty ChangedTabCommandProperty = BindableProperty.Create(
        nameof(ChangedTabCommand), typeof(Command), typeof(TabView), default(Command), BindingMode.TwoWay);

    public Command ChangedTabCommand
    {
        get => (Command)GetValue(ChangedTabCommandProperty);
        set => SetValue(ChangedTabCommandProperty, value);
    }

    public TabView()
    {
        PersianTheme.SeedControlResources(Resources);
        InitializeComponent();
        AttachSwipeGestures();
    }

    private void AttachSwipeGestures()
    {
        var pan = new PanGestureRecognizer();
        pan.PanUpdated += OnContentPanUpdated;
        contentHost.GestureRecognizers.Add(pan);
    }

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName is nameof(ItemsSource) or "Renderer" or "Window")
            GenerateTabPages();
    }

    private void OnSelectedTabChanged(object oldValue, object newValue)
    {
        if (!_isGeneratedTabs || _tabs.Count == 0)
            return;

        if (newValue is not int index || index < 0 || index >= _tabs.Count)
            return;

        if (index == _currentIndex)
            return;

        _ = SelectTabAsync(_tabs[index], animate: EnableAnimations);
    }

    private void TabBar_SizeChanged(object? sender, EventArgs e) =>
        ScheduleIndicatorSync();

    private void ChromeHost_SizeChanged(object? sender, EventArgs e) =>
        ScheduleIndicatorSync();

    private void ScheduleIndicatorSync()
    {
        if (_currentIndex < 0 || _tabs.Count == 0)
            return;

        // Android often reports Width=0 on the first SizeChanged; retry after layout settles.
        Dispatcher.DispatchAsync(async () =>
        {
            for (var attempt = 0; attempt < 3; attempt++)
            {
                await Task.Delay(attempt == 0 ? 16 : 32);
                if (_currentIndex < 0)
                    return;
                if (chromeHost.Width > 0)
                {
                    await MoveIndicatorAsync(_currentIndex, animate: false);
                    return;
                }
            }
        });
    }

    private void GenerateTabPages()
    {
        try
        {
            if (ItemsSource is null || ItemsSource.Count == 0 || _isGeneratedTabs || contentHost is null)
                return;

            _isGeneratedTabs = true;
            _tabs.Clear();
            tabButtons.Children.Clear();
            tabButtons.ColumnDefinitions.Clear();

            var visibleItems = ItemsSource.Where(x => x.IsVisible).ToList();
            if (visibleItems.Count == 0)
                return;

            for (var i = 0; i < visibleItems.Count; i++)
            {
                var item = visibleItems[i];

                tabButtons.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });

                var iconButton = new Button
                {
                    IsEnabled = item.IsEnabled,
                    Text = item.Icon,
                    FontFamily = "FontAwesome",
                    FontSize = 22,
                    Padding = new Thickness(0, AnimateCaptions ? 6 : 14, 0, AnimateCaptions ? 18 : 8),
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill,
                    TextColor = UnSelectedTabColor,
                    BackgroundColor = Colors.Transparent,
                    BorderWidth = 0
                };

                var caption = new Label
                {
                    IsEnabled = item.IsEnabled,
                    Text = item.Title,
                    FontSize = 11,
                    FontFamily = "IranianSans",
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    InputTransparent = true,
                    TextColor = UnSelectedTabColor,
                    Margin = new Thickness(0, 0, 0, 6),
                    Opacity = AnimateCaptions ? 0 : 1,
                    Scale = AnimateCaptions ? 0.85 : 1
                };

                // Reposition indicator when this tab finishes layout.
                iconButton.SizeChanged += (_, _) =>
                {
                    if (_currentIndex == i)
                        ScheduleIndicatorSync();
                };

                var chrome = new TabChrome
                {
                    Item = item,
                    IconButton = iconButton,
                    Caption = caption,
                    Column = i
                };

                iconButton.Command = new Command(() => _ = SelectTabAsync(chrome, animate: EnableAnimations));

                tabButtons.Add(iconButton, i, 0);
                tabButtons.Add(caption, i, 0);
                _tabs.Add(chrome);
            }

            var initial = Math.Clamp(SelectedTab, 0, _tabs.Count - 1);
            _ = SelectTabAsync(_tabs[initial], animate: false);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"TabView.GenerateTabPages failed: {ex}");
        }
    }

    private void EnsureMaterialized(TabItemView tab)
    {
        if (!_materializedTabs.Add(tab))
            return;

        tab.Opacity = 0;
        tab.IsVisible = false;
        contentHost.Add(tab);
    }

    private void OnContentPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        if (!EnableSwipe || _isAnimating || _tabs.Count < 2)
            return;

        switch (e.StatusType)
        {
            case GestureStatus.Started:
                _panTotalX = 0;
                _panHandled = false;
                break;

            case GestureStatus.Running:
                _panTotalX = e.TotalX;
                break;

            case GestureStatus.Completed:
            case GestureStatus.Canceled:
                if (_panHandled)
                    return;

                const double threshold = 48;
                if (Math.Abs(_panTotalX) < threshold)
                    return;

                _panHandled = true;
                // Finger moved left (negative TotalX) → show next in LTR.
                var direction = _panTotalX < 0 ? SwipeDirection.Left : SwipeDirection.Right;
                _ = MoveBySwipeAsync(direction);
                break;
        }
    }

    private Task MoveBySwipeAsync(SwipeDirection direction)
    {
        if (!EnableSwipe || _isAnimating || _tabs.Count == 0)
            return Task.CompletedTask;

        var delta = ResolveSwipeDelta(direction);
        var next = _currentIndex + delta;
        if (next < 0 || next >= _tabs.Count)
            return Task.CompletedTask;

        return SelectTabAsync(_tabs[next], animate: EnableAnimations);
    }

    private int ResolveSwipeDelta(SwipeDirection direction)
    {
        // Match natural reading direction: in RTL, swipe left goes to the previous index.
        var rtl = IsEffectivelyRtl();
        return direction switch
        {
            SwipeDirection.Left => rtl ? -1 : 1,
            SwipeDirection.Right => rtl ? 1 : -1,
            _ => 0
        };
    }

    private bool IsEffectivelyRtl()
    {
        if (FlowDirection == FlowDirection.RightToLeft)
            return true;

        for (var parent = Parent; parent is not null; parent = parent.Parent)
        {
            if (parent is VisualElement ve && ve.FlowDirection == FlowDirection.RightToLeft)
                return true;
        }

        return false;
    }

    private async Task SelectTabAsync(TabChrome chrome, bool animate)
    {
        if (_isAnimating && animate)
            return;

        var nextIndex = chrome.Column;
        if (nextIndex == _currentIndex && chrome.Item.IsVisible)
        {
            await MoveIndicatorAsync(nextIndex, animate: false);
            return;
        }

        _isAnimating = true;
        try
        {
            var previousIndex = _currentIndex;
            var direction = nextIndex >= previousIndex ? 1 : -1;

            EnsureMaterialized(chrome.Item);

            TabItemView? previousPage = previousIndex >= 0 && previousIndex < _tabs.Count
                ? _tabs[previousIndex].Item
                : null;

            foreach (var tab in _tabs)
            {
                tab.IconButton.TextColor = UnSelectedTabColor;
                tab.Caption.TextColor = UnSelectedTabColor;
                if (AnimateCaptions && animate)
                {
                    _ = tab.Caption.FadeToAsync(0, 90);
                    _ = tab.Caption.ScaleToAsync(0.85, 90);
                    tab.IconButton.Padding = new Thickness(0, 6, 0, 18);
                }
                else if (AnimateCaptions)
                {
                    tab.Caption.Opacity = 0;
                    tab.Caption.Scale = 0.85;
                    tab.IconButton.Padding = new Thickness(0, 6, 0, 18);
                }
            }

            if (previousPage is not null && !ReferenceEquals(previousPage, chrome.Item))
            {
                if (animate)
                {
                    await Task.WhenAll(
                        previousPage.FadeToAsync(0, 120),
                        previousPage.TranslateToAsync(-direction * 28, 0, 120, Easing.CubicIn));
                }

                previousPage.IsVisible = false;
                previousPage.TranslationX = 0;
                previousPage.Opacity = 1;
            }

            chrome.Item.TranslationX = animate ? direction * 32 : 0;
            chrome.Item.Opacity = animate ? 0 : 1;
            chrome.Item.IsVisible = true;

            chrome.IconButton.TextColor = SelectedTabColor;
            chrome.Caption.TextColor = SelectedTabColor;
            if (AnimateCaptions)
                chrome.IconButton.Padding = new Thickness(0, 4, 0, 20);

            indicator.BackgroundColor = IndicatorColor ?? SelectedTabColor;

            var motion = new List<Task>
            {
                MoveIndicatorAsync(nextIndex, animate)
            };

            if (animate)
            {
                motion.Add(chrome.Item.FadeToAsync(1, 180));
                motion.Add(chrome.Item.TranslateToAsync(0, 0, 180, Easing.CubicOut));
                motion.Add(PulseIconAsync(chrome.IconButton));

                if (AnimateCaptions)
                {
                    motion.Add(chrome.Caption.FadeToAsync(1, 160));
                    motion.Add(chrome.Caption.ScaleToAsync(1, 160, Easing.CubicOut));
                }
            }
            else
            {
                chrome.Item.Opacity = 1;
                chrome.Item.TranslationX = 0;
                chrome.Caption.Opacity = 1;
                chrome.Caption.Scale = 1;
            }

            await Task.WhenAll(motion);

            _currentIndex = nextIndex;
            if (SelectedTab != nextIndex)
                SelectedTab = nextIndex;

            // Final sync after layout settles (caption padding changes width slightly).
            ScheduleIndicatorSync();

            ChangedTabCommand?.Execute(chrome.Item.Key);
        }
        finally
        {
            _isAnimating = false;
        }
    }

    private static async Task PulseIconAsync(Button icon)
    {
        await icon.ScaleToAsync(1.12, 90, Easing.CubicOut);
        await icon.ScaleToAsync(1.0, 110, Easing.CubicIn);
    }

    private async Task MoveIndicatorAsync(int column, bool animate)
    {
        if (column < 0 || column >= _tabs.Count)
            return;

        var hostWidth = chromeHost.Width;
        if (hostWidth <= 0 || _tabs.Count == 0)
        {
            indicator.Opacity = 0;
            return;
        }

        // Equal * columns: derive physical left from index (do not use View.X — unreliable on Android RTL).
        var cellWidth = hostWidth / _tabs.Count;
        var visualIndex = IsEffectivelyRtl() ? (_tabs.Count - 1 - column) : column;
        var cellLeft = visualIndex * cellWidth;
        var pillWidth = Math.Clamp(cellWidth * 0.36, 24, 48);
        var targetX = cellLeft + ((cellWidth - pillWidth) / 2.0);

        indicator.WidthRequest = pillWidth;
        indicator.BackgroundColor = IndicatorColor ?? SelectedTabColor;
        indicator.Opacity = 1;

        // Keep AbsoluteLayout bounds at origin; slide with TranslationX in the LTR overlay.
        AbsoluteLayout.SetLayoutBounds(indicator, new Rect(0, 2, pillWidth, 3));

        if (animate)
            await indicator.TranslateToAsync(targetX, 0, 240, Easing.CubicInOut);
        else
        {
            indicator.CancelAnimations();
            indicator.TranslationX = targetX;
            indicator.TranslationY = 0;
        }
    }
}
