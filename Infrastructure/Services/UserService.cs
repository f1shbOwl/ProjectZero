using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Diagnostics;

namespace Infrastructure.Services;

public class UserService
{
    private readonly UserRepo _userRepo;
    private readonly RoleService _roleService;
    private readonly AddressService _addressService;
    private readonly AuthenticationService _authenticationService;
    private readonly ContactInformationService _contactInformationService;
    

    public UserService(UserRepo userRepo, RoleService roleService, AddressService addressService, AuthenticationService authenticationService, ContactInformationService contactInformationService)
    {
        _userRepo = userRepo;
        _roleService = roleService;
        _addressService = addressService;
        _authenticationService = authenticationService;
        _contactInformationService = contactInformationService;
    }

    public UserEntity CreateUser(User user)
    {
        try
        {
            if (!_userRepo.Exists(x => x.Email == user.Email))
            {
                var roleEntity = _roleService.CreateRole(user.RoleName);
                var addressEntity = _addressService.CreateAddress(user.StreetName, user.PostalCode, user.City);

                var userEntity = new UserEntity
                {
                    Email = user.Email,
                    RoleId = roleEntity.Id,
                    AddressId = addressEntity.Id
                };

                var userResult = _userRepo.Create(userEntity);

                if (userResult != null)
                {
                    var contactInformationEntity = _contactInformationService.CreateContactInfo(user.FirstName, user.LastName, user.PhoneNumber!, userResult.Id);
                    var authenticationEntity = _authenticationService.CreateAuthentication(user.UserName, user.Password, userResult.Id);
                }
                return userResult!;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }




    public IEnumerable<User> GetAllUsers()
    {
        var users = new List<User>();
        try
        {
            var result = _userRepo.GetAll();

            if (result != null)
            {
                foreach (var user in result)
                    users.Add(new User
                    {
                        FirstName = user.ContactInformation.FirstName,
                        LastName = user.ContactInformation.LastName,
                        Email = user.Email,
                        RoleName = user.Role.RoleName,
                        PhoneNumber = user.ContactInformation.PhoneNumber,
                        StreetName = user.Address.StreetName,
                        City = user.Address.City,
                        PostalCode = user.Address.PostalCode,
                        UserName = user.Authentication.UserName,
                        Password = user.Authentication.Password,
                    });
            }
            return users;

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;


    }

    public UserEntity GetUserById(Guid userId)
    {
        var userEntity = _userRepo.GetOne(x => x.Id == userId);
        return userEntity;
    }

    public UserEntity GetUserByEmail(User user)
    {
        var userEntity = _userRepo.GetOne(x => x.Email == user.Email);
        return userEntity;
    }


    public UserEntity UpdateUser(UserEntity userEntity)
    {
        var updatedUserEntity = _userRepo.Update(x => x.Email == userEntity.Email, userEntity);
        return updatedUserEntity;

    }

    public void DeleteUser(User user)
    {
        _userRepo.Delete(x => x.Email == user.Email);
    }

}
