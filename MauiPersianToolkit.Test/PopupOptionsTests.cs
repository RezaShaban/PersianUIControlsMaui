using MauiPersianToolkit.Core;
using Xunit;

namespace MauiPersianToolkit.Test;

/// <summary>
/// Covers the platform-neutral popup contracts that replaced the CommunityToolkit ones.
/// </summary>
public class PopupOptionsTests
{
    [Fact]
    public void DefaultsToADimmedBackdropThatCanBeDismissedByTheHostPopup()
    {
        var options = new PopupOptions();

        Assert.NotNull(options.Shape);
        Assert.Null(options.Shadow);
        Assert.Null(options.CanBeDismissedByTappingOutsideOfPopup);
        Assert.True(options.BackgroundColor.Alpha is > 0 and < 1);
    }

    [Fact]
    public void AnUnshapedSurfaceCanBeRequestedByClearingShapeAndShadow()
    {
        // This is what the dialog pages do: they draw their own card, so the popup surface
        // must not add a second border or shadow around it.
        var options = new PopupOptions { Shape = null, Shadow = null };

        Assert.Null(options.Shape);
        Assert.Null(options.Shadow);
    }

    [Fact]
    public void DefaultHandsOutAFreshShapePerDisplay()
    {
        // The shape is a live element, so two popups on screen at once must not share one.
        Assert.NotSame(PopupOptions.Default, PopupOptions.Default);
        Assert.NotSame(PopupOptions.Default.Shape, PopupOptions.Default.Shape);
    }

    [Theory]
    [InlineData(ToastDuration.Short, 2)]
    [InlineData(ToastDuration.Long, 3.5)]
    public void ToastDurationMapsOntoWallClockTimeForPlatformsWithoutAShortLongNotion(
        ToastDuration duration, double expectedSeconds)
    {
        Assert.Equal(expectedSeconds, duration.ToTimeSpan().TotalSeconds);
    }

    [Fact]
    public void SnackbarOptionsDefaultToAReadableDarkSurface()
    {
        var options = new SnackbarOptions();

        Assert.Equal(Colors.White, options.TextColor);
        Assert.Equal(Colors.White, options.ActionButtonTextColor);
        Assert.Equal(4, options.CornerRadius.TopLeft);
    }
}
