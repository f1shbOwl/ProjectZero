using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Dtos;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace Presentation.WPF.ViewModels;

public partial class RoleListViewModel : ObservableObject
{
    private readonly RoleService _roleService;
    private readonly IServiceProvider _serviceProvider;
    private readonly UserService _userService;

    [ObservableProperty]
    private Role role = new();

    [ObservableProperty]
    private ObservableCollection<Role> roleList = [];

    [ObservableProperty]
    private ObservableCollection<User> _userList = new ObservableCollection<User>();

    public RoleListViewModel(RoleService roleService, UserService userService, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _roleService = roleService;
        _userService = userService;


        RoleList = new ObservableCollection<Role>(_roleService.GetAllRoles());
        UserList = new ObservableCollection<User>(_userService.GetAllUsers());

    }









    [RelayCommand]
    private async Task AddRole()
    {
        if (string.IsNullOrWhiteSpace(Role.RoleName))
        {
            MessageBox.Show("Rolename can not be empty, please enter a role", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }


        await _roleService.CreateRoleAsync(Role.RoleName!);

        RoleList = new ObservableCollection<Role>(_roleService.GetAllRoles());
        Role = new();

        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<RoleListViewModel>();
    }

    [RelayCommand]
    private void DeleteRole(Role role)
    {

        if (UserList.Any(user => user.RoleName == role.RoleName))
        {
            MessageBoxResult result = MessageBox.Show("There are registered users with this role name. If you proceed with deletion, the users will also be removed.", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Error);

            if (result == MessageBoxResult.Yes)
            {
                _roleService.DeleteRole(role);
                RoleList = new ObservableCollection<Role>(_roleService.GetAllRoles());
            }
        }
        else
        {
            _roleService.DeleteRole(role);
            RoleList = new ObservableCollection<Role>(_roleService.GetAllRoles());
        }
    }

    [RelayCommand]
    private void NavigateToUsers()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<UserListViewModel>();
    }

    [RelayCommand]
    private void NavigateToAddresses()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<AddressListViewModel>();
    }

}
