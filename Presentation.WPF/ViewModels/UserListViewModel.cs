using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Dtos;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;

namespace Presentation.WPF.ViewModels;

public partial class UserListViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly UserService _userService;

    [ObservableProperty]
    private ObservableCollection<User> _userList = new ObservableCollection<User>();


    public UserListViewModel(IServiceProvider serviceProvider, UserService userService)
    {
        _serviceProvider = serviceProvider;
        _userService = userService;

        UserList = new ObservableCollection<User>(_userService.GetAllUsers());
    }











    [RelayCommand]
    private void NavigateToAddView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<UserAddViewModel>();
    }

    [RelayCommand]
    private void DeleteUser(User user)
    {
        MessageBoxResult result = MessageBox.Show("Are you sure you want to remove this user?", "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            _userService.DeleteUser(user);

            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<UserListViewModel>();
        }
    }

    [RelayCommand]
    private void NavigateToUserDetailView(User user)
    {
        _userService.SelectedUser = user;

        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<UserDetailViewModel>();


    }

    [RelayCommand]
    private void NavigateToEditContactView(User user)
    {
        _userService.SelectedUser = user;

        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<EditUserViewModel>();
    }

    [RelayCommand]
    private void NavigateToRoles()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<RoleListViewModel>();
    }

}
