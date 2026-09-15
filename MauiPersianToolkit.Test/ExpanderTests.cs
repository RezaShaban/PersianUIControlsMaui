using MauiPersianToolkit.Controls;
using Xunit;

namespace MauiPersianToolkit.Test;

/// <summary>
/// Covers the native expander that replaced the CommunityToolkit one.
/// </summary>
public class ExpanderTests
{
    [Fact]
    public void HeaderAndContentBothStayInTheLayout()
    {
        var header = new Label();
        var content = new Label();

        var expander = new Expander { Header = header, Content = content };

        Assert.Contains(header, expander.Children);
        Assert.Contains(content, expander.Children);
        Assert.Equal(0, Grid.GetRow(header));
        Assert.Equal(1, Grid.GetRow(content));
    }

    [Fact]
    public void CollapsingHidesOnlyTheContent()
    {
        var header = new Label();
        var content = new Label();
        var expander = new Expander { Header = header, Content = content };

        expander.IsExpanded = false;

        Assert.False(content.IsVisible);
        Assert.True(header.IsVisible);
    }

    [Fact]
    public void ContentAssignedWhileCollapsedStartsHidden()
    {
        var expander = new Expander { IsExpanded = false };
        var content = new Label();

        expander.Content = content;

        Assert.False(content.IsVisible);
    }

    [Fact]
    public void ExpandedChangedReportsTheNewState()
    {
        var expander = new Expander { Content = new Label() };
        var states = new List<bool>();
        expander.ExpandedChanged += (_, e) => states.Add(e.IsExpanded);

        expander.IsExpanded = false;
        expander.IsExpanded = true;

        Assert.Equal([false, true], states);
    }

    [Fact]
    public void ReplacingTheContentRemovesThePreviousOne()
    {
        var first = new Label();
        var second = new Label();
        var expander = new Expander { Content = first };

        expander.Content = second;

        Assert.DoesNotContain(first, expander.Children);
        Assert.Contains(second, expander.Children);
    }

    [Fact]
    public void TappingTheHeaderTogglesExpansion()
    {
        var header = new Label();
        var expander = new Expander { Header = header, Content = new Label() };

        var tap = Assert.IsType<TapGestureRecognizer>(Assert.Single(header.GestureRecognizers));
        tap.Command!.Execute(null);

        Assert.False(expander.IsExpanded);
    }
}
