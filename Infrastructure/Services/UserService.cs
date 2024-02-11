using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Transactions;
using System.Xml.XPath;

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


    /// <summary>
    /// Används för att lagra datan och för att sedan kunna skicka den datan mellan vyer i WPF-appen
    /// </summary>
    public User SelectedUser { get; set; } = null!;



    /// <summary>
    /// Async Create One User
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<UserEntity> CreateUserAsync(User user)
    {
        try
        {
            if (!_userRepo.Exists(x => x.Email == user.Email))
            {
                var roleEntity =  await _roleService.CreateRoleAsync(user.RoleName);
                var addressEntity = _addressService.CreateAddress(user.StreetName, user.PostalCode, user.City);

                var userEntity = new UserEntity
                {
                    Email = user.Email,
                    RoleId = roleEntity.Id,
                    AddressId = addressEntity.Id
                };

                var userResult = await _userRepo.CreateAsync(userEntity);
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


    /// <summary>
    /// Create One User - no Async
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
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



    /// <summary>
    /// Lists all users
    /// </summary>
    /// <returns></returns>
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
                        Id = user.Id,
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

    /// <summary>
    /// Get user by Id
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public UserEntity GetUserById(User user)
    {
        var userEntity = _userRepo.GetOne(x => x.Id == user.Id);
        return userEntity;
    }

    /// <summary>
    /// Get user by Email
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public UserEntity GetUserByEmail(User user)
    {
        var userEntity = _userRepo.GetOne(x => x.Email == user.Email);
        return userEntity;
    }


    /// <summary>
    /// En metod jag lade till för att prova uppdatera endast email.
    /// </summary>
    /// <param name="updatedUser"></param>
    /// <returns></returns>
    public async Task<UserEntity> UpdateUserEmailAsync(User updatedUser)
    {
        try
        {
            var existingUserEntity = await _userRepo.GetOneAsync(x => x.Id == updatedUser.Id);

            if (existingUserEntity != null)
            {
                existingUserEntity.Email = updatedUser.Email;
                return await _userRepo.UpdateAsync(x => x.Id == updatedUser.Id, existingUserEntity);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("ERROR :: " + ex.Message);
        }

        return null!;
    }


    /// <summary>
    /// Update one user
    /// </summary>
    /// <param name="updatedUser"></param>
    /// <returns></returns>
    public async Task<UserEntity> UpdateUserAsync(User updatedUser)
    {
        try
        {
            var entity = await _userRepo.GetOneAsync(x => x.Id == updatedUser.Id);
            if (entity != null)
            {
                var roleEntity = _roleService.CreateRole(updatedUser.RoleName);
                var addressEntity = _addressService.CreateAddress(updatedUser.StreetName, updatedUser.PostalCode, updatedUser.City);

                entity.Email = updatedUser.Email;
                entity.AddressId = addressEntity.Id;
                entity.RoleId = roleEntity.Id;

                var result = await _userRepo.UpdateAsync(x => x.Id == updatedUser.Id, entity);
                if (result != null)
                    return new UserEntity
                    {
                        Id = updatedUser.Id,
                        Email = result.Email,
                        AddressId = result.AddressId,
                        RoleId = result.RoleId,
                    };
                var contactInformationEntity = _contactInformationService.CreateContactInfo(updatedUser.FirstName, updatedUser.LastName, updatedUser.PhoneNumber!, updatedUser.Id);
                var authenticationEntity = _authenticationService.CreateAuthentication(updatedUser.UserName, updatedUser.Password, updatedUser.Id);
            }
        }
        catch { }
        return null!;
    }


    public void DeleteUser(User user)
    {
        _userRepo.Delete(x => x.Email == user.Email);
        
    }

}
