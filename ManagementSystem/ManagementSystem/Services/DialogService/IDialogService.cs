using System.Threading.Tasks;
using MsBox.Avalonia.Enums;

namespace ManagementSystem.Services.DialogService;

public interface IDialogService
{
    public Task<ButtonResult?> ShowPopupDialogAsync(string title, string content, ButtonEnum button = ButtonEnum.Ok, Icon icon = Icon.None);
    public Task<ButtonResult?> ShowWindowDialogAsync(string title, string content, ButtonEnum button = ButtonEnum.Ok, Icon icon = Icon.None);
}