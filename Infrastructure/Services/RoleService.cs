using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Services;

public class RoleService(RoleRepo roleRepo)
{
    private readonly RoleRepo _roleRepo = roleRepo;





    public async Task<Role> CreateRoleAsync(string roleName)
    {
        try
        {
            var result = await _roleRepo.GetOneAsync(x => x.RoleName == roleName);
            result ??= await _roleRepo.CreateAsync(new RoleEntity { RoleName = roleName });

            return new Role { Id = result.Id, RoleName = result.RoleName };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }


    public RoleEntity CreateRole(string roleName)
    {
        var result = _roleRepo.GetOne(x => x.RoleName == roleName);
        result ??= _roleRepo.Create(new RoleEntity() { Id = result!.Id, RoleName = result.RoleName });

        return new RoleEntity { Id = result.Id, RoleName = result.RoleName };
    }


    public async Task<Role> GetRoleAsync(Expression<Func<RoleEntity, bool>> predicate)
    {
        try
        {
            var result = await _roleRepo.GetOneAsync(predicate);
            if (result != null)
                return new Role { Id = result.Id, RoleName = result.RoleName };
        }
        catch { }
        return null!;
    }

    public RoleEntity GetRoleByName(Role role)
    {
        var roleEntity = _roleRepo.GetOne(x => x.RoleName == role.RoleName);
        return roleEntity;
    }

    public RoleEntity GetRoleById(int Id)
    {
        var roleEntity = _roleRepo.GetOne(x => x.Id == Id);
        return roleEntity;
    }


    public IEnumerable<Role> GetAllRoles()
    {
        var roles = new List<Role>();
        try
        {
            var result = _roleRepo.GetAll();

            if (result != null)
            {
                foreach (var role in result)
                    roles.Add(new Role
                    {
                        Id = role.Id,
                        RoleName = role.RoleName
                    });
            }
            return roles;

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;


    }

    public IEnumerable<RoleEntity> GetRoles()
    {
        var roles = new List<RoleEntity>();
        return roles;
    }


    public async Task <Role> UpdateRoleAsync(Role updatedRole)
    {
        try
        {
            var entity = await _roleRepo.GetOneAsync(x => x.Id == updatedRole.Id);
            if (entity != null)
            {
                entity.RoleName = updatedRole.RoleName!;

                var result = await _roleRepo.UpdateAsync(x => x.Id == updatedRole.Id, entity);
                if (result != null)
                    return new Role { Id = updatedRole.Id, RoleName = updatedRole.RoleName };
            }
        }
        catch { }
        return null!;
    }


    public RoleEntity UpdateRole(RoleEntity roleEntity)
    {
        var updatedRoleEntity = _roleRepo.Update(x => x.Id == roleEntity.Id, roleEntity);
        return updatedRoleEntity;
    }

    public void DeleteRole(Role role)
    {
        _roleRepo.Delete(x => x.RoleName == role.RoleName);
    }


}
