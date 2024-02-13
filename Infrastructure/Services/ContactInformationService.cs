﻿using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Infrastructure.Services;

public class ContactInformationService(ContactInformationRepo contactInformationRepo)
{
    private readonly ContactInformationRepo _contactInformationRepo = contactInformationRepo;



    public async Task<ContactInformation> CreateContactInfoAsync(string firstName, string lastName, string phoneNumber, Guid userId)
    {
        try
        {
            var result = await _contactInformationRepo.GetOneAsync(x => x.UserId == userId);
            result ??= await _contactInformationRepo.CreateAsync(new ContactInformationEntity { FirstName = firstName, LastName = lastName, PhoneNumber = phoneNumber, UserId = userId });

            return new ContactInformation { Userid = result.UserId, FirstName = result.FirstName, LastName = result.LastName, PhoneNumber = result.PhoneNumber };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }



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


    public async Task<bool> UpdateContactInfoAsync(Guid userId, string firstName, string lastName, string? phoneNumber)
    {
        try
        {
            var newContactInformation = await _contactInformationRepo.UpdateOneAsync(new ContactInformationEntity
            {
                UserId = userId,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber
            });
            return newContactInformation != null;
        }
        catch
        {

        }
        return false!;
    }


}
