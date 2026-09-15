using MauiPersianToolkit.Core;
using MauiPersianToolkit.Enums;
using MauiPersianToolkit.Extensions;
using MauiPersianToolkit.Models;
using System.Runtime.CompilerServices;

namespace MauiPersianToolkit.Controls;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class DatePicker : PersianInputBase
{
    #region Fields

    private ContentPage _parentPage;
    private DatePickerView _pickerView;
    private Task _initTask;
    private bool _isShowing;

    #endregion

    #region Properties

    public static readonly BindableProperty CalendarOptionProperty = BindableProperty.Create(
        nameof(CalendarOption), typeof(CalendarOptions), typeof(DatePicker),
        defaultValueCreator: static _ => new CalendarOptions());

    public CalendarOptions CalendarOption
    {
        get => (CalendarOptions)GetValue(CalendarOptionProperty);
        set => SetValue(CalendarOptionProperty, value);
    }

    public static readonly BindableProperty SelectedPersianDateProperty = BindableProperty.Create(
        nameof(SelectedPersianDate), typeof(string), typeof(DatePicker),
        default(string), BindingMode.TwoWay);

    public string SelectedPersianDate
    {
        get => (string)GetValue(SelectedPersianDateProperty);
        set => SetValue(SelectedPersianDateProperty, value);
    }

    public static readonly BindableProperty FormattedDateProperty = BindableProperty.Create(
        nameof(FormattedDate), typeof(string), typeof(DatePicker),
        default(string), BindingMode.TwoWay);

    public string FormattedDate
    {
        get => (string)GetValue(FormattedDateProperty);
        set => SetValue(FormattedDateProperty, value);
    }

    public static readonly BindableProperty BadgeDatesProperty = BindableProperty.Create(
        nameof(BadgeDates), typeof(List<string>), typeof(DatePicker),
        defaultValueCreator: static _ => new List<string>());

    public List<string> BadgeDates
    {
        get => (List<string>)GetValue(BadgeDatesProperty);
        set => SetValue(BadgeDatesProperty, value);
    }

    public static readonly BindableProperty DateSeparatorProperty = BindableProperty.Create(
        nameof(DateSeparator), typeof(char), typeof(DatePicker),
        '/', BindingMode.TwoWay);

    public char DateSeparator
    {
        get => (char)GetValue(DateSeparatorProperty);
        set => SetValue(DateSeparatorProperty, value);
    }

    public static readonly BindableProperty DisplayFormatProperty = BindableProperty.Create(
        nameof(DisplayFormat), typeof(string), typeof(DatePicker),
        "yyyy/MM/dd", BindingMode.TwoWay);

    public string DisplayFormat
    {
        get => (string)GetValue(DisplayFormatProperty);
        set => SetValue(DisplayFormatProperty, value);
    }

    public static readonly BindableProperty IsLoadingProperty = BindableProperty.Create(
        nameof(IsLoading), typeof(bool), typeof(DatePicker),
        false, BindingMode.TwoWay);

    public bool IsLoading
    {
        get => (bool)GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }

    public static readonly BindableProperty OnChangeDateCommandProperty = BindableProperty.Create(
        nameof(OnChangeDateCommand), typeof(Command), typeof(DatePicker),
        default(Command), BindingMode.TwoWay);

    public Command OnChangeDateCommand
    {
        get => (Command)GetValue(OnChangeDateCommandProperty);
        set => SetValue(OnChangeDateCommandProperty, value);
    }

    public static readonly BindableProperty OnOpenedCommandProperty = BindableProperty.Create(
        nameof(OnOpenedCommand), typeof(Command), typeof(DatePicker),
        default(Command), BindingMode.TwoWay);

    public Command OnOpenedCommand
    {
        get => (Command)GetValue(OnOpenedCommandProperty);
        set => SetValue(OnOpenedCommandProperty, value);
    }

    #endregion

    public DatePicker()
    {
        InitializeComponent();
        AttachInputChrome(outline);
        AttachGestureRecognizer();
    }

    private void AttachGestureRecognizer()
    {
        var gestureRecognizer = new TapGestureRecognizer
        {
            Command = new Command(OnDatePickerTapped)
        };
        container.GestureRecognizers.Add(gestureRecognizer);
    }

    /// <summary>
    /// Ensures a picker instance exists. Concurrent callers share the same in-flight task
    /// so a second tap never proceeds with a null <see cref="_pickerView"/>.
    /// </summary>
    private Task EnsurePickerReadyAsync()
    {
        if (_pickerView != null)
            return Task.CompletedTask;

        if (_initTask is { IsCompleted: false })
            return _initTask;

        _initTask = CreatePickerViewAsync();
        return _initTask;
    }

    private async Task CreatePickerViewAsync()
    {
        try
        {
            IsLoading = true;

            ConfigureCalendarOptions();
            _pickerView = new DatePickerView(CalendarOption);
            AttachPickerViewEventHandlers();
        }
        catch
        {
            _pickerView = null;
            _initTask = null;
            throw;
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void ConfigureCalendarOptions()
    {
        CalendarOption.SelectedPersianDate = string.IsNullOrWhiteSpace(SelectedPersianDate)
            ? DateTime.Now.ToCalendarDate(CalendarOption.CalendarType)
            : SelectedPersianDate;
        CalendarOption.SelectedPersianDates = BadgeDates;
        CalendarOption.AutoCloseAfterSelectDate = CalendarOption.SelectionMode != Enums.SelectionMode.Multiple
            && CalendarOption.AutoCloseAfterSelectDate;
    }

    private void AttachPickerViewEventHandlers()
    {
        _pickerView.SelectedDateChanged += OnPickerViewSelectedDateChanged;
        _pickerView.Opened += OnPickerViewOpened;
        _pickerView.Closed += OnPickerViewClosed;
    }

    private async void OnPickerViewSelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
    {
        if (CalendarOption.SelectionMode == Enums.SelectionMode.Single)
        {
            SelectedPersianDate = e.SelectedDate.PersianDate.ToString();
            UpdateFormattedDate();
            OnChangeDateCommand?.Execute(SelectedPersianDate);
        }
    }

    private void OnPickerViewOpened(object sender, EventArgs e)
    {
        OnOpenedCommand?.Execute(e);
    }

    private void OnPickerViewClosed(object sender, EventArgs e)
    {
        // Keep the warm DatePickerView instance; only clear the showing guard.
        _isShowing = false;
    }

    #region Event Handlers

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        switch (propertyName)
        {
            case nameof(IsEnabled):
                UpdateVisualStateFromEnabled();
                break;

            case nameof(SelectedPersianDate):
                if (!string.IsNullOrEmpty(SelectedPersianDate))
                {
                    UpdateFormattedDate();
                    _ = EnsurePickerReadyAsync();
                }
                break;
        }
    }

    private void UpdateVisualStateFromEnabled()
    {
        // Chrome state is owned by PersianInputBase; keep placeholder readable when disabled.
        if (!IsEnabled)
            PlaceHolderColor = ThemeColors.Disabled;
    }

    private void ucDatePicker_Loaded(object sender, EventArgs e)
    {
        _ = EnsurePickerReadyAsync();
    }

    #endregion

    #region Private Methods

    private void UpdateFormattedDate()
    {
        if (string.IsNullOrEmpty(SelectedPersianDate))
            return;

        var dateParts = SelectedPersianDate.Replace('/', DateSeparator).Split(DateSeparator);

        if (dateParts.Length == 0)
            return;

        FormattedDate = ApplyYearFormat(DisplayFormat, dateParts);
        FormattedDate = ApplyMonthFormat(FormattedDate, dateParts);
        FormattedDate = ApplyDayFormat(FormattedDate, dateParts);
    }

    private string ApplyYearFormat(string format, string[] dateParts)
    {
        if (dateParts.Length <= 0)
            return format;

        var yearValue = dateParts[0];
        return format
            .Replace("yyyy", yearValue)
            .Replace("yy", yearValue.Length >= 2 ? yearValue.Substring(yearValue.Length - 2) : yearValue);
    }

    private string ApplyMonthFormat(string format, string[] dateParts)
    {
        if (dateParts.Length < 2)
            return format;

        var monthValue = dateParts[1];
        var monthNumber = monthValue.ToInt();

        return format
            .Replace("MMM", Enum.GetName(typeof(PersianMonthNames), monthNumber - 1))
            .Replace("MM", monthValue)
            .Replace("M", monthNumber.ToString());
    }

    private string ApplyDayFormat(string format, string[] dateParts)
    {
        if (dateParts.Length < 3)
            return format;

        var dayValue = dateParts[2];
        var dayNumber = dayValue.ToInt();

        return format
            .Replace("dd", dayValue)
            .Replace("d", dayNumber.ToString())
            .Replace("DD", dayValue);
    }

    private async void OnDatePickerTapped(object sender)
    {
        // Ignore taps while opening or while the popup is already visible (prevents crash on double-tap).
        if (_isShowing)
            return;

        _isShowing = true;
        IsLoading = true;

        try
        {
            await EnsurePickerReadyAsync();

            if (_pickerView == null)
                return;

            ConfigureCalendarOptions();
            _pickerView.Prepare(CalendarOption);

            _parentPage ??= FindParentContentPage();
            if (_parentPage == null)
                return;

            await _parentPage.ShowPopupAsync(_pickerView, new PopupOptions
            {
                Shape = null,
                Shadow = null
            });
        }
        finally
        {
            IsLoading = false;
            _isShowing = false;
        }
    }

    private ContentPage FindParentContentPage()
    {
        var parent = Parent;
        while (parent is not null and not ContentPage)
        {
            parent = parent.Parent;
        }

        return parent as ContentPage;
    }

    #endregion
}