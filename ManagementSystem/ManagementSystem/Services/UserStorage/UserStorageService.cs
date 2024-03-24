using System.ComponentModel;
using System.Runtime.CompilerServices;
using ManagementSystem.ViewModels.DataVM.User;

namespace ManagementSystem.Services.UserStorage;

public class UserStorageService : IUserStorageService
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private UserViewModel? _currentUser = null;
    public UserViewModel? CurrentUser
    {
        get => _currentUser;
        set => Set(ref _currentUser, value);
    }

    protected void OnPropertyChanged([CallerMemberName]string? property = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }

    private bool Set<T>(ref T field, T value, [CallerMemberName] string? property = null)
    {
        if (Equals(field, value))
        {
            return false;
        }
        field = value;
        OnPropertyChanged(property);
        return true;
    }
}