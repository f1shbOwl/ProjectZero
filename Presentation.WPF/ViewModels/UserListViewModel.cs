using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Dtos;
using Infrastructure.Services;
using System.Collections.ObjectModel;

namespace Presentation.WPF.ViewModels;

public partial class UserListViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly UserService _userService;


    public UserListViewModel(IServiceProvider serviceProvider, UserService userService)
    {
        _serviceProvider = serviceProvider;
        _userService = userService;

        UserList = new ObservableCollection<User>(_userService.GetAllUsers());
    }


    [ObservableProperty]
    private ObservableCollection<User> _userList = new ObservableCollection<User>();

}
