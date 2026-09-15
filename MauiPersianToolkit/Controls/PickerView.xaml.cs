using MauiPersianToolkit.Core;
using MauiPersianToolkit.Extensions;
using MauiPersianToolkit.Localization;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using static Microsoft.Maui.Controls.VisualStateManager;

namespace MauiPersianToolkit.Controls;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class PickerView : PersianInputBase
{
    #region Fields

    private Page? _hostPage;
    private Popup? _sheet;
    private CollectionView? _itemsList;
    private Label? _titleLabel;
    private Grid? _buttonLayout;
    private Button? _cancelButton;
    private Button? _acceptButton;
    private HorizontalStackLayout? _titleLayout;
    private bool _isShowing;
    private bool _sheetBuilt;
    private PropertyInfo? _displayPropertyInfo;
    private PropertyInfo? _valuePropertyInfo;
    private Type? _displayPropertyType;
    private Type? _valuePropertyType;
    private string? _cachedDisplayPropertyName;
    private string? _cachedValueMemberName;

    #endregion

    #region Properties

    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(PickerView), default(string), BindingMode.TwoWay);
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly BindableProperty SelectedItemColorProperty = BindableProperty.Create(nameof(SelectedItemColor), typeof(Color), typeof(PickerView), Colors.Orange, BindingMode.TwoWay);
    public Color SelectedItemColor
    {
        get => (Color)GetValue(SelectedItemColorProperty);
        set => SetValue(SelectedItemColorProperty, value);
    }

    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(PickerView), default(string), BindingMode.TwoWay);
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty CancelTextProperty = BindableProperty.Create(
        nameof(CancelText), typeof(string), typeof(PickerView), null, BindingMode.TwoWay,
        defaultValueCreator: static _ => PersianToolkitStrings.Cancel);
    public string CancelText
    {
        get => (string)GetValue(CancelTextProperty);
        set => SetValue(CancelTextProperty, value);
    }

    public static readonly BindableProperty AcceptTextProperty = BindableProperty.Create(
        nameof(AcceptText), typeof(string), typeof(PickerView), null, BindingMode.TwoWay,
        defaultValueCreator: static _ => PersianToolkitStrings.Confirm);
    public string AcceptText
    {
        get => (string)GetValue(AcceptTextProperty);
        set => SetValue(AcceptTextProperty, value);
    }

    public static readonly BindableProperty SelectionModeProperty = BindableProperty.Create(nameof(SelectionMode), typeof(SelectionMode), typeof(PickerView), SelectionMode.Single, BindingMode.TwoWay);
    public SelectionMode SelectionMode
    {
        get => (SelectionMode)GetValue(SelectionModeProperty);
        set => SetValue(SelectionModeProperty, value);
    }

    public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(PickerView), default(string), BindingMode.TwoWay);
    public string FontFamily
    {
        get => (string)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public static readonly BindableProperty SelectionChangedCommandProperty = BindableProperty.Create(nameof(SelectionChangedCommand), typeof(Command), typeof(PickerView), default(Command), BindingMode.TwoWay);
    public Command SelectionChangedCommand
    {
        get => (Command)GetValue(SelectionChangedCommandProperty);
        set => SetValue(SelectionChangedCommandProperty, value);
    }

    public static readonly BindableProperty SelectionChangedCommandParameterProperty = BindableProperty.Create(nameof(SelectionChangedCommandParameter), typeof(object), typeof(PickerView), null, BindingMode.TwoWay);
    public object SelectionChangedCommandParameter
    {
        get => GetValue(SelectionChangedCommandParameterProperty);
        set => SetValue(SelectionChangedCommandParameterProperty, value);
    }

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(PickerView), default(IList), BindingMode.TwoWay);
    public IList ItemsSource
    {
        get => (IList)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly BindableProperty AdditionButtonsProperty = BindableProperty.Create(
        nameof(AdditionButtons), typeof(IList<PickerButton>), typeof(PickerView),
        defaultValueCreator: static _ => new List<PickerButton>());
    public IList<PickerButton> AdditionButtons
    {
        get => (IList<PickerButton>)GetValue(AdditionButtonsProperty);
        set => SetValue(AdditionButtonsProperty, value);
    }

    public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(PickerView), default(DataTemplate), BindingMode.TwoWay);
    public DataTemplate ItemTemplate
    {
        get => (DataTemplate)GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(PickerView), default(object), BindingMode.TwoWay);
    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public static readonly BindableProperty SelectedValueProperty = BindableProperty.Create(nameof(SelectedValue), typeof(object), typeof(PickerView), default(object), BindingMode.TwoWay);
    public object SelectedValue
    {
        get => GetValue(SelectedValueProperty);
        set => SetValue(SelectedValueProperty, value);
    }

    public static readonly BindableProperty SelectedItemsProperty = BindableProperty.Create(
        nameof(SelectedItems), typeof(ObservableCollection<object>), typeof(PickerView),
        defaultValueCreator: static _ => new ObservableCollection<object>());
    public ObservableCollection<object> SelectedItems
    {
        get => (ObservableCollection<object>)GetValue(SelectedItemsProperty);
        set => SetValue(SelectedItemsProperty, value);
    }

    public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(PickerView), -1, BindingMode.TwoWay);
    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    public static readonly BindableProperty DisplayPropertyProperty = BindableProperty.Create(nameof(DisplayProperty), typeof(string), typeof(PickerView), default(string), BindingMode.TwoWay);
    public string DisplayProperty
    {
        get => (string)GetValue(DisplayPropertyProperty);
        set => SetValue(DisplayPropertyProperty, value);
    }

    public static readonly BindableProperty ValueMemberProperty = BindableProperty.Create(nameof(ValueMember), typeof(string), typeof(PickerView), default(string), BindingMode.TwoWay);
    public string ValueMember
    {
        get => (string)GetValue(ValueMemberProperty);
        set => SetValue(ValueMemberProperty, value);
    }

    public static readonly BindableProperty RowIconPropertyProperty = BindableProperty.Create(nameof(RowIconProperty), typeof(string), typeof(PickerView), default(string), BindingMode.TwoWay);
    public string RowIconProperty
    {
        get => (string)GetValue(RowIconPropertyProperty);
        set => SetValue(RowIconPropertyProperty, value);
    }

    public static readonly BindableProperty AcceptCommandProperty = BindableProperty.Create(nameof(AcceptCommand), typeof(Command), typeof(PickerView), default(Command), BindingMode.TwoWay);
    public Command AcceptCommand
    {
        get => (Command)GetValue(AcceptCommandProperty);
        set => SetValue(AcceptCommandProperty, value);
    }

    public static readonly BindableProperty OnOpenCommandProperty = BindableProperty.Create(nameof(OnOpenCommand), typeof(Command), typeof(PickerView), default(Command), BindingMode.TwoWay);
    public Command OnOpenCommand
    {
        get => (Command)GetValue(OnOpenCommandProperty);
        set => SetValue(OnOpenCommandProperty, value);
    }

    #endregion

    public event EventHandler<SelectionChangedEventArgs>? SelectionChanged;

    public PickerView()
    {
        InitializeComponent();
        AttachInputChrome(outline);

        var tapped = new TapGestureRecognizer
        {
            Command = new Command(OpenSheet)
        };
        grdPattern.GestureRecognizers.Add(tapped);
    }

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == IsEnabledProperty.PropertyName)
            PlaceHolderColor = IsEnabled ? PlaceHolderColor : Colors.Gray;

        if (propertyName == IconProperty.PropertyName)
            lblIcon.IsVisible = !string.IsNullOrEmpty(Icon);

        if (propertyName == SelectedIndexProperty.PropertyName
            && ItemsSource is not null
            && SelectedIndex >= 0
            && SelectedIndex < ItemsSource.Count)
        {
            SelectedItem = ItemsSource[SelectedIndex];
        }

        if (propertyName == SelectedValueProperty.PropertyName
            && ItemsSource is not null
            && !string.IsNullOrEmpty(ValueMember)
            && SelectedValue is not null)
        {
            foreach (var item in ItemsSource)
            {
                if (SelectedValue.Equals(GetValueMember(item)))
                {
                    SelectedItem = item;
                    break;
                }
            }
        }

        if (propertyName == DisplayPropertyProperty.PropertyName)
        {
            _displayPropertyInfo = null;
            _displayPropertyType = null;
            _cachedDisplayPropertyName = null;
        }

        if (propertyName == ValueMemberProperty.PropertyName)
        {
            _valuePropertyInfo = null;
            _valuePropertyType = null;
            _cachedValueMemberName = null;
        }
    }

    public void ShowDialog() => OpenSheet();

    private async void OpenSheet()
    {
        if (_isShowing)
            return;

        OnOpenCommand?.Execute(null);

        _hostPage ??= Application.Current?.Windows.FirstOrDefault()?.Page;
        if (_hostPage is null)
            return;

        EnsureSheet();
        RefreshSheetContent();

        _isShowing = true;
        try
        {
            await _hostPage.ShowPopupAsync(_sheet!, new PopupOptions { Shape = null, Shadow = null });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"PickerView.OpenSheet failed: {ex}");
        }
        finally
        {
            _isShowing = false;
        }
    }

    private void EnsureSheet()
    {
        if (_sheetBuilt && _sheet is not null)
            return;

        _titleLabel = new Label
        {
            HorizontalOptions = LayoutOptions.Start,
            HorizontalTextAlignment = TextAlignment.Start,
            VerticalOptions = LayoutOptions.Center,
            FontFamily = "IranianSans",
            FontSize = 15,
            TextColor = Colors.Gray,
            Padding = new Thickness(15, 0)
        };

        _titleLayout = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Fill,
            Padding = new Thickness(0, 0, 0, 5),
            FlowDirection = FlowDirection.RightToLeft,
            Children = { _titleLabel }
        };

        _itemsList = new CollectionView
        {
            VerticalOptions = LayoutOptions.Fill,
            HorizontalOptions = LayoutOptions.Fill,
            HeightRequest = 300
        };
        _itemsList.SelectionChanged += OnItemsSelectionChanged;
        EnsureSelectionVisualStates(_itemsList);

        _cancelButton = new Button
        {
            TextColor = ThemeColors.Cancel,
            HorizontalOptions = LayoutOptions.Fill,
            BackgroundColor = Colors.Transparent,
            FontFamily = "IranianSans",
            Command = new Command(async () =>
            {
                SelectedItems.Clear();
                if (_sheet is not null)
                    await _sheet.CloseAsync();
            })
        };

        _acceptButton = new Button
        {
            TextColor = ThemeColors.Accept,
            HorizontalOptions = LayoutOptions.Fill,
            BackgroundColor = Colors.Transparent,
            FontFamily = "IranianSans",
            Command = new Command(async () =>
            {
                BindableLayout.SetItemsSource(hslSelecteItems, SelectedItems.Select(GetDisplayText).ToList());
                AcceptCommand?.Execute(SelectedItems);
                if (_sheet is not null)
                    await _sheet.CloseAsync();
            })
        };

        _buttonLayout = new Grid
        {
            BackgroundColor = ThemeColors.Footer,
            HorizontalOptions = LayoutOptions.Fill,
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = 50 }
            },
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) }
            }
        };

        _sheet = new Popup
        {
            VerticalOptions = LayoutOptions.End,
            HorizontalOptions = LayoutOptions.Fill,
            BackgroundColor = Colors.Transparent,
            Content = new VerticalStackLayout
            {
                BackgroundColor = ThemeColors.Surface,
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                Padding = new Thickness(15, 15, 15, 0),
                Children =
                {
                    _titleLayout,
                    _itemsList,
                    _buttonLayout
                }
            }
        };

        _sheetBuilt = true;
    }

    private void RefreshSheetContent()
    {
        if (_sheet is null || _itemsList is null || _titleLabel is null || _buttonLayout is null
            || _cancelButton is null || _acceptButton is null || _titleLayout is null)
            return;

        // Re-assert bottom-sheet placement (must survive popup reuse).
        _sheet.VerticalOptions = LayoutOptions.End;
        _sheet.HorizontalOptions = LayoutOptions.Fill;
        _sheet.BackgroundColor = Colors.Transparent;

        _titleLabel.Text = string.IsNullOrEmpty(Title) ? PlaceHolder : Title;

        // Reset extra title actions, keep the title label.
        while (_titleLayout.Count > 1)
            _titleLayout.RemoveAt(_titleLayout.Count - 1);

        if (AdditionButtons is { Count: > 0 })
        {
            foreach (var button in AdditionButtons)
                _titleLayout.Children.Add(button.ShallowCopy());

            var occupiedWidth = AdditionButtons.Sum(x => x.WidthRequest);
            _titleLabel.WidthRequest =
                (DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density) - 45 - occupiedWidth;
        }
        else
        {
            _titleLabel.ClearValue(WidthRequestProperty);
        }

        _itemsList.SelectionMode = SelectionMode;
        _itemsList.ItemsSource = ItemsSource;
        _itemsList.SelectedItem = SelectedItem;
        _itemsList.SelectedItems = SelectedItems;
        _itemsList.ItemTemplate = ItemTemplate ?? DefaultItemTemplate;

        _cancelButton.Text = CancelText;
        _acceptButton.Text = AcceptText;
        _acceptButton.IsVisible = SelectionMode == SelectionMode.Multiple;

        _buttonLayout.Children.Clear();
        _buttonLayout.ColumnDefinitions.Clear();

        if (SelectionMode == SelectionMode.Multiple)
        {
            _buttonLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) });
            _buttonLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) });
            _buttonLayout.Add(_acceptButton, 0, 0);
            _buttonLayout.Add(_cancelButton, 1, 0);
        }
        else
        {
            _buttonLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            _buttonLayout.Add(_cancelButton, 0, 0);
        }
    }

    private async void OnItemsSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is not CollectionView list || list.SelectionMode != SelectionMode.Single)
            return;

        if (e.CurrentSelection.Count > 0)
            SelectedItem = e.CurrentSelection[0];

        if (SelectedItem is null)
            return;

        SelectionChanged?.Invoke(this, e);
        SelectionChangedCommand?.Execute(SelectionChangedCommandParameter ?? SelectedItem);
        lblSelected.Text = GetDisplayText(SelectedItem);

        if (_sheet is not null)
            await _sheet.CloseAsync();
    }

    private void EnsureSelectionVisualStates(CollectionView list)
    {
        if (list.Resources.Any(x => x.Key == "Microsoft.Maui.Controls.StackLayout"))
            return;

        var backgroundColorSetter = new Setter { Property = BackgroundColorProperty, Value = SelectedItemColor };
        var stateSelected = new VisualState { Name = CommonStates.Selected, Setters = { backgroundColorSetter } };
        var stateNormal = new VisualState { Name = CommonStates.Normal };
        var visualStateGroup = new VisualStateGroup { Name = nameof(CommonStates), States = { stateSelected, stateNormal } };
        var visualStateGroupList = new VisualStateGroupList { visualStateGroup };
        var vsgSetter = new Setter { Property = VisualStateGroupsProperty, Value = visualStateGroupList };
        var style = new Style(typeof(StackLayout)) { Setters = { vsgSetter }, BaseResourceKey = "collectionItem" };
        list.Resources.Add(style);
    }

    private DataTemplate DefaultItemTemplate => new(() =>
    {
        var label = new LabelView
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            FontSize = 14,
            IconFontSize = 16,
            TextColor = ThemeColors.OnSurface,
            FlowDirection = FlowDirection.RightToLeft,
            InputTransparent = true,
            HorizontalTextAlignment = TextAlignment.Start
        };

        if (string.IsNullOrEmpty(DisplayProperty))
            label.SetBinding(LabelView.TextProperty, ".");
        else
            label.SetBinding(LabelView.TextProperty, DisplayProperty);

        if (!string.IsNullOrEmpty(RowIconProperty))
            label.SetBinding(LabelView.IconProperty, RowIconProperty);
        else
            label.Icon = "";

        return new StackLayout
        {
            HorizontalOptions = LayoutOptions.Fill,
            Padding = new Thickness(0, 10),
            Children = { label }
        };
    });

    private object? GetValueMember(object item)
    {
        if (item is null || string.IsNullOrWhiteSpace(ValueMember))
            return null;

        EnsureValuePropertyInfo(item);
        return _valuePropertyInfo?.GetValue(item);
    }

    private void EnsureValuePropertyInfo(object item)
    {
        var type = item.GetType();
        if (_valuePropertyInfo is not null
            && _valuePropertyType == type
            && _cachedValueMemberName == ValueMember)
            return;

        _valuePropertyType = type;
        _cachedValueMemberName = ValueMember;
        _valuePropertyInfo = type.GetProperty(ValueMember);
    }

    private string GetDisplayText(object? item)
    {
        if (item is null)
            return string.Empty;

        if (string.IsNullOrWhiteSpace(DisplayProperty))
            return item.ToString() ?? string.Empty;

        EnsureDisplayPropertyInfo(item);
        return _displayPropertyInfo?.GetValue(item)?.ToString() ?? string.Empty;
    }

    private void EnsureDisplayPropertyInfo(object item)
    {
        var type = item.GetType();
        if (_displayPropertyInfo is not null
            && _displayPropertyType == type
            && _cachedDisplayPropertyName == DisplayProperty)
            return;

        _displayPropertyType = type;
        _cachedDisplayPropertyName = DisplayProperty;
        _displayPropertyInfo = type.GetProperty(DisplayProperty);
    }
}

public class PickerButton : Button
{
    public PickerButton()
    {
        FontSize = 24;
        BackgroundColor = ThemeColors.Surface;
        HorizontalOptions = LayoutOptions.End;
        WidthRequest = 32;
        HeightRequest = 32;
        Padding = 0;
        TextColor = ThemeColors.Muted;
        FontFamily = "FontAwesome";
    }

    public PickerButton ShallowCopy() => (PickerButton)MemberwiseClone();
}
