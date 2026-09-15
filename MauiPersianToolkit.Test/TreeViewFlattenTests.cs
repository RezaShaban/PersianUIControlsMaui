using MauiPersianToolkit.Controls;
using MauiPersianToolkit.Enums;
using System.Collections.ObjectModel;
using Xunit;

namespace MauiPersianToolkit.Test;

public class TreeViewFlattenTests
{
    private sealed class Node
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public int? ParentId { get; set; }
    }

    [Fact]
    public void Rebuild_ShowsOnlyRoots_UntilExpanded()
    {
        var model = new TreeViewFlatModel();
        model.Rebuild(
            new ObservableCollection<Node>
            {
                new() { Id = 1, Title = "root", ParentId = null },
                new() { Id = 2, Title = "child", ParentId = 1 },
                new() { Id = 3, Title = "grand", ParentId = 2 },
            },
            nameof(Node.Id),
            nameof(Node.ParentId),
            nameof(Node.Title),
            TreeViewSelectionMode.Multiple,
            selectedItems: null);

        Assert.Single(model.VisibleRows);
        Assert.Equal(1, model.VisibleRows[0].Id);
        Assert.True(model.VisibleRows[0].HasChildren);

        model.ToggleExpand(model.VisibleRows[0]);
        Assert.Equal(2, model.VisibleRows.Count);
        Assert.Equal(2, model.VisibleRows[1].Id);
        Assert.Equal(1, model.VisibleRows[1].Depth);

        model.ToggleExpand(model.VisibleRows[1]);
        Assert.Equal(3, model.VisibleRows.Count);
        Assert.Equal(3, model.VisibleRows[2].Id);
        Assert.Equal(2, model.VisibleRows[2].Depth);

        model.ToggleExpand(model.VisibleRows[0]);
        Assert.Single(model.VisibleRows);
    }
}
