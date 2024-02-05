using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Transactions;

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




    public async Task <UserEntity> CreateUserAsync(User user)
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

    public UserEntity GetUserById(Guid userId)
    {
        var userEntity = _userRepo.GetOne(x => x.Id == userId);
        return userEntity;
    }

    public UserEntity GetUserByEmail(string email)
    {
        var userEntity = _userRepo.GetOne(x => x.Email == email);
        return userEntity;
    }


    public async Task <User> UpdateUserEmailAsync(User updatedUser)
    {
        try
        {
            var userEntity = new UserEntity { Email =  updatedUser.Email };
            var updatedUserEntity = await _userRepo.UpdateAsync(x => x.Id == userEntity.Id, userEntity);
            if (userEntity != null)
            {
                var user = new User { Email = updatedUser.Email };
                return user;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("ERROR :: " + ex.Message);
        }
        return null!;
    }


    public async Task<UserEntity> UpdateUserAsync(User updatedUser)
    {
        try
        {
            var existingUserEntity = await _userRepo.GetOneAsync(x => x.Id == updatedUser.Id);

            if (existingUserEntity != null)
            {
                existingUserEntity.Email = updatedUser.Email;
                existingUserEntity.ContactInformation.FirstName = updatedUser.FirstName;
                existingUserEntity.ContactInformation.LastName = updatedUser.LastName;
                existingUserEntity.ContactInformation.PhoneNumber = updatedUser.PhoneNumber ?? existingUserEntity.ContactInformation.PhoneNumber;

                existingUserEntity.Address.StreetName = updatedUser.StreetName;
                existingUserEntity.Address.PostalCode = updatedUser.PostalCode;
                existingUserEntity.Address.City = updatedUser.City;

                existingUserEntity.Role.RoleName = updatedUser.RoleName;

                existingUserEntity.Authentication.UserName = updatedUser.UserName;
                existingUserEntity.Authentication.Password = updatedUser.Password;

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
    /// Den här metoden gör vad den men Email sparas inte till databasen.
    /// </summary>
    /// <param name="updatedUser"></param>
    /// <returns></returns>
    //public async Task<User> UpdateUserAsync(User updatedUser)
    //{
    //    try
    //    {
    //        var existingUserEntity = await _userRepo.GetOneAsync(x => x.Email == updatedUser.Email);

    //        if (existingUserEntity != null)
    //        {
    //            existingUserEntity.Email = updatedUser.Email;
    //            existingUserEntity.ContactInformation.FirstName = updatedUser.FirstName;
    //            existingUserEntity.ContactInformation.LastName = updatedUser.LastName;
    //            existingUserEntity.ContactInformation.PhoneNumber = updatedUser.PhoneNumber;

    //            existingUserEntity.Address.StreetName = updatedUser.StreetName;
    //            existingUserEntity.Address.PostalCode = updatedUser.PostalCode;
    //            existingUserEntity.Address.City = updatedUser.City;

    //            existingUserEntity.Role.RoleName = updatedUser.RoleName;

    //            existingUserEntity.Authentication.UserName = updatedUser.UserName;
    //            existingUserEntity.Authentication.Password = updatedUser.Password;

    //            await _userRepo.UpdateAsync(x => x.Id == updatedUser.Id, existingUserEntity);


    //            var updatedUserDto = new User
    //            {
    //                Email = existingUserEntity.Email,
    //                FirstName = existingUserEntity.ContactInformation.FirstName,
    //                LastName = existingUserEntity.ContactInformation.LastName,
    //                PhoneNumber = existingUserEntity.ContactInformation.PhoneNumber,
    //                StreetName = existingUserEntity.Address.StreetName,
    //                PostalCode = existingUserEntity.Address.PostalCode,
    //                City = existingUserEntity.Address.City,
    //                RoleName = existingUserEntity.Role.RoleName,
    //                UserName = existingUserEntity.Authentication.UserName,
    //                Password = existingUserEntity.Authentication.Password,
    //            };

    //            return updatedUserDto;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.WriteLine("ERROR :: " + ex.Message);
    //    }

    //    return null!;
    //}



    //Metoden nedan no good

    //public User UpdateUser(User updatedUser)
    //{
    //    try
    //    {
    //        var userEntity = new UserEntity { Email = updatedUser.Email};
    //        var updatedUserEntity = _userRepo.Update(x => x.Email == updatedUser.Email, userEntity);

    //        if (updatedUser != null)
    //        {
    //            var user = new User
    //            {
    //                Email = updatedUserEntity.Email,
    //                FirstName = updatedUserEntity.ContactInformation.FirstName,
    //                LastName = updatedUserEntity.ContactInformation.LastName,
    //                PhoneNumber = updatedUserEntity.ContactInformation.PhoneNumber,
    //                StreetName = updatedUserEntity.Address.StreetName,
    //                PostalCode = updatedUserEntity.Address.PostalCode,
    //                City = updatedUserEntity.Address.City,
    //                RoleName = updatedUserEntity.Role.RoleName,
    //                UserName = updatedUserEntity.Authentication.UserName,
    //                Password = updatedUserEntity.Authentication.Password,
    //            };
    //            return updatedUser;
    //            //return user;
    //        }
    //    }
    //    catch { }
    //    return null!;
    //}



    /// <summary>
    /// den här metoden fungerar inte med hur min constructor i User.cs ser ut, behöver isf lägga till en standardkonstruktor med de olika stringvärdena.
    /// Då fungerar metoden men databasen uppdateras ändå inte. I appen ser det OK ut tills det att jag går tillbaka till listview och sedan in i detailview igen. 
    /// </summary>
    /// <param name="updatedUser"></param>
    /// <returns></returns>
    //public User UpdateUser(User updatedUser)
    //{
    //    try
    //    {
    //        var userEntity = _userRepo.GetOne(x => x.Email == updatedUser.Email);
    //        var updatedUserEntity = _userRepo.Update(x => x.Email == updatedUser.Email, userEntity);

    //        if (userEntity != null)
    //        {
    //            var user = new User
    //                (
    //                updatedUserEntity.Email,
    //                updatedUserEntity.ContactInformation.FirstName,
    //                updatedUserEntity.ContactInformation.LastName,
    //                updatedUserEntity.ContactInformation.PhoneNumber,
    //                updatedUserEntity.Address.StreetName,
    //                updatedUserEntity.Address.City,
    //                updatedUserEntity.Address.PostalCode,
    //                updatedUserEntity.Authentication.UserName,
    //                updatedUserEntity.Authentication.Password,
    //                updatedUserEntity.Role.RoleName
    //                );
    //            return user;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.WriteLine("ERROR :: " + ex.Message);
    //    }

    //    return null!;
    //}



    // Den här metoden uppdaterar också i edit, resetas när man går tillbaka till list och in igen i detailview. Databasen ej uppdaterad.

    //public async Task<User> UpdateUserAsync(User updatedUser)
    //{
    //    try
    //    {
    //        var userEntity = new UserEntity { Email = updatedUser.Email };
    //        var updatedUserEntity = await _userRepo.UpdateAsync(x => x.Id == updatedUser.Id, userEntity);

    //        if (updatedUserEntity != null)
    //        {
    //            var user = new User
    //            {
    //                FirstName = updatedUser.FirstName,
    //                LastName = updatedUser.LastName,
    //                PhoneNumber = updatedUser.PhoneNumber,
    //                Email = updatedUser.Email,
    //                StreetName = updatedUser.StreetName,
    //                City = updatedUser.City,
    //                PostalCode = updatedUser.PostalCode,
    //                UserName = updatedUser.UserName,
    //                RoleName = updatedUser.RoleName,
    //                Password = updatedUser.Password,
    //            };
    //            return user;
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.WriteLine("ERROR :: " + ex.Message);
    //    }
    //    return null!;
    //}



    //public UserEntity UpdateUser(UserEntity userEntity)
    //{
    //    if (userEntity != null)
    //    {
    //        var updatedUserEntity = _userRepo.Update(x => x.Email == userEntity.Email, userEntity);
    //        return updatedUserEntity;
    //    }
    //    return null!;
    //}

    public void DeleteUser(User user)
    {
        _userRepo.Delete(x => x.Email == user.Email);
        
    }

}
