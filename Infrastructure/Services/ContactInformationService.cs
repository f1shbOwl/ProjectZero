using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Infrastructure.Services;

public class ContactInformationService(ContactInformationRepo contactInformationRepo)
{
    private readonly ContactInformationRepo _contactInformationRepo = contactInformationRepo;
    

    public ContactInformationEntity CreateContactInfo(string firstName, string lastName, string phoneNumber, Guid userId)
    {
        
        var contactInformationEntity = new ContactInformationEntity
        {
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            UserId = userId,
        };

        return _contactInformationRepo.Create(contactInformationEntity);
    }

    public ContactInformationEntity GetContactInfo(ContactInformation contactInformation)
    {
        var contactInformationEntity = _contactInformationRepo.GetOne(x => x.FirstName == contactInformation.FirstName && x.LastName == contactInformation.LastName && x.PhoneNumber == x.PhoneNumber);
        return contactInformationEntity;
    }

    public ContactInformationEntity GetContactInfoByUserId(Guid UserId)
    {
        var contactInformationEntity = _contactInformationRepo.GetOne(x => x.UserId == UserId);
        return contactInformationEntity;
    }


    public ContactInformationEntity UpdateContactInfo(ContactInformationEntity contactInformationEntity)
    {
        var updatedContactInfo = _contactInformationRepo.Update(x => x.UserId == contactInformationEntity.UserId, contactInformationEntity);
        return updatedContactInfo;
    }

    public void DeleteContactInfo(Guid Userid)
    {
        _contactInformationRepo.Delete(x => x.UserId == Userid);
    }
}
