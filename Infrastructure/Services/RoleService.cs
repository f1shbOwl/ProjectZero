using Infrastructure.Entities;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class RoleService(RoleRepo roleRepo)
{
    private readonly RoleRepo _roleRepo = roleRepo;


    public RoleEntity CreateRole(string roleName)
    {
        var roleEntity = _roleRepo.GetOne(x => x.RoleName == roleName);
        roleEntity ??= _roleRepo.Create(new RoleEntity() { RoleName = roleName });

        return roleEntity;
    }

    public RoleEntity GetRoleByName(string roleName)
    {
        var roleEntity = _roleRepo.GetOne(x => x.RoleName == roleName);
        return roleEntity;
    }

    public RoleEntity GetRoleById(int Id)
    {
        var roleEntity = _roleRepo.GetOne(x => x.Id == Id);
        return roleEntity;
    }

    public IEnumerable<RoleEntity> GetRoles()
    {
        var roles = new List<RoleEntity>();
        return roles;
    }

    public RoleEntity UpdateRole(RoleEntity roleEntity)
    {
        var updatedRoleEntity = _roleRepo.Update(x => x.Id == roleEntity.Id, roleEntity);
        return updatedRoleEntity;
    }

    public void DeleteRole(int id)
    {
        _roleRepo.Delete(x => x.Id == id);
    }


}
