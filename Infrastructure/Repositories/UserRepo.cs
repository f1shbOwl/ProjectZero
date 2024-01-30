using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;




public class UserRepo(UserContext userContext) : BaseRepo<UserEntity>(userContext)
{
    private readonly UserContext _userContext = userContext;

    public override IEnumerable<UserEntity> GetAll()
    {
        try
        {
            return _userContext.Users.Include(x => x.Role).Include(x => x.ContactInformation).Include(x => x.Authentication).Include(x => x.Address).ToList();

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public override UserEntity GetOne(Expression<Func<UserEntity, bool>> predicate)
    {
        try
        {
            return _userContext.Users.Include(x => x.Role).Include(x => x.ContactInformation).Include(x => x.Authentication).Include(x => x.Address).FirstOrDefault(predicate, null!);

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }
}
