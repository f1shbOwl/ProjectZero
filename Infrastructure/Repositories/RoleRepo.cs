using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class RoleRepo(UserContext userContext) : BaseRepo<RoleEntity>(userContext)
{
    private readonly UserContext _userContext = userContext;
}
