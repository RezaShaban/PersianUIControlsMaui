using System.Runtime.CompilerServices;

namespace MauiPersianToolkit.Controls;

/// <summary>
/// Single virtualized row for <see cref="TreeView"/>. Expansion mutates the owner's
/// flattened <see cref="TreeView.VisibleRows"/> instead of nesting visual children.
/// </summary>
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class TreeViewNode : ContentView
{
    private bool _suppressSelectionEvent;

    public TreeViewNode()
    {
        PersianTheme.SeedControlResources(Resources);
        InitializeComponent();
    }

    public static readonly BindableProperty ShowItemProperty = BindableProperty.Create(
        nameof(ShowItem), typeof(TreeViewItem), typeof(TreeViewNode), default(TreeViewItem),
        propertyChanged: static (b, _, _) => ((TreeViewNode)b).OnShowItemChanged());

    public TreeViewItem? ShowItem
    {
        get => (TreeViewItem?)GetValue(ShowItemProperty);
        set => SetValue(ShowItemProperty, value);
    }

    public static readonly BindableProperty OwnerProperty = BindableProperty.Create(
        nameof(Owner), typeof(TreeView), typeof(TreeViewNode));

    public TreeView? Owner
    {
        get => (TreeView?)GetValue(OwnerProperty);
        set => SetValue(OwnerProperty, value);
    }

    public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
        nameof(ItemTemplate), typeof(ControlTemplate), typeof(TreeViewNode), default(ControlTemplate),
        propertyChanged: static (b, _, _) => ((TreeViewNode)b).UpdateTemplateVisibility());

    public ControlTemplate? ItemTemplate
    {
        get => (ControlTemplate?)GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    public static readonly BindableProperty ItemHeightProperty = BindableProperty.Create(
        nameof(ItemHeight), typeof(int), typeof(TreeViewNode), 32);

    public int ItemHeight
    {
        get => (int)GetValue(ItemHeightProperty);
        set => SetValue(ItemHeightProperty, value);
    }

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == nameof(ItemTemplate))
            UpdateTemplateVisibility();
    }

    private void OnShowItemChanged()
    {
        UpdateTemplateVisibility();
        SyncSelectionControls();
    }

    private void UpdateTemplateVisibility()
    {
        var hasTemplate = ItemTemplate is not null;
        defaultTemplate.IsVisible = !hasTemplate;
        customContent.IsVisible = hasTemplate;
    }

    private void SyncSelectionControls()
    {
        if (ShowItem is null)
            return;

        _suppressSelectionEvent = true;
        try
        {
            chk.IsChecked = ShowItem.IsSelected;
            rdo.IsChecked = ShowItem.IsSelected;
        }
        finally
        {
            _suppressSelectionEvent = false;
        }
    }

    private void Expand_Clicked(object? sender, EventArgs e)
    {
        if (ShowItem is null || Owner is null)
            return;

        Owner.ToggleExpand(ShowItem);
    }

    private void Selection_CheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (_suppressSelectionEvent || ShowItem is null || Owner is null)
            return;

        ShowItem.IsSelected = e.Value;
        Owner.OnNodeSelectionChanged(ShowItem);
    }
}
