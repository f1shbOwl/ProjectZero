using Infrastructure.Dtos;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Infrastructure.Contexts;

public partial class UserContext : DbContext
{
    public UserContext() 
    { 
    } 

    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
        
    }

    public virtual DbSet<UserEntity> Users { get; set; }
    public virtual DbSet<AddressEntity> Addresses { get; set; }
    public virtual DbSet<ContactInformationEntity> ContactInformation { get; set; }
    public virtual DbSet<RoleEntity> Roles { get; set; }
    public virtual DbSet<AuthenticationEntity> Authentication { get; set; }


}
