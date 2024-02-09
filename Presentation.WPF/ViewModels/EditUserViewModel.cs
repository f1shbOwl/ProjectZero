using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Dtos;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace Presentation.WPF.ViewModels;

public partial class EditUserViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly UserService _userService;
    private readonly RoleService _roleService;


    [ObservableProperty]
    private ObservableCollection<Role> _roleList = new ObservableCollection<Role>();

    public Role SelectedRole { get; set; } = null!;


    public EditUserViewModel(IServiceProvider serviceProvider, UserService userService, RoleService roleService)
    {
        _serviceProvider = serviceProvider;
        _userService = userService;
        _roleService = roleService;

        RoleList = new ObservableCollection<Role>(_roleService.GetAllRoles());

        User = _userService.SelectedUser;

    }

    [ObservableProperty]
    private User user = new();



    [RelayCommand]
    private async Task Update()
    {

        if (SelectedRole != null)
        {
            User.RoleName = SelectedRole.RoleName;
        }

        await _userService.UpdateUserAsync(User);

        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<UserDetailViewModel>();

    }

    [RelayCommand]
    private void Cancel()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<UserDetailViewModel>();
    }





}
