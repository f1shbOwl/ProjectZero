using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;


namespace Infrastructure.Services;

public class UserService (UserRepo userRepo, AddressRepo addressRepo, AuthenticationRepo authenticationRepo, ContactInformationRepo contactInformationRepo, RoleRepo roleRepo)
{

    private readonly UserRepo _userRepo = userRepo;
    private readonly AddressRepo _addressRepo = addressRepo;
    private readonly AuthenticationRepo _authenticationRepo = authenticationRepo;
    private readonly ContactInformationRepo _contactInformationRepo = contactInformationRepo;
    private readonly RoleRepo _roleRepo = roleRepo;


    public bool CreateUser(User user)
    {
        if (!_userRepo.Exists(x => x.Email == user.Email))
        {
            var roleEntity = new RoleEntity
            {
                RoleName = user.RoleName,
                
            };
            var roleResult = _roleRepo.Create(roleEntity);



            var addressEntity = new AddressEntity
            {
                StreetName = user.StreetName,
                PostalCode = user.PostalCode,
                City = user.City,
            };
            var addressResult = _addressRepo.Create(addressEntity);


            var userEntity = new UserEntity
            {
                Email = user.Email,
                AddressId = addressResult.Id,
                RoleId = roleResult.Id,

            };
            var userResult = _userRepo.Create(userEntity);



            var contactInformationEntity = new ContactInformationEntity
            {
                UserId = userResult.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,

            };
            var contactInformationResult = _contactInformationRepo.Create(contactInformationEntity);



            var authenticationEntity = new AuthenticationEntity
            {
                UserId = userResult.Id,
                UserName = user.UserName,
                Password = user.Password,
            };
            var authenticationResult = _authenticationRepo.Create(authenticationEntity);

            
            if (authenticationResult != null) 
                return true;
        }
        return false;

        
    }

    public IEnumerable<User> GetAllUsers()
    {
        var result = _userRepo.GetAll();
        var users = new List<User>();
        foreach (var user in result)
            users.Add(new User
            {
                Email = user.Email,
                FirstName = user.ContactInformation.FirstName,
                LastName = user.ContactInformation.LastName,
                PhoneNumber = user.ContactInformation.PhoneNumber,
                StreetName = user.Address.StreetName,
                City = user.Address.City,
                RoleName = user.Role.RoleName
            });
        return users;
    }


}
