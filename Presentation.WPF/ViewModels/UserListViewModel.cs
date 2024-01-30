using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Presentation.WPF.ViewModels;

internal class UserListViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;

    public UserListViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    
}
