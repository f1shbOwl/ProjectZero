using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Linq.Expressions;

namespace Infrastructure.Services;

public class AddressService(AddressRepo addressRepo)
{
    private readonly AddressRepo _addressRepo = addressRepo;



    // Create Address
    public async Task<Address> CreateAddressAsync(string streetName, string postalCode, string city)
    {
        try
        {
            var result = await _addressRepo.GetOneAsync(x => x.StreetName == streetName && x.PostalCode == postalCode && x.City == city);
            result ??= await _addressRepo.CreateAsync(new AddressEntity { StreetName = streetName, PostalCode = postalCode, City = city });

            return new Address { Id = result.Id, StreetName = result.StreetName, PostalCode = result.PostalCode, City = result.City };
        }
        catch { }
        return null!;
    }


    public AddressEntity CreateAddress(string streetName, string postalCode, string city)
    {
        var result = _addressRepo.GetOne(x => x.StreetName == streetName && x.PostalCode == postalCode && x.City == city);
        result ??= _addressRepo.Create(new AddressEntity { StreetName = streetName, PostalCode = postalCode, City = city });

        return new AddressEntity { Id = result.Id, StreetName = result.StreetName, PostalCode = result.PostalCode, City = result.City };
    }
    //public AddressEntity CreateAddress(string streetName, string postalCode, string city)
    //{
    //    var addressEntity = _addressRepo.GetOne(x => x.StreetName == streetName && x.PostalCode == postalCode && x.City == city);
    //    addressEntity ??= _addressRepo.Create(new AddressEntity { StreetName = streetName, PostalCode = postalCode, City = city });

    //    return addressEntity;
    //}


    //Get One Address

    public async Task <Address> GetAddressAsync(Expression<Func<AddressEntity, bool>> predicate)
    {
        try
        {
            var result = await _addressRepo.GetOneAsync(predicate);
            if (result != null)
                return new Address { Id = result.Id, StreetName = result.StreetName, PostalCode = result.PostalCode, City = result.City };
        }
        catch { }
        return null!;
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
