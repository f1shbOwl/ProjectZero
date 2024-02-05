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



    /// <summary>
    /// hmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm, hur ska denna skrivas?????????
    /// </summary>
    [RelayCommand]
    private void Update()
    {
        _userService.UpdateUser(User);

        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<UserDetailViewModel>();

    }





}
