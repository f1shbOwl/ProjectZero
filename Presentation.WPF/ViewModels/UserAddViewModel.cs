using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Dtos;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.WPF.ViewModels;

public partial class UserAddViewModel : ObservableObject
{
    private readonly UserService _userService;
    private readonly IServiceProvider _serviceProvider;

    public UserAddViewModel(UserService userService, IServiceProvider serviceProvider)
    {
        _userService = userService;
        _serviceProvider = serviceProvider;
    }

    [ObservableProperty]
    private User user = new User();


    [RelayCommand]
    private async Task AddUser(User user)
    {
        await _userService.CreateUserAsync(User);

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
