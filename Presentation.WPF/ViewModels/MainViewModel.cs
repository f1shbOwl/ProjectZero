using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.WPF.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;

    public MainViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        CurrentViewModel = _serviceProvider.GetRequiredService<UserListViewModel>();

    }

    [ObservableProperty]
    private ObservableObject _currentViewModel = null!;

}
