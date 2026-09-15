using MauiPersianToolkit.Localization;
using MauiPersianToolkit.Models;

namespace MauiPersianToolkit.Controls;

/// <summary>
/// Bottom-sheet time picker with snap-scrolling hour/minute/(optional) second wheels.
/// Uses <see cref="ScrollView"/> (not CollectionView) so snap position and animation stay accurate.
/// </summary>
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class TimePickerView : Popup
{
    private const double ItemHeight = 44;
    private const double ViewportHeight = 176;
    private const double WheelSpacer = (ViewportHeight - ItemHeight) / 2.0; // 66 — mirrored in XAML Padding

    private static readonly IReadOnlyList<TimeWheelItem> Hours24 = BuildRange(0, 23);
    private static readonly IReadOnlyList<TimeWheelItem> Hours12 = BuildRange(1, 12);
    private static readonly IReadOnlyList<TimeWheelItem> Seconds = BuildRange(0, 59);
    private static readonly Dictionary<int, IReadOnlyList<TimeWheelItem>> MinutesCache = new();

    private readonly Dictionary<ScrollView, CancellationTokenSource?> _snapTokens = new();
    private readonly Dictionary<ScrollView, double> _lastY = new();
    private readonly Dictionary<ScrollView, IReadOnlyList<TimeWheelItem>> _wheelItems = new();

    private bool _isSnapping;
    private bool _isPm;
    private int _hour;
    private int _minute;
    private int _second;
    private int _minuteInterval = 1;
    private bool _showSeconds;
    private bool _is24Hour = true;
    private int _builtMinuteInterval = -1;
    private bool _hoursBuiltFor24 = true;
    private bool _secondsBuilt;

    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title), typeof(string), typeof(TimePickerView), null,
        defaultValueCreator: static _ => PersianToolkitStrings.SelectTime);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly BindableProperty AcceptTextProperty = BindableProperty.Create(
        nameof(AcceptText), typeof(string), typeof(TimePickerView), null,
        defaultValueCreator: static _ => PersianToolkitStrings.Confirm);

    public string AcceptText
    {
        get => (string)GetValue(AcceptTextProperty);
        set => SetValue(AcceptTextProperty, value);
    }

    public static readonly BindableProperty CancelTextProperty = BindableProperty.Create(
        nameof(CancelText), typeof(string), typeof(TimePickerView), null,
        defaultValueCreator: static _ => PersianToolkitStrings.Cancel);

    public string CancelText
    {
        get => (string)GetValue(CancelTextProperty);
        set => SetValue(CancelTextProperty, value);
    }

    public event EventHandler<TimeSelectedEventArgs>? TimeSelected;

    public TimePickerView()
    {
        InitializeComponent();
        VerticalOptions = LayoutOptions.End;
        HorizontalOptions = LayoutOptions.Fill;
        ApplyMeridiemChrome();
    }

    public void Prepare(TimeSpan time, bool is24Hour, bool showSeconds, int minuteInterval)
    {
        _is24Hour = is24Hour;
        _showSeconds = showSeconds;
        _minuteInterval = Math.Clamp(minuteInterval <= 0 ? 1 : minuteInterval, 1, 30);

        var clamped = ClampTime(time, _minuteInterval, _showSeconds);
        _hour = clamped.Hours;
        _minute = clamped.Minutes;
        _second = clamped.Seconds;
        _isPm = _hour >= 12;

        secondScroll.IsVisible = _showSeconds;
        secondLabel.IsVisible = _showSeconds;
        lblSecond.IsVisible = _showSeconds;
        lblSecondSep.IsVisible = _showSeconds;
        meridiemBar.IsVisible = !_is24Hour;
        lblMeridiem.IsVisible = !_is24Hour;

        ConfigureColumns(_showSeconds);
        EnsureWheelItems();

        ApplyMeridiemChrome();
        RefreshDigital();

        Dispatcher.DispatchAsync(async () =>
        {
            await Task.Delay(50);
            await ScrollWheelToAsync(hourScroll, IndexOfValue(_wheelItems[hourScroll], GetHourWheelValue()), animated: false);
            await ScrollWheelToAsync(minuteScroll, IndexOfValue(_wheelItems[minuteScroll], _minute), animated: false);
            if (_showSeconds)
                await ScrollWheelToAsync(secondScroll, IndexOfValue(_wheelItems[secondScroll], _second), animated: false);
        });
    }

    public TimeSpan DraftTime => new(_hour, _minute, _showSeconds ? _second : 0);

    private void ConfigureColumns(bool showSeconds)
    {
        void Apply(Grid grid, int columns)
        {
            grid.ColumnDefinitions.Clear();
            for (var i = 0; i < columns; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        }

        var cols = showSeconds ? 3 : 2;
        Apply(labelsRow, cols);
        Apply(wheelsGrid, cols);
        Grid.SetColumnSpan(selectionBand, cols);
        if (showSeconds)
        {
            Grid.SetColumn(secondLabel, 2);
            Grid.SetColumn(secondScroll, 2);
        }
    }

    private void EnsureWheelItems()
    {
        if (_hoursBuiltFor24 != _is24Hour || hourStack.Children.Count == 0)
        {
            BuildWheel(hourScroll, hourStack, _is24Hour ? Hours24 : Hours12);
            _hoursBuiltFor24 = _is24Hour;
        }

        if (_builtMinuteInterval != _minuteInterval || minuteStack.Children.Count == 0)
        {
            BuildWheel(minuteScroll, minuteStack, GetMinutes(_minuteInterval));
            _builtMinuteInterval = _minuteInterval;
        }

        if (!_secondsBuilt || secondStack.Children.Count == 0)
        {
            BuildWheel(secondScroll, secondStack, Seconds);
            _secondsBuilt = true;
        }
    }

    private void BuildWheel(ScrollView scroll, VerticalStackLayout stack, IReadOnlyList<TimeWheelItem> items)
    {
        stack.Children.Clear();
        _wheelItems[scroll] = items;

        for (var i = 0; i < items.Count; i++)
        {
            var item = items[i];
            var index = i;
            var row = new Grid
            {
                HeightRequest = ItemHeight,
                Padding = new Thickness(4, 0)
            };
            row.Add(new Label
            {
                Text = item.Label,
                FontFamily = "IranianSans",
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = ThemeColors.OnSurface,
                InputTransparent = true
            });

            var tap = new TapGestureRecognizer();
            tap.Tapped += async (_, _) => await OnWheelItemTappedAsync(scroll, index);
            row.GestureRecognizers.Add(tap);
            stack.Children.Add(row);
        }
    }

    private void OnWheelScrolled(object? sender, ScrolledEventArgs e)
    {
        if (_isSnapping || sender is not ScrollView scroll)
            return;

        _lastY[scroll] = e.ScrollY;

        if (_snapTokens.TryGetValue(scroll, out var existing))
            existing?.Cancel();

        var cts = new CancellationTokenSource();
        _snapTokens[scroll] = cts;
        _ = SnapWhenIdleAsync(scroll, cts.Token);
    }

    private async Task SnapWhenIdleAsync(ScrollView scroll, CancellationToken token)
    {
        try
        {
            // Let the finger/fling fully settle before correcting.
            await Task.Delay(180, token);

            if (!_lastY.TryGetValue(scroll, out var y) || !_wheelItems.TryGetValue(scroll, out var items) || items.Count == 0)
                return;

            var index = Math.Clamp(IndexFromOffset(y), 0, items.Count - 1);
            var targetY = index * ItemHeight;

            ApplyIndex(scroll, index);
            RefreshDigital();

            // Already parked on the band — no animation (avoids the "tick").
            if (Math.Abs(y - targetY) < 1.5)
                return;

            await ScrollWheelToAsync(scroll, index, animated: true);
        }
        catch (TaskCanceledException)
        {
        }
    }

    private async Task OnWheelItemTappedAsync(ScrollView scroll, int index)
    {
        if (_isSnapping)
            return;

        ApplyIndex(scroll, index);
        RefreshDigital();
        await ScrollWheelToAsync(scroll, index, animated: true);
    }

    private async Task ScrollWheelToAsync(ScrollView scroll, int index, bool animated)
    {
        if (!_wheelItems.TryGetValue(scroll, out var items) || items.Count == 0)
            return;

        index = Math.Clamp(index, 0, items.Count - 1);
        var targetY = index * ItemHeight;

        _isSnapping = true;
        try
        {
            // ScrollView.ScrollToAsync(..., animated: true) eases smoothly on all platforms.
            await scroll.ScrollToAsync(0, targetY, animated);
            _lastY[scroll] = targetY;
        }
        finally
        {
            // Brief guard so settle events from the animation don't re-enter snap.
            await Task.Delay(animated ? 50 : 0);
            _isSnapping = false;
        }
    }

    /// <summary>
    /// With top/bottom padding = (viewport - item) / 2, centered item i sits at ScrollY = i * ItemHeight.
    /// </summary>
    private static int IndexFromOffset(double scrollY)
    {
        if (scrollY <= 0)
            return 0;

        // Floor+0.5 bias keeps a stable slot while the finger is near a cell center.
        return Math.Max(0, (int)Math.Floor((scrollY / ItemHeight) + 0.5));
    }

    private static int IndexOfValue(IReadOnlyList<TimeWheelItem> items, int value)
    {
        for (var i = 0; i < items.Count; i++)
        {
            if (items[i].Value == value)
                return i;
        }

        return 0;
    }

    private void ApplyIndex(ScrollView scroll, int index)
    {
        if (!_wheelItems.TryGetValue(scroll, out var items) || items.Count == 0)
            return;

        index = Math.Clamp(index, 0, items.Count - 1);
        var item = items[index];

        if (ReferenceEquals(scroll, hourScroll))
            ApplyHour(item.Value);
        else if (ReferenceEquals(scroll, minuteScroll))
            _minute = item.Value;
        else if (ReferenceEquals(scroll, secondScroll))
            _second = item.Value;
    }

    private void ApplyHour(int wheelValue)
    {
        if (_is24Hour)
        {
            _hour = wheelValue;
            return;
        }

        if (wheelValue == 12)
            _hour = _isPm ? 12 : 0;
        else
            _hour = _isPm ? wheelValue + 12 : wheelValue;
    }

    private void OnAmClicked(object? sender, EventArgs e)
    {
        if (!_isPm)
            return;

        _isPm = false;
        if (_hour >= 12)
            _hour -= 12;
        ApplyMeridiemChrome();
        RefreshDigital();
    }

    private void OnPmClicked(object? sender, EventArgs e)
    {
        if (_isPm)
            return;

        _isPm = true;
        if (_hour < 12)
            _hour += 12;
        ApplyMeridiemChrome();
        RefreshDigital();
    }

    private void OnNowClicked(object? sender, EventArgs e) =>
        Prepare(DateTime.Now.TimeOfDay, _is24Hour, _showSeconds, _minuteInterval);

    private async void OnAcceptClicked(object? sender, EventArgs e)
    {
        CommitFromOffset(hourScroll);
        CommitFromOffset(minuteScroll);
        if (_showSeconds)
            CommitFromOffset(secondScroll);
        RefreshDigital();

        TimeSelected?.Invoke(this, new TimeSelectedEventArgs(DraftTime));
        await CloseAsync();
    }

    private void CommitFromOffset(ScrollView scroll)
    {
        if (_lastY.TryGetValue(scroll, out var y))
            ApplyIndex(scroll, IndexFromOffset(y));
    }

    private async void OnCancelClicked(object? sender, EventArgs e) =>
        await CloseAsync();

    private void RefreshDigital()
    {
        if (_is24Hour)
        {
            lblHour.Text = _hour.ToString("00");
            lblMeridiem.Text = string.Empty;
        }
        else
        {
            var display = _hour % 12;
            if (display == 0)
                display = 12;
            lblHour.Text = display.ToString("00");
            lblMeridiem.Text = _isPm ? PersianToolkitStrings.Pm : PersianToolkitStrings.Am;
        }

        lblMinute.Text = _minute.ToString("00");
        lblSecond.Text = _second.ToString("00");
    }

    private void ApplyMeridiemChrome()
    {
        var accent = ThemeColors.Accent;
        var muted = ThemeColors.Muted;
        var surface = ThemeColors.InputFill;

        btnAm.BackgroundColor = _isPm ? surface : accent;
        btnAm.TextColor = _isPm ? muted : Colors.White;
        btnPm.BackgroundColor = _isPm ? accent : surface;
        btnPm.TextColor = _isPm ? Colors.White : muted;
    }

    private int GetHourWheelValue()
    {
        if (_is24Hour)
            return _hour;

        var display = _hour % 12;
        return display == 0 ? 12 : display;
    }

    private static IReadOnlyList<TimeWheelItem> GetMinutes(int interval)
    {
        if (MinutesCache.TryGetValue(interval, out var cached))
            return cached;

        var list = new List<TimeWheelItem>();
        for (var m = 0; m < 60; m += interval)
            list.Add(new TimeWheelItem(m, m.ToString("00")));

        MinutesCache[interval] = list;
        return list;
    }

    private static IReadOnlyList<TimeWheelItem> BuildRange(int from, int to)
    {
        var list = new List<TimeWheelItem>(to - from + 1);
        for (var i = from; i <= to; i++)
            list.Add(new TimeWheelItem(i, i.ToString("00")));
        return list;
    }

    public static TimeSpan ClampTime(TimeSpan time, int minuteInterval, bool showSeconds)
    {
        var interval = Math.Clamp(minuteInterval <= 0 ? 1 : minuteInterval, 1, 30);
        var totalMinutes = (int)time.TotalMinutes;
        var hours = Math.Clamp(totalMinutes / 60, 0, 23);
        var minutes = totalMinutes % 60;
        minutes = (minutes / interval) * interval;
        var seconds = showSeconds ? Math.Clamp(time.Seconds, 0, 59) : 0;
        return new TimeSpan(hours, minutes, seconds);
    }
}
