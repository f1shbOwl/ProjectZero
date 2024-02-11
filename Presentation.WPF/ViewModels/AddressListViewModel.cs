using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Dtos;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;

namespace Presentation.WPF.ViewModels;

public partial class AddressListViewModel : ObservableObject
{
    private readonly AddressService _addressService;
    private readonly IServiceProvider _serviceProvider;


    [ObservableProperty]
    private ObservableCollection<Address> addressList = [];

    [ObservableProperty]
    private Address address = new();


    public AddressListViewModel(AddressService addressService, IServiceProvider serviceProvider)
    {
        _addressService = addressService;
        _serviceProvider = serviceProvider;

        AddressList = new ObservableCollection<Address>(_addressService.GetAllAddresses());
    }






    [RelayCommand]
    private async Task AddAddress()
    {
        if (string.IsNullOrWhiteSpace(Address.StreetName) || string.IsNullOrWhiteSpace(Address.PostalCode) || string.IsNullOrWhiteSpace(Address.City))
        {
            MessageBox.Show("Address fields can not be empty, please enter a address", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }


        await _addressService.CreateAddressAsync(Address.StreetName, Address.PostalCode, Address.City);

        AddressList = new ObservableCollection<Address>(_addressService.GetAllAddresses());
        Address = new();

        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<AddressListViewModel>();
    }



    [RelayCommand]
    private void NavigateToUsers()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<UserListViewModel>();
    }

    [RelayCommand]
    private void NavigateToRoles()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<RoleListViewModel>();
    }


}
