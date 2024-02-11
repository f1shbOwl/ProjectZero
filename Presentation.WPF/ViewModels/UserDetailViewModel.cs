using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Dtos;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;

namespace Presentation.WPF.ViewModels;

public partial class UserDetailViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly UserService _userService;

    public UserDetailViewModel(IServiceProvider serviceProvider, UserService userService)
    {
        _serviceProvider = serviceProvider;
        _userService = userService;

        User = _userService.SelectedUser;
    }

    [ObservableProperty]
    private User user = new();


    

    [RelayCommand]
    private void NavigateToListView()
    {
        
        

        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<UserListViewModel>();
    }

    [RelayCommand]
    private void DeleteUser(User user)
    {
        MessageBoxResult result = MessageBox.Show("Är du säker på att du vill ta bort användaren?", "Bekräfta borttagning av användare", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            _userService.DeleteUser(user);

            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<UserListViewModel>();
        }
    }

    [RelayCommand]
    private void NavigateToEditUserView()
    {
        _userService.SelectedUser = user;

        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<EditUserViewModel>();
    }


}
