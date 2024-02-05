using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Dtos;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.WPF.ViewModels;

public partial class EditUserViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly UserService _userService;

    public EditUserViewModel(IServiceProvider serviceProvider, UserService userService)
    {
        _serviceProvider = serviceProvider;
        _userService = userService;

        User = _userService.SelectedUser;

    }

    [ObservableProperty]
    private User user = new();



    [RelayCommand]
    private async Task Update()
    {
        await _userService.UpdateUserAsync(User);

        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<UserDetailViewModel>();

    }

    [RelayCommand]
    private async Task UpdateUserEmail()
    {
        await _userService.UpdateUserEmailAsync(User);

        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<UserDetailViewModel>();

    }





}
