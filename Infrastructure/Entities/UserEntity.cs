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
    public int AddressId { get; set; }
    public virtual AddressEntity Address { get; set; } = null!;

    public int RoleId { get; set; }
    public virtual RoleEntity Role { get; set; } = null!;

    public virtual AuthenticationEntity Authentication { get; set; } = null!;
    public virtual ContactInformationEntity ContactInformation { get; set; } = null!;


}