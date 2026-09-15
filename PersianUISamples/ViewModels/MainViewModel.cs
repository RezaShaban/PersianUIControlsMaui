using MauiPersianToolkit.Controls;
using MauiPersianToolkit.Models;
using MauiPersianToolkit.Services.Dialog;
using MauiPersianToolkit.ViewModels;
using PersianUISamples.Localization;
using PersianUISamples.Models;
using System.Collections.ObjectModel;
using PickerItem = PersianUISamples.Models.PickerItem;
using TreeViewModel = PersianUISamples.Models.TreeNodeModel;

namespace PersianUISamples.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private string persianDate;
        private string persianDateRange;
        private string persianDateMultiple;
        private List<string> badgeDates;
        private List<string> multipleBadgeDates;
        private CalendarOptions calendarOption;
        private CalendarOptions rangeCalendarOption;
        private CalendarOptions multipleCalendarOption;
        ObservableCollection<TreeViewModel> treeItems;
        ObservableCollection<TreeViewModel> selectedItemsTree;
        ObservableCollection<PickerItem> pickerMultipleItems;
        ObservableCollection<PickerItem> pickerItems;
        private Command onChangeDateCommand;
        private Command showAlertCommand;
        private Command showConfirmCommand;
        private Command showPromptCommand;
        private Command showCustomCommand;
        private Command registerInCommand;
        private Command showToastCommand;
        private Command showSnackbarCommand;
        private readonly IDialogService dialogService;
        private TimeSpan meetingTime = new(14, 30, 0);
        private PickerItem selectedCity;
        private string selectedCityText;

        public string PersianDate { get => persianDate; set => SetProperty(ref persianDate, value); }
        public TimeSpan MeetingTime { get => meetingTime; set => SetProperty(ref meetingTime, value); }
        public string PersianDateRange { get => persianDateRange; set => SetProperty(ref persianDateRange, value); }
        public string PersianDateMultiple { get => persianDateMultiple; set => SetProperty(ref persianDateMultiple, value); }
        public List<string> BadgeDates { get => badgeDates; set => SetProperty(ref badgeDates, value); }
        public List<string> MultipleBadgeDates { get => multipleBadgeDates; set => SetProperty(ref multipleBadgeDates, value); }
        public CalendarOptions CalendarOption { get => calendarOption; set => SetProperty(ref calendarOption, value); }
        public CalendarOptions RangeCalendarOption { get => rangeCalendarOption; set => SetProperty(ref rangeCalendarOption, value); }
        public CalendarOptions MultipleCalendarOption { get => multipleCalendarOption; set => SetProperty(ref multipleCalendarOption, value); }
        public ObservableCollection<PickerItem> PickerItems { get => pickerItems; set => SetProperty(ref pickerItems, value); }
        public ObservableCollection<TreeViewModel> TreeItems { get => treeItems; set => SetProperty(ref treeItems, value); }
        public ObservableCollection<TreeViewModel> SelectedItemsTree { get => selectedItemsTree; set => SetProperty(ref selectedItemsTree, value); }
        public ObservableCollection<PickerItem> PickerMultipleItems { get => pickerMultipleItems; set => SetProperty(ref pickerMultipleItems, value); }
        public ObservableCollection<PickerButton> PickerAdditionButtons { get; set; }
        public ObservableCollection<PickerItem> Cities { get; private set; }
        public AutoCompleteOptions CityAutoCompleteOptions { get; private set; }
        public PickerItem SelectedCity { get => selectedCity; set => SetProperty(ref selectedCity, value); }
        public string SelectedCityText { get => selectedCityText; set => SetProperty(ref selectedCityText, value); }
        public Command OnChangeDateCommand { get { onChangeDateCommand ??= new Command(OnDateChanged); return onChangeDateCommand; } }
        public Command ShowAlertCommand { get { showAlertCommand ??= new Command(ShowAlert); return showAlertCommand; } }
        public Command ShowConfirmCommand { get { showConfirmCommand ??= new Command(ShowConfirm); return showConfirmCommand; } }
        public Command ShowPromptCommand { get { showPromptCommand ??= new Command(ShowPrompt); return showPromptCommand; } }
        public Command ShowCustomCommand { get { showCustomCommand ??= new Command(ShowCustom); return showCustomCommand; } }
        public Command RegisterInCommand { get { registerInCommand ??= new Command(RegisterIn); return registerInCommand; } }
        public Command ShowToastCommand { get { showToastCommand ??= new Command(ShowToast); return showToastCommand; } }
        public Command ShowSnackbarCommand { get { showSnackbarCommand ??= new Command(ShowSnackbar); return showSnackbarCommand; } }
        public MainViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            InitData();
        }

        private void InitData()
        {
            SelectedItemsTree =
            [
                new() { Id = 1, Title = DemoStrings.TreeL11, ParentId = null },
                new() { Id = 8, Title = DemoStrings.TreeL41, ParentId = 6 }
            ];
            CalendarOption = new CalendarOptions()
            {
                CalendarType = MauiPersianToolkit.Enums.CalendarType.Hijri,
                SelectDateMode = MauiPersianToolkit.Enums.SelectionDateMode.Day,
                SelectionMode = MauiPersianToolkit.Enums.SelectionMode.Single,
                SelectDayColor = Color.FromArgb("#5B2BDF"),
                MinDateCanSelect = DateTime.Now.AddDays(-10),
                MaxDateCanSelect = DateTime.Now.AddDays(10),
                AutoCloseAfterSelectDate = true,
            };
            RangeCalendarOption = new CalendarOptions()
            {
                CalendarType = MauiPersianToolkit.Enums.CalendarType.Persian,
                SelectDateMode = MauiPersianToolkit.Enums.SelectionDateMode.Day,
                SelectionMode = MauiPersianToolkit.Enums.SelectionMode.Range,
                SelectDayColor = Color.FromArgb("#5B2BDF"),
                AutoCloseAfterSelectDate = false,
                OnAccept = OnAcceptDate,
                OnCancel = new Action(() => { }),
                MinDateCanSelect = DateTime.Now.Date,
                InactiveDays =
                [
                    DateTime.Now.AddDays(2),
                    DateTime.Now.AddDays(5),
                    DateTime.Now.AddDays(7),
                    DateTime.Now.AddDays(8)
                ],
                CanSelectHolidays = true
            };
            MultipleCalendarOption = new CalendarOptions()
            {
                CalendarType = MauiPersianToolkit.Enums.CalendarType.Gregorian,
                SelectDateMode = MauiPersianToolkit.Enums.SelectionDateMode.Day,
                SelectionMode = MauiPersianToolkit.Enums.SelectionMode.Multiple,
                SelectDayColor = Color.FromArgb("#5B2BDF"),
                AutoCloseAfterSelectDate = false,
                OnAccept = OnAcceptDateMultiple,
                OnCancel = new Action(() => { }),
                MinDateCanSelect = DateTime.Now.Date,
                InactiveDays =
                [
                    DateTime.Now.AddDays(2),
                    DateTime.Now.AddDays(5),
                    DateTime.Now.AddDays(7),
                    DateTime.Now.AddDays(8)
                ],
                CanSelectHolidays = true
            };
            PickerMultipleItems = SampleData.CreatePickerItems();
            PickerItems = SampleData.CreatePickerItems();
            Cities = SampleData.CreateCities();
            CityAutoCompleteOptions = new AutoCompleteOptions
            {
                FilterMode = MauiPersianToolkit.Enums.AutoCompleteFilterMode.Contains,
                MinimumPrefixLength = 1,
                MaxSuggestions = 8,
                UseNativeSuggestions = true,
                ShowClearButton = true,
                OpenOnFocus = true,
                FilterDebounceMs = 100
            };
            PickerAdditionButtons =
            [
                new() { Text = "\uf067" },
                new() { Text = "\uf057" }
            ];
            TreeItems = SampleData.CreateTreeItems();
        }

        private void OnAcceptDate(object obj)
        {
            if (obj is not List<DayOfMonth> dates)
                return;
            BadgeDates = dates.Select(x => x.PersianDate).ToList();
            this.PersianDateRange = dates.FirstOrDefault()?.PersianDate;
        }

        private void OnAcceptDateMultiple(object obj)
        {
            if (obj is not List<DayOfMonth> dates)
                return;
            MultipleBadgeDates = dates.Select(x => x.PersianDate).ToList();
            this.PersianDateMultiple = dates.FirstOrDefault()?.PersianDate;
        }

        private void OnDateChanged(object obj)
        {

        }

        private void RegisterIn()
        {
            ShowAlert(null);
        }

        private void ShowAlert(object obj)
        {
            dialogService.Alert(DemoStrings.AlertMessage, DemoStrings.AlertTitle);
        }

        private void ShowToast(object obj)
        {
            dialogService.Toast(new ToastConfig()
            {
                Message = DemoStrings.ToastMessage,
            });
        }

        private void ShowSnackbar(object obj)
        {
            dialogService.Snackbar(new SnackbarConfig()
            {
                Message = DemoStrings.SnackbarMessage,
                Duration = TimeSpan.FromSeconds(10),
                OnAction = new Action(() => { }),
            });
        }

        private void ShowConfirm(object obj)
        {
            dialogService.Confirm(new ConfirmConfig()
            {
                Title = DemoStrings.ConfirmTitle,
                AcceptText = DemoStrings.ConfirmYes,
                CancelText = DemoStrings.ConfirmNo,
                Message = DemoStrings.ConfirmMessage,
                Icon = MessageIcon.QUESTION,
                OnAction = new Action<bool>((arg) => { }),
            });
        }

        private void ShowPrompt(object obj)
        {
            dialogService.Prompt(new PromptConfig()
            {
                Title = DemoStrings.PromptTitle,
                Message = DemoStrings.PromptMessage,
                Placeholder = DemoStrings.PromptPlaceholder,
                Icon = MessageIcon.QUESTION,
                OnAction = new Action<PromptResult>((arg) => { }),
            });
        }

        private void ShowCustom(object obj)
        {
            dialogService.CustomDialog(new CustomDialogConfig()
            {
                Title = DemoStrings.CustomDialogTitle,
                Message = DemoStrings.CustomDialogMessage,
                Icon = MessageIcon.QUESTION,
                AcceptIcon = MessageIcon.QUESTION,
                Cancelable = true,
                CancelIcon = MessageIcon.ERROR,
                DialogColor = Colors.DeepPink,
                CloseWhenBackgroundIsClicked = true,
                CloseAfterAccept = true,
                OnAction = new Action<bool>((arg) => { }),
                Content = new StackLayout()
                {
                    Children =
                    {
                        new EntryView(){ PlaceHolder = DemoStrings.CustomNamePlaceholder },
                        new MauiPersianToolkit.Controls.DatePicker(){ PlaceHolder = DemoStrings.CustomDatePlaceholder }
                    }
                }
            });
        }
    }
}
