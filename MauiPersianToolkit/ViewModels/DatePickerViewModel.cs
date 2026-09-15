using MauiPersianToolkit.Enums;
using MauiPersianToolkit.Extensions;
using MauiPersianToolkit.Models;
using MauiPersianToolkit.Services.Calendar;
using System.Collections.ObjectModel;

namespace MauiPersianToolkit.ViewModels;

public class DatePickerViewModel : ObservableObject
{
    #region Fields

    private ICalendarService _calendarService;
    private ObservableCollection<PuiTuple>? _cachedYears;
    private ObservableCollection<PuiTuple>? _cachedMonths;
    private CalendarType? _yearsCacheType;
    private CalendarType? _monthsCacheType;

    #endregion

    #region Properties

    private int currentYear;
    private string currentMonth;
    private SelectionDateMode selectDateMode;
    private CalendarOptions options;

    public int CurrentYear { get => currentYear; set => SetProperty(ref currentYear, value); }
    public string CurrentMonth { get => currentMonth; set => SetProperty(ref currentMonth, value); }
    public SelectionDateMode SelectDateMode { get => selectDateMode; set => SetProperty(ref selectDateMode, value); }
    public CalendarOptions Options { get => options; set => SetProperty(ref options, value); }

    #endregion

    #region Collections

    private List<string> daysOfWeek;
    private List<DayOfMonth> daysOfMonth;
    private ObservableCollection<PuiTuple> persianMonths;
    private ObservableCollection<PuiTuple> persianYears;
    private ObservableCollection<DayOfMonth> selectedDays;

    public List<string> DaysOfWeek { get => daysOfWeek; set => SetProperty(ref daysOfWeek, value); }
    public List<DayOfMonth> DaysOfMonth { get => daysOfMonth; set => SetProperty(ref daysOfMonth, value); }
    public ObservableCollection<DayOfMonth> SelectedDays
    {
        get => selectedDays;
        set
        {
            if (selectedDays is not null)
                selectedDays.CollectionChanged -= OnSelectedDaysChanged;

            if (SetProperty(ref selectedDays, value) && selectedDays is not null)
                selectedDays.CollectionChanged += OnSelectedDaysChanged;

            OnPropertyChanged(nameof(CanAccept));
        }
    }

    /// <summary>
    /// Whether the confirm button may be pressed. Range selection needs both ends;
    /// single and multiple need at least one day.
    /// </summary>
    public bool CanAccept => Options.SelectionMode switch
    {
        Enums.SelectionMode.Range => SelectedDays.Count == 2,
        Enums.SelectionMode.Multiple => SelectedDays.Count > 0,
        _ => SelectedDays.Count > 0
    };

    public ObservableCollection<PuiTuple> PersianMonths { get => persianMonths; set => SetProperty(ref persianMonths, value); }
    public ObservableCollection<PuiTuple> PersianYears { get => persianYears; set => SetProperty(ref persianYears, value); }

    #endregion

    #region Commands

    public Command NextMonthCommand { get; }
    public Command PrevMonthCommand { get; }
    public Command NextYearCommand { get; }
    public Command PrevYearCommand { get; }
    public Command SwitchModeCommand { get; }
    public Command SelectMonthCommand { get; }
    public Command SelectYearCommand { get; }
    public Command GotoTodayCommand { get; }
    public Command InitCalendarDaysCommand { get; }
    public Command SelectDateCommand { get; }

    #endregion

    public DatePickerViewModel(CalendarOptions options)
    {
        NextMonthCommand = new Command(NextMonth);
        PrevMonthCommand = new Command(PrevMonth);
        NextYearCommand = new Command(NextYear);
        PrevYearCommand = new Command(PrevYear);
        SwitchModeCommand = new Command(SwitchMode);
        SelectMonthCommand = new Command(SelectMonth);
        SelectYearCommand = new Command(SelectYear);
        GotoTodayCommand = new Command(GotoToday);
        InitCalendarDaysCommand = new Command(InitCalendarDays);
        SelectDateCommand = new Command(SelectDate);

        ApplyOptions(options, rebuildWeekHeaders: true);
    }

    /// <summary>
    /// Rebinds options onto a warm view-model without reallocating commands or year lists
    /// when the calendar type is unchanged.
    /// </summary>
    public void Refresh(CalendarOptions options) =>
        ApplyOptions(options, rebuildWeekHeaders: true);

    private void ApplyOptions(CalendarOptions options, bool rebuildWeekHeaders)
    {
        Options = options;
        _calendarService = CalendarServiceFactory.GetService(options.CalendarType);

        SelectedDays = new ObservableCollection<DayOfMonth>(GetSelectedDates(options.SelectedPersianDates));
        SelectDateMode = options.SelectDateMode;

        if (rebuildWeekHeaders || DaysOfWeek is null)
            DaysOfWeek = FillDaysOfWeek();

        InitCalendarDays(_calendarService.ToGregorianDate(options.SelectedPersianDate));
    }

    private void OnSelectedDaysChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) =>
        OnPropertyChanged(nameof(CanAccept));

    private IEnumerable<DayOfMonth> GetSelectedDates(List<string> selectedDates) =>
        [.. (selectedDates ?? []).Select(x =>
        {
            var currentDate = _calendarService.ToGregorianDate(x);
            return CreateDayOfMonth(currentDate, true, false, false);
        })];

    private List<string> FillDaysOfWeek()
    {
        var firstDayOfWeek = (int)_calendarService.GetFirstDayOfWeek();
        return Enumerable.Range(0, 7)
            .Select(i => _calendarService.GetDayOfWeekName((DayOfWeek)((firstDayOfWeek + i) % 7)))
            .ToList();
    }

    private void InitCalendarDays(object obj)
    {
        if (obj is not DateTime date) return;

        CurrentMonth = _calendarService.GetMonthName(_calendarService.GetMonth(date));
        CurrentYear = _calendarService.GetYear(date);
        PersianMonths = GetMonths();
        PersianYears = GetYears();

        var monthBeginningStr = _calendarService.GetMonthBeginning(date);
        var monthEndingStr = _calendarService.GetMonthEnding(date);

        var firstDayOfMonth = _calendarService.ToGregorianDate(monthBeginningStr);
        var endDayOfMonth = _calendarService.ToGregorianDate(monthEndingStr);

        var firstDayOfWeek = (int)_calendarService.GetFirstDayOfWeek();
        var startDayOffset = ((int)_calendarService.GetDayOfWeek(firstDayOfMonth) - firstDayOfWeek + 7) % 7;
        var daysInMonth = (int)(endDayOfMonth - firstDayOfMonth).TotalDays + 1;

        var monthDaysCount = Enumerable.Range(-startDayOffset, daysInMonth + startDayOffset).ToList();
        DaysOfMonth = GetDaysOfMonth(monthDaysCount, firstDayOfMonth, date);
    }

    private List<DayOfMonth> GetDaysOfMonth(List<int> monthDaysCount, DateTime firstDayOfMonth, DateTime date)
    {
        var currentMonth = _calendarService.GetMonth(date);
        var currentYear = _calendarService.GetYear(date);

        return [.. monthDaysCount.Select(offset =>
        {
            var currentDate = firstDayOfMonth.AddDays(offset);
            var isInCurrentMonth = _calendarService.GetMonth(currentDate) == currentMonth
                && _calendarService.GetYear(currentDate) == currentYear;
            return CreateDayOfMonth(currentDate, GetIsSelected(currentDate, date), GetIsInRange(currentDate), isInCurrentMonth);
        })];
    }

    private DayOfMonth CreateDayOfMonth(DateTime currentDate, bool isSelected, bool isInRange, bool isInCurrentMonth)
    {
        var dayNum = _calendarService.GetDayOfMonth(currentDate);
        var calendarDate = _calendarService.ToCalendarDate(currentDate);
        var dateNo = calendarDate.Replace("/", "").ToInt();
        var dayOfWeek = _calendarService.GetDayOfWeek(currentDate);
        var isHoliday = dayOfWeek == _calendarService.GetLastDayOfWeek();

        return new()
        {
            DayNum = dayNum,
            GregorianDate = currentDate,
            PersianDate = calendarDate,
            PersianDateNo = dateNo,
            IsSelected = isSelected,
            IsInRange = isInRange,
            IsInCurrentMonth = isInCurrentMonth,
            IsHoliday = isHoliday,
            IsToday = currentDate.Date == DateTime.Now.Date,
            DayOfWeek = (PersianDayOfWeek)dayOfWeek,
            CanSelect = GetCanSelect(currentDate),
        };
    }

    private ObservableCollection<PuiTuple> GetYears()
    {
        if (_cachedYears is not null && _yearsCacheType == Options.CalendarType)
            return _cachedYears;

        var currentYear = _calendarService.GetYear(DateTime.Now);
        _cachedYears = Enumerable.Range(currentYear - 100, 150)
            .Select(year => new PuiTuple(year.ToString(), year.ToString()))
            .ToObservableCollection();
        _yearsCacheType = Options.CalendarType;
        return _cachedYears;
    }

    private ObservableCollection<PuiTuple> GetMonths()
    {
        if (_cachedMonths is not null && _monthsCacheType == Options.CalendarType)
            return _cachedMonths;

        var monthNames = _calendarService.GetAllMonthNames();
        _cachedMonths = Enumerable.Range(1, _calendarService.GetMonthsInYear())
            .Select((i, index) => new PuiTuple(i.ToString(), monthNames.ElementAtOrDefault(index) ?? i.ToString()))
            .ToObservableCollection();
        _monthsCacheType = Options.CalendarType;
        return _cachedMonths;
    }

    private void SelectDate(object obj)
    {
        if (obj is not DayOfMonth dayOfMonth || !dayOfMonth.CanSelect) return;

        switch (Options.SelectionMode)
        {
            case Enums.SelectionMode.Single:
                DaysOfMonth.ForEach(day => day.IsSelected = day.PersianDate == dayOfMonth.PersianDate);
                SelectedDays.Clear();
                SelectedDays.Add(dayOfMonth);
                break;
            case Enums.SelectionMode.Multiple:
                ToggleMultipleDates(dayOfMonth);
                break;
            case Enums.SelectionMode.Range:
                HandleRangeSelection(dayOfMonth);
                break;
        }
    }

    private void HandleRangeSelection(DayOfMonth dayOfMonth)
    {
        if (dayOfMonth.PersianDateNo <= SelectedDays.FirstOrDefault()?.PersianDateNo)
        {
            ResetSelection();
        }
        ToggleRangeDates(dayOfMonth);
    }

    private void ResetSelection()
    {
        DaysOfMonth.ForEach(day =>
        {
            day.IsSelected = false;
            day.IsInRange = false;
            day.CanSelect = GetCanSelect(day.GregorianDate);
        });
        SelectedDays.Clear();
    }

    private void ToggleRangeDates(DayOfMonth dayOfMonth)
    {
        if (SelectedDays.Count == 2)
        {
            ResetSelection();
        }

        var selectedDate = DaysOfMonth.FirstOrDefault(day => day.PersianDateNo == dayOfMonth.PersianDateNo);
        if (selectedDate == null) return;

        selectedDate.IsSelected = !selectedDate.IsSelected;
        if (selectedDate.IsSelected)
        {
            SelectedDays.Add(selectedDate);
            if (SelectedDays.Count == 2)
            {
                DaysOfMonth.Where(day => day.PersianDateNo > SelectedDays[0].PersianDateNo && day.PersianDateNo < SelectedDays[1].PersianDateNo)
                           .ToList()
                           .ForEach(day => day.IsInRange = true);
            }
        }
    }

    private void ToggleMultipleDates(DayOfMonth dayOfMonth)
    {
        var selectedDate = DaysOfMonth.FirstOrDefault(day => day.PersianDateNo == dayOfMonth.PersianDateNo);
        if (selectedDate == null) return;

        selectedDate.IsSelected = !selectedDate.IsSelected;
        if (selectedDate.IsSelected)
        {
            SelectedDays.Add(selectedDate);
        }
        else
        {
            SelectedDays.Remove(selectedDate);
        }
    }

    private bool GetCanSelect(DateTime currentDate) =>
        (Options.MinDateCanSelect == null || currentDate >= Options.MinDateCanSelect) &&
        (Options.MaxDateCanSelect == null || currentDate <= Options.MaxDateCanSelect) &&
        (!Options.InactiveDays.Exists(day => day.Date == currentDate.Date)) &&
        (Options.CanSelectHolidays || _calendarService.GetDayOfWeek(currentDate) != _calendarService.GetLastDayOfWeek());

    private bool GetIsSelected(DateTime currentDate, DateTime date) =>
        Options.SelectionMode switch
        {
            Enums.SelectionMode.Single => date.Date == currentDate.Date,
            Enums.SelectionMode.Multiple or Enums.SelectionMode.Range => SelectedDays.Any(day => day.GregorianDate.Date == currentDate.Date),
            _ => false
        };

    private bool GetIsInRange(DateTime currentDate) =>
        Options.SelectionMode == Enums.SelectionMode.Range &&
        SelectedDays.Count == 2 &&
        SelectedDays[0].GregorianDate.Date < currentDate.Date &&
        SelectedDays[1].GregorianDate.Date > currentDate.Date;

    private void NextMonth(object obj) => InitCalendarDays(GetSelectedDate().AddMonths(1));
    private void PrevMonth(object obj) => InitCalendarDays(GetSelectedDate().AddMonths(-1));
    private void NextYear(object obj) => InitCalendarDays(GetSelectedDate().AddYears(1));
    private void PrevYear(object obj) => InitCalendarDays(GetSelectedDate().AddYears(-1));
    private void GotoToday(object obj) => InitCalendarDays(DateTime.Now.Date);

    private void SwitchMode(object obj) =>
        SelectDateMode = Enum.Parse<SelectionDateMode>(obj.ToString());

    private void SelectMonth(object obj)
    {
        if (obj is not PuiTuple value) return;

        if (int.TryParse(value.Key, out int monthNum))
        {
            var year = _calendarService.GetYear(GetSelectedDate());
            var date = _calendarService.ToGregorianDate($"{year}/{monthNum}/01");
            SelectDateMode = SelectionDateMode.Day;
            InitCalendarDays(date);
        }
    }

    private void SelectYear(object obj)
    {
        if (obj is not PuiTuple value) return;

        if (int.TryParse(value.Key, out int year))
        {
            var month = _calendarService.GetMonth(GetSelectedDate());
            var date = _calendarService.ToGregorianDate($"{year}/{month}/01");
            SelectDateMode = SelectionDateMode.Day;
            InitCalendarDays(date);
        }
    }

    private DateTime GetSelectedDate() =>
        DaysOfMonth.FirstOrDefault(day => day.IsSelected)?.GregorianDate ??
        DaysOfMonth[DaysOfMonth.Count / 2].GregorianDate;

    public bool CanClose(DayOfMonth selectedDate) =>
        (Options.AutoCloseAfterSelectDate && Options.SelectionMode == Enums.SelectionMode.Range && SelectedDays.Count == 2) ||
        (selectedDate.CanSelect && Options.AutoCloseAfterSelectDate && Options.SelectionMode == Enums.SelectionMode.Single);
}
