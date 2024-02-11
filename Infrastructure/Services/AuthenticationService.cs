using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.ComponentModel;
using System.Diagnostics;

namespace Infrastructure.Services;

public class AuthenticationService
{
    private readonly AuthenticationRepo _authenticationRepo;

    public AuthenticationService(AuthenticationRepo authenticationRepo)
    {
        _authenticationRepo = authenticationRepo;
    }


    public AuthenticationEntity CreateAuthentication(string username, string password, Guid userId)
    {
        try
        {
            var authenticationEntity = new AuthenticationEntity()
            {
                UserName = username,
                Password = password,
                UserId = userId
            };
            var result = _authenticationRepo.Create(authenticationEntity);
            if (result != null)
                return result;
        }
        catch (Exception ex)  {Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;

    }

    public AuthenticationEntity GetAuthByUserName(string username)
    {
        var authenticationEntity = _authenticationRepo.GetOne(x => x.UserName == username);
        return authenticationEntity;
    }

    public AuthenticationEntity GetAuthById(Guid userId)
    {
        var authenticationEntity = _authenticationRepo.GetOne(x => x.UserId == userId);
        return authenticationEntity;
    }

    public IEnumerable<AuthenticationEntity> GetRoles()
    {
        var authentications = new List<AuthenticationEntity>();
        return authentications;
    }

    public AuthenticationEntity UpdateRole(AuthenticationEntity authenticationEntity)
    {
        var updatedAuthentication = _authenticationRepo.Update(x => x.UserId == authenticationEntity.UserId, authenticationEntity);
        return updatedAuthentication;
    }



    public async Task<bool> UpdateAuthAsync(Guid userId, string userName, string password)
    {
        try
        {
            var newAuthentication = await _authenticationRepo.UpdateOneAsync(new AuthenticationEntity
            {
                UserId = userId,
                UserName = userName,
                Password = password
                
            });
            return newAuthentication != null;
        }
        catch
        {

        }
        return false!;
    }

    public void DeleteRole(Guid id)
    {
        _authenticationRepo.Delete(x => x.UserId == id);
    }
}
