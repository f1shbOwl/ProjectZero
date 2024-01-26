using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class ContactInformationRepo(UserContext userContext) : BaseRepo<ContactInformationEntity>(userContext)
{
    private readonly UserContext _userContext = userContext;
}
