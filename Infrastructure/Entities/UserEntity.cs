using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;




[Index(nameof(Email), IsUnique = true)]
public class UserEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(AddressEntity.Id))]
    public int AddressId { get; set; }
}

public class AddressEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string StreetName { get; set; } = null!;

    [Required]
    public string City { get; set; } = null!;

    [Required]
    public string PostalCode { get; set; } = null!;

    public virtual ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
}

public class ContactInformationEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    [ForeignKey(nameof(UserEntity.Id))]
    public int UserId { get; set; }

}

public class UserRolesEntity
{
    [Key]
    [ForeignKey(nameof(UserEntity.Id))]
    public int UserId { get; set; }

    public int RoleId { get; set; }

}

public class RolesEntity
{
    [Key]
    [ForeignKey(nameof(UserRolesEntity.UserId))]
    public int Id { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<UserRolesEntity> Roles { get; set; } = null!;
}



