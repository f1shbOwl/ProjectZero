using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;

namespace Presentation.WPF.ViewModels;

public partial class UserAddViewModel : ObservableObject
{
    private readonly UserService _userService;
    private readonly RoleService _roleService;
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private ObservableCollection<Role> _roleList = new ObservableCollection<Role>();

    [ObservableProperty]
    private ObservableCollection<User> _userList = new ObservableCollection<User>();

    public Role SelectedRole { get; set; } = null!;


    [ObservableProperty]
    private User user = new();


    public UserAddViewModel(UserService userService,RoleService roleService ,IServiceProvider serviceProvider)
    {
        _userService = userService;
        _roleService = roleService;
        _serviceProvider = serviceProvider;

        RoleList = new ObservableCollection<Role>(_roleService.GetAllRoles());

        UserList = new ObservableCollection<User>(_userService.GetAllUsers());

    }




    





    [RelayCommand]
    private async Task AddUser(User user)
    {
        if (SelectedRole != null)
        {
            User.RoleName = SelectedRole.RoleName;
        }

        await _userService.CreateUserAsync(User);
        UserList = new ObservableCollection<User>(_userService.GetAllUsers());


        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<UserListViewModel>();
    }

    [RelayCommand]
    private void NavigateToList()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<UserListViewModel>();
    }

}
