using System.ComponentModel;
using ManagementSystem.ViewModels.DataVM.User;

namespace ManagementSystem.Services.UserStorage;

public interface IUserStorageService : INotifyPropertyChanged
{
    public UserViewModel? CurrentUser { get; set; }
}