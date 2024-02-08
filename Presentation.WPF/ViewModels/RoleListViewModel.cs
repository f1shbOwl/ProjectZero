using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Dtos;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;

namespace Presentation.WPF.ViewModels;

public partial class RoleListViewModel : ObservableObject
{
    private readonly RoleService _roleService;
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private Role role = new();

    [ObservableProperty]
    private ObservableCollection<Role> roleList = [];

    public RoleListViewModel(RoleService roleService, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _roleService = roleService;

        RoleList = new ObservableCollection<Role>(_roleService.GetAllRoles());

    }





    [RelayCommand]
    private async Task AddRole()
    {
        await _roleService.CreateRoleAsync(Role.RoleName!);

        RoleList = new ObservableCollection<Role>(_roleService.GetAllRoles());
        Role = new();

        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<RoleListViewModel>();
    }

    [RelayCommand]
    private void DeleteRole(Role role)
    {
        MessageBoxResult result = MessageBox.Show("Are you sure you want to remove this user?", "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            _roleService.DeleteRole(Role);

            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<RoleListViewModel>();
        }
    }

    [RelayCommand]
    private void NavigateToUsers()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<UserListViewModel>();
    }

}
