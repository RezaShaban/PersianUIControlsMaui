using MauiPersianToolkit.Enums;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace MauiPersianToolkit.Controls;

/// <summary>
/// Hierarchical list backed by a virtualized <see cref="CollectionView"/>.
/// Only currently visible (expanded) rows are in the visual tree.
/// </summary>
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class TreeView : ContentView
{
    private readonly TreeViewFlatModel _model = new();
    private INotifyCollectionChanged? _itemsNotify;

    public TreeView()
    {
        PersianTheme.SeedControlResources(Resources);
        InitializeComponent();
        list.ItemsSource = _model.VisibleRows;
    }

    /// <summary>Flattened rows currently shown (roots + expanded descendants).</summary>
    public ObservableCollection<TreeViewItem> VisibleRows => _model.VisibleRows;

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
        nameof(ItemsSource), typeof(IList), typeof(TreeView), default(IList), BindingMode.TwoWay);

    public IList ItemsSource
    {
        get => (IList)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly BindableProperty SelectedItemsProperty = BindableProperty.Create(
        nameof(SelectedItems), typeof(IList), typeof(TreeView),
        defaultBindingMode: BindingMode.TwoWay,
        defaultValueCreator: static _ => new List<object>());

    public IList SelectedItems
    {
        get => (IList)GetValue(SelectedItemsProperty);
        set => SetValue(SelectedItemsProperty, value);
    }

    public static readonly BindableProperty DisplayPropertyProperty = BindableProperty.Create(
        nameof(DisplayProperty), typeof(string), typeof(TreeView), default(string), BindingMode.TwoWay);

    public string DisplayProperty
    {
        get => (string)GetValue(DisplayPropertyProperty);
        set => SetValue(DisplayPropertyProperty, value);
    }

    public static readonly BindableProperty KeyPropertyProperty = BindableProperty.Create(
        nameof(KeyProperty), typeof(string), typeof(TreeView), default(string), BindingMode.TwoWay);

    public string KeyProperty
    {
        get => (string)GetValue(KeyPropertyProperty);
        set => SetValue(KeyPropertyProperty, value);
    }

    public static readonly BindableProperty SelectionModeProperty = BindableProperty.Create(
        nameof(SelectionMode), typeof(TreeViewSelectionMode), typeof(TreeView), default(TreeViewSelectionMode), BindingMode.TwoWay);

    public TreeViewSelectionMode SelectionMode
    {
        get => (TreeViewSelectionMode)GetValue(SelectionModeProperty);
        set => SetValue(SelectionModeProperty, value);
    }

    public static readonly BindableProperty ParentChildPropertyProperty = BindableProperty.Create(
        nameof(ParentChildProperty), typeof(string), typeof(TreeView), default(string), BindingMode.TwoWay);

    public string ParentChildProperty
    {
        get => (string)GetValue(ParentChildPropertyProperty);
        set => SetValue(ParentChildPropertyProperty, value);
    }

    public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
        nameof(ItemTemplate), typeof(ControlTemplate), typeof(TreeView), default(ControlTemplate), BindingMode.TwoWay);

    public ControlTemplate ItemTemplate
    {
        get => (ControlTemplate)GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    public static readonly BindableProperty ItemHeightProperty = BindableProperty.Create(
        nameof(ItemHeight), typeof(int), typeof(TreeView), 32, BindingMode.TwoWay);

    public int ItemHeight
    {
        get => (int)GetValue(ItemHeightProperty);
        set => SetValue(ItemHeightProperty, value);
    }

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName is nameof(ItemsSource) or nameof(KeyProperty) or nameof(ParentChildProperty)
            or nameof(DisplayProperty) or nameof(SelectionMode) or nameof(SelectedItems))
        {
            if (propertyName == nameof(ItemsSource))
                HookItemsSource();

            Rebuild();
        }
    }

    public void ToggleExpand(TreeViewItem item) => _model.ToggleExpand(item);

    internal void OnNodeSelectionChanged(TreeViewItem node)
    {
        SelectedItems ??= new List<object>();

        object? existing = null;
        foreach (var selected in SelectedItems.OfType<object>())
        {
            if (Equals(selected.GetType().GetProperty(KeyProperty)?.GetValue(selected), node.Id))
            {
                existing = selected;
                break;
            }
        }

        if (node.IsSelected && existing is null)
        {
            if (SelectionMode == TreeViewSelectionMode.Single)
            {
                SelectedItems.Clear();
                foreach (var row in VisibleRows)
                    row.IsSelected = Equals(row.Id, node.Id);
            }

            SelectedItems.Add(node.OriginalItem);
        }
        else if (!node.IsSelected && existing is not null)
        {
            SelectedItems.Remove(existing);
        }
    }

    private void HookItemsSource()
    {
        if (_itemsNotify is not null)
            _itemsNotify.CollectionChanged -= OnItemsCollectionChanged;

        _itemsNotify = ItemsSource as INotifyCollectionChanged;
        if (_itemsNotify is not null)
            _itemsNotify.CollectionChanged += OnItemsCollectionChanged;
    }

    private void OnItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => Rebuild();

    private void Rebuild() =>
        _model.Rebuild(ItemsSource, KeyProperty, ParentChildProperty, DisplayProperty, SelectionMode, SelectedItems);
}

public class TreeViewItem : ViewModels.ObservableObject
{
    private bool _isSelected;
    private bool _isExpanded;
    private bool _hasChildren;
    private int _depth;

    public object Id { get; set; } = null!;
    public object? ParentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public TreeViewSelectionMode SelectionMode { get; set; }
    public object OriginalItem { get; set; } = null!;
    public List<TreeViewItem> ChildItems { get; set; } = [];

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    public bool IsExpanded
    {
        get => _isExpanded;
        set => SetProperty(ref _isExpanded, value);
    }

    public bool HasChildren
    {
        get => _hasChildren;
        set => SetProperty(ref _hasChildren, value);
    }

    public int Depth
    {
        get => _depth;
        set
        {
            if (SetProperty(ref _depth, value))
                OnPropertyChanged(nameof(IndentMargin));
        }
    }

    public Thickness IndentMargin => new(Depth * 16, 0, 0, 0);
}
