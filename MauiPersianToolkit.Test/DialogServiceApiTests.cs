using MauiPersianToolkit.Controls;
using MauiPersianToolkit.Models;
using MauiPersianToolkit.Services.Dialog;
using Xunit;

namespace MauiPersianToolkit.Test;

public class DialogServiceApiTests
{
    [Fact]
    public void IDialogService_Exposes_Async_Confirm_Prompt_And_Alert()
    {
        var methods = typeof(IDialogService).GetMethods().Select(m => m.Name).ToHashSet();

        Assert.Contains(nameof(IDialogService.AlertAsync), methods);
        Assert.Contains(nameof(IDialogService.ConfirmAsync), methods);
        Assert.Contains(nameof(IDialogService.PromptAsync), methods);
        Assert.Contains(nameof(IDialogService.CustomDialogAsync), methods);
    }

    [Fact]
    public void PromptResult_Defaults_To_NotOk()
    {
        var result = new PromptResult();
        Assert.False(result.IsOk);
        Assert.Null(result.Value);
    }

    [Fact]
    public void PersianInputBase_VisualState_Names_Are_Stable()
    {
        Assert.Equal("Normal", PersianInputBase.InputVisualState.Normal);
        Assert.Equal("Focused", PersianInputBase.InputVisualState.Focused);
        Assert.Equal("Disabled", PersianInputBase.InputVisualState.Disabled);
        Assert.Equal("Error", PersianInputBase.InputVisualState.Error);
    }
}
