using Infrastructure.Entities;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class AddressService(AddressRepo addressRepo)
{
    private readonly AddressRepo _addressRepo = addressRepo;

    public AddressEntity CreateAddress(string streetName, string postalCode, string city)
    {
        var addressEntity = _addressRepo.GetOne(x => x.StreetName == streetName && x.PostalCode == postalCode && x.City == city);
        addressEntity ??= _addressRepo.Create(new AddressEntity() { StreetName = streetName, PostalCode = postalCode, City = city });

        return addressEntity;
    }

    public AddressEntity GetAddress(string streetName, string postalCode, string city)
    {
        var addressEntity = _addressRepo.GetOne(x => x.StreetName == streetName && x.PostalCode == postalCode && x.City == city);
        return addressEntity;
    }

    public AddressEntity GetAddressById(int Id)
    {
        var addressEntity = _addressRepo.GetOne(x => x.Id == Id);
        return addressEntity;
    }

    public IEnumerable<AddressEntity> GetAllAddresses()
    {
        var addresses = new List<AddressEntity>();
        return addresses;
    }

    public AddressEntity UpdateAddress(AddressEntity addressEntity)
    {
        var updatedAddressEntity = _addressRepo.Update(x => x.Id == addressEntity.Id, addressEntity);
        return updatedAddressEntity;
    }

    public void DeleteRole(int id)
    {
        _addressRepo.Delete(x => x.Id == id);
    }
}
