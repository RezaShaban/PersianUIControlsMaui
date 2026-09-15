using MauiPersianToolkit.Core;
using MauiPersianToolkit.Extensions;
using MauiPersianToolkit.Localization;
using MauiPersianToolkit.Models;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace MauiPersianToolkit.Controls;

/// <summary>
/// Persian-styled time field that opens a reusable bottom-sheet wheel picker.
/// </summary>
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class TimePicker : PersianInputBase
{
    private ContentPage? _parentPage;
    private TimePickerView? _sheet;
    private Task? _initTask;
    private bool _isShowing;
    private bool _hasUserValue;

    public static readonly BindableProperty SelectedTimeProperty = BindableProperty.Create(
        nameof(SelectedTime), typeof(TimeSpan), typeof(TimePicker),
        defaultValueCreator: static _ => TimeSpan.Zero,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: static (b, _, _) => ((TimePicker)b).OnSelectedTimeChanged());

    public TimeSpan SelectedTime
    {
        get => (TimeSpan)GetValue(SelectedTimeProperty);
        set => SetValue(SelectedTimeProperty, value);
    }

    public static readonly BindableProperty FormattedTimeProperty = BindableProperty.Create(
        nameof(FormattedTime), typeof(string), typeof(TimePicker),
        string.Empty, BindingMode.OneWay);

    public string FormattedTime
    {
        get => (string)GetValue(FormattedTimeProperty);
        private set => SetValue(FormattedTimeProperty, value);
    }

    public static readonly BindableProperty DisplayFormatProperty = BindableProperty.Create(
        nameof(DisplayFormat), typeof(string), typeof(TimePicker),
        "HH:mm", BindingMode.OneWay,
        propertyChanged: static (b, _, _) => ((TimePicker)b).UpdateFormattedTime());

    /// <summary>Standard .NET time format (e.g. <c>HH:mm</c>, <c>hh:mm tt</c>, <c>HH:mm:ss</c>).</summary>
    public string DisplayFormat
    {
        get => (string)GetValue(DisplayFormatProperty);
        set => SetValue(DisplayFormatProperty, value);
    }

    public static readonly BindableProperty Is24HourProperty = BindableProperty.Create(
        nameof(Is24Hour), typeof(bool), typeof(TimePicker), true, BindingMode.OneWay);

    public bool Is24Hour
    {
        get => (bool)GetValue(Is24HourProperty);
        set => SetValue(Is24HourProperty, value);
    }

    public static readonly BindableProperty ShowSecondsProperty = BindableProperty.Create(
        nameof(ShowSeconds), typeof(bool), typeof(TimePicker), false, BindingMode.OneWay,
        propertyChanged: static (b, _, _) => ((TimePicker)b).UpdateFormattedTime());

    public bool ShowSeconds
    {
        get => (bool)GetValue(ShowSecondsProperty);
        set => SetValue(ShowSecondsProperty, value);
    }

    public static readonly BindableProperty MinuteIntervalProperty = BindableProperty.Create(
        nameof(MinuteInterval), typeof(int), typeof(TimePicker), 1, BindingMode.OneWay);

    /// <summary>Minute step in the wheel (1, 5, 10, 15, …). Clamped to 1–30.</summary>
    public int MinuteInterval
    {
        get => (int)GetValue(MinuteIntervalProperty);
        set => SetValue(MinuteIntervalProperty, value);
    }

    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title), typeof(string), typeof(TimePicker), null, BindingMode.OneWay,
        defaultValueCreator: static _ => PersianToolkitStrings.SelectTime);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly BindableProperty AcceptTextProperty = BindableProperty.Create(
        nameof(AcceptText), typeof(string), typeof(TimePicker), null, BindingMode.OneWay,
        defaultValueCreator: static _ => PersianToolkitStrings.Confirm);

    public string AcceptText
    {
        get => (string)GetValue(AcceptTextProperty);
        set => SetValue(AcceptTextProperty, value);
    }

    public static readonly BindableProperty CancelTextProperty = BindableProperty.Create(
        nameof(CancelText), typeof(string), typeof(TimePicker), null, BindingMode.OneWay,
        defaultValueCreator: static _ => PersianToolkitStrings.Cancel);

    public string CancelText
    {
        get => (string)GetValue(CancelTextProperty);
        set => SetValue(CancelTextProperty, value);
    }

    public static readonly BindableProperty OnTimeChangedCommandProperty = BindableProperty.Create(
        nameof(OnTimeChangedCommand), typeof(Command), typeof(TimePicker), default(Command));

    public Command OnTimeChangedCommand
    {
        get => (Command)GetValue(OnTimeChangedCommandProperty);
        set => SetValue(OnTimeChangedCommandProperty, value);
    }

    public static readonly BindableProperty OnOpenedCommandProperty = BindableProperty.Create(
        nameof(OnOpenedCommand), typeof(Command), typeof(TimePicker), default(Command));

    public Command OnOpenedCommand
    {
        get => (Command)GetValue(OnOpenedCommandProperty);
        set => SetValue(OnOpenedCommandProperty, value);
    }

    /// <summary>Raised after the user confirms a time in the sheet.</summary>
    public event EventHandler<TimeSelectedEventArgs>? TimeChanged;

    public TimePicker()
    {
        InitializeComponent();
        AttachInputChrome(outline);
        AttachGestureRecognizer();

        if (string.IsNullOrEmpty(Icon))
            Icon = "\uf017";
    }

    private void AttachGestureRecognizer()
    {
        container.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(OnTapped)
        });
    }

    private void OnLoaded(object? sender, EventArgs e) =>
        _ = EnsureSheetReadyAsync();

    private Task EnsureSheetReadyAsync()
    {
        if (_sheet is not null)
            return Task.CompletedTask;

        if (_initTask is { IsCompleted: false })
            return _initTask;

        _initTask = CreateSheetAsync();
        return _initTask;
    }

    private Task CreateSheetAsync()
    {
        _sheet = new TimePickerView();
        _sheet.TimeSelected += OnSheetTimeSelected;
        _sheet.Opened += OnSheetOpened;
        _sheet.Closed += OnSheetClosed;
        return Task.CompletedTask;
    }

    private void OnSheetOpened(object? sender, EventArgs e) =>
        OnOpenedCommand?.Execute(e);

    private void OnSheetClosed(object? sender, EventArgs e) =>
        _isShowing = false;

    private void OnSheetTimeSelected(object? sender, TimeSelectedEventArgs e)
    {
        _hasUserValue = true;
        SelectedTime = e.Time;
        UpdateFormattedTime();
        TimeChanged?.Invoke(this, e);
        OnTimeChangedCommand?.Execute(e.Time);
    }

    private void OnSelectedTimeChanged()
    {
        _hasUserValue = true;
        UpdateFormattedTime();
    }

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == nameof(IsEnabled) && !IsEnabled)
            PlaceHolderColor = ThemeColors.Disabled;
    }

    private void UpdateFormattedTime()
    {
        if (!_hasUserValue && SelectedTime == TimeSpan.Zero && string.IsNullOrEmpty(FormattedTime))
        {
            // Keep empty until the user picks, unless bound SelectedTime was set explicitly later.
            return;
        }

        var format = string.IsNullOrWhiteSpace(DisplayFormat)
            ? (ShowSeconds ? "HH:mm:ss" : "HH:mm")
            : DisplayFormat;

        // Prefer 12-hour tokens when Is24Hour is false and format still uses HH.
        if (!Is24Hour && format.Contains("HH", StringComparison.Ordinal))
            format = format.Replace("HH", "hh", StringComparison.Ordinal);

        FormattedTime = DateTime.Today.Add(SelectedTime)
            .ToString(format, CultureInfo.CurrentCulture);
    }

    private async void OnTapped()
    {
        if (_isShowing || !IsEnabled)
            return;

        _isShowing = true;

        try
        {
            await EnsureSheetReadyAsync();
            if (_sheet is null)
                return;

            _sheet.Title = Title ?? PersianToolkitStrings.SelectTime;
            _sheet.AcceptText = AcceptText ?? PersianToolkitStrings.Confirm;
            _sheet.CancelText = CancelText ?? PersianToolkitStrings.Cancel;

            var seed = _hasUserValue ? SelectedTime : DateTime.Now.TimeOfDay;
            _sheet.Prepare(seed, Is24Hour, ShowSeconds, MinuteInterval);

            _parentPage ??= FindParentContentPage();
            if (_parentPage is null)
                return;

            await _parentPage.ShowPopupAsync(_sheet, new PopupOptions
            {
                Shape = null,
                Shadow = null
            });
        }
        finally
        {
            _isShowing = false;
        }
    }

    private ContentPage? FindParentContentPage()
    {
        var parent = Parent;
        while (parent is not null and not ContentPage)
            parent = parent.Parent;

        return parent as ContentPage;
    }
}
