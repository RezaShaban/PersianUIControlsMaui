﻿using CommunityToolkit.Maui.Views;
using MauiPersianToolkit.Models;
using MauiPersianToolkit.ViewModels;

namespace MauiPersianToolkit.Controls;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class DatePickerView : Popup
{
    DayOfMonth selectedDate;
    DatePickerViewModel viewModel;

    public event EventHandler<SelectedDateChangedEventArgs> SelectedDateChanged;

    public DatePickerView(CalendarOptions options)
    {
        retry:
        try
        {
            InitializeComponent();
            btnAccept.Clicked += btnAccept_Clicked;
            btnCancel.Clicked += btnCancel_Clicked;
            viewModel = new DatePickerViewModel(options);
            this.BindingContext = viewModel;
        }
        catch(Exception ex)
        {
            goto retry;
        }
    }

    private void btnDay_Clicked(object sender, EventArgs e)
    {
        if (((Button)sender).CommandParameter is not DayOfMonth _selectedDate || !_selectedDate.CanSelect)
            return;

        viewModel.SelectDateCommand.Execute(_selectedDate);

        selectedDate = _selectedDate;

        if (SelectedDateChanged != null && viewModel.CanClose(selectedDate))
        {
            viewModel.Options.OnAccept?.Invoke(viewModel.SelectedDays);
            SelectedDateChanged.Invoke(sender, new SelectedDateChangedEventArgs()
            {
                SelectedDate = selectedDate,
                SelectedDates = viewModel.SelectedDays.ToList()
            });

        }
    }

    private void btnAccept_Clicked(object sender, EventArgs e)
    {
        var dates = viewModel.SelectedDays.Where(x => x.IsSelected).ToList();
        viewModel.Options.OnAccept?.Invoke(dates);
        this.Close();
    }

    private void btnCancel_Clicked(object sender, EventArgs e)
    {
        viewModel.Options.OnCancel?.Invoke();
        this.Close();
    }
}

public class SelectedDateChangedEventArgs : EventArgs
{
    public DayOfMonth SelectedDate { get; set; }
    public List<DayOfMonth> SelectedDates { get; set; }
}