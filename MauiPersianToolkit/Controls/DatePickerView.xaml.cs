using MauiPersianToolkit.Models;
using MauiPersianToolkit.ViewModels;
using System.Runtime.CompilerServices;

namespace MauiPersianToolkit.Controls;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class DatePickerView : Popup
{
    private DayOfMonth _selectedDate;
    private DatePickerViewModel _viewModel;

    #region Properties

    public static readonly BindableProperty CalendarOptionProperty = BindableProperty.Create(
        nameof(CalendarOption), typeof(CalendarOptions), typeof(DatePickerView),
        defaultValueCreator: static _ => new CalendarOptions());
    public CalendarOptions CalendarOption
    {
        get => (CalendarOptions)GetValue(CalendarOptionProperty);
        set => SetValue(CalendarOptionProperty, value);
    }
    #endregion

    public event EventHandler<SelectedDateChangedEventArgs> SelectedDateChanged;

    public DatePickerView()
    {
        InitializeComponent();
        WireButtons();
    }

    public DatePickerView(CalendarOptions options)
    {
        InitializeComponent();
        WireButtons();
        InitializeView(options);
    }

    /// <summary>
    /// Refreshes calendar state for a reused popup instance without rebuilding XAML.
    /// </summary>
    public void Prepare(CalendarOptions options)
    {
        if (_viewModel is null)
        {
            _viewModel = new DatePickerViewModel(options);
            BindingContext = _viewModel;
        }
        else
        {
            _viewModel.Refresh(options);
        }

        if (!ReferenceEquals(CalendarOption, options))
            SetValue(CalendarOptionProperty, options);
    }

    private void WireButtons()
    {
        btnAccept.Clicked -= BtnAccept_Clicked;
        btnCancel.Clicked -= BtnCancel_Clicked;
        btnAccept.Clicked += BtnAccept_Clicked;
        btnCancel.Clicked += BtnCancel_Clicked;
    }

    private void InitializeView(CalendarOptions options) => Prepare(options);

    private async void BtnDay_Clicked(object sender, EventArgs e)
    {
        if (((Button)sender).CommandParameter is not DayOfMonth selectedDayOfMonth || !selectedDayOfMonth.CanSelect)
            return;

        _viewModel.SelectDateCommand.Execute(selectedDayOfMonth);
        _selectedDate = selectedDayOfMonth;

        if (SelectedDateChanged != null && _viewModel.CanClose(_selectedDate))
        {
            _viewModel.Options.OnAccept?.Invoke(_viewModel.SelectedDays);
            SelectedDateChanged.Invoke(sender, new SelectedDateChangedEventArgs
            {
                SelectedDate = _selectedDate,
                SelectedDates = _viewModel.SelectedDays.ToList()
            });
            await this.CloseAsync();
        }
    }

    private async void BtnAccept_Clicked(object sender, EventArgs e)
    {
        if (!_viewModel.CanAccept)
            return;

        var dates = _viewModel.SelectedDays.Where(x => x.IsSelected).ToList();
        _viewModel.Options.OnAccept?.Invoke(dates);
        await this.CloseAsync();
    }

    private async void BtnCancel_Clicked(object sender, EventArgs e)
    {
        _viewModel.Options.OnCancel?.Invoke();
        await this.CloseAsync();
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        switch (propertyName)
        {
            case nameof(CalendarOption) when CalendarOption is not null:
                if (_viewModel is null)
                    Prepare(CalendarOption);
                else if (!ReferenceEquals(_viewModel.Options, CalendarOption))
                    _viewModel.Refresh(CalendarOption);
                break;
        }
    }
}

public class SelectedDateChangedEventArgs : EventArgs
{
    public DayOfMonth SelectedDate { get; set; }
    public List<DayOfMonth> SelectedDates { get; set; }
}