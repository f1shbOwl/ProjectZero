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




    //public async Task <User> CreateUserAsync(User user)
    //{
    //    try
    //    {
    //        var result = await _userRepo.GetOneAsync(x => x.Email == user.Email);
    //        if (result == null)
    //        {
    //            var role = await _roleService.CreateRoleAsync(user.RoleName);
    //            var address = _addressService.CreateAddress(user.StreetName, user.PostalCode, user.City);

    //            var userEntity = new UserEntity
    //            {
    //                Email = user.Email,
    //                RoleId = role.Id,
    //                AddressId = address.Id
    //            };

    //            result = await _userRepo.CreateAsync(userEntity);
    //            if (result != null)
    //                return new User
    //                {
    //                    Id = result.Id,
    //                    Email = result.Email,
    //                    Role = new Role
    //                    {
    //                        Id = result.Role.Id,
    //                        RoleName = result.Role.RoleName
    //                    },
    //                    Address = new Address
    //                    {
    //                        Id = result.Address.Id,
    //                        StreetName = result.Address.StreetName,
    //                        PostalCode = result.Address.PostalCode,
    //                        City = result.Address.City
    //                    },
    //                    ContactInformation = new ContactInformation
    //                    {
    //                        Userid = result.ContactInformation.UserId,
    //                        FirstName = result.ContactInformation.FirstName,
    //                        LastName = result.ContactInformation.LastName,
    //                        PhoneNumber = result.ContactInformation.PhoneNumber,
    //                    },
    //                    Authentication = new Authentication
    //                    {
    //                        UserId = result.Authentication.UserId,
    //                        UserName = result.Authentication.UserName,
    //                        Password = result.Authentication.Password,
    //                    }
    //                };
    //        }
    //    }
    //    catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
    //    return null!;
    //}


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




    //public async Task <User> UpdateUserAsync(User updatedUser)
    //{
    //    try
    //    {
    //        var entity = await _userRepo.GetOneAsync(x => x.Id == updatedUser.Id);
    //        if (entity != null)
    //        {
    //            var role = await _roleService.CreateRoleAsync(updatedUser.RoleName);
    //            var address = await _addressService.CreateAddressAsync(updatedUser.StreetName, updatedUser.PostalCode, updatedUser.City);

    //            entity.Email = updatedUser.Email;
    //            entity.RoleId = updatedUser.RoleId;
    //            entity.AddressId = updatedUser.AddressId;

    //            var result = await _userRepo.UpdateAsync(x => x.Id == updatedUser.Id, entity);
    //            if (result != null)
    //                return new User
    //                {
    //                    Id = result.Id,
    //                    Email = result.Email,
    //                    Role = new Role
    //                    {
    //                        Id = result.Role.Id,
    //                        RoleName = result.Role.RoleName,
    //                    },
    //                    Address = new Address
    //                    {
    //                        Id = result.Address.Id,
    //                        StreetName = result.Address.StreetName,
    //                        PostalCode = result.Address.PostalCode,
    //                        City = result.Address.City,
    //                    },
    //                    ContactInformation = new ContactInformation
    //                    {
    //                        Userid = result.ContactInformation.UserId,
    //                        PhoneNumber = result.ContactInformation.PhoneNumber,
    //                        FirstName = result.ContactInformation.FirstName,
    //                        LastName = result.ContactInformation.LastName,
    //                    },
    //                    Authentication = new Authentication
    //                    {
    //                        UserId = result.Authentication.UserId,
    //                        UserName = result.Authentication.UserName,
    //                        Password = result.Authentication.Password,
    //                    }
    //                };
    //        }
    //    }
    //    catch { }
    //    return null!;
    //}



    public async Task<UserEntity> UpdateUserAsync(User updatedUser)
    {
        try
        {
            var existingUserEntity = await _userRepo.GetOneAsync(x => x.Id == updatedUser.Id);

            if (existingUserEntity != null)
            {
                existingUserEntity.Role.RoleName = updatedUser.RoleName;

                existingUserEntity.Address.StreetName = updatedUser.StreetName;
                existingUserEntity.Address.PostalCode = updatedUser.PostalCode;
                existingUserEntity.Address.City = updatedUser.City;

                existingUserEntity.ContactInformation.FirstName = updatedUser.FirstName;
                existingUserEntity.ContactInformation.LastName = updatedUser.LastName;
                existingUserEntity.ContactInformation.PhoneNumber = updatedUser.PhoneNumber ?? existingUserEntity.ContactInformation.PhoneNumber;

                existingUserEntity.Authentication.UserName = updatedUser.UserName;
                existingUserEntity.Authentication.Password = updatedUser.Password;

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


    public void DeleteUser(User user)
    {
        _userRepo.Delete(x => x.Email == user.Email);
        
    }

}
