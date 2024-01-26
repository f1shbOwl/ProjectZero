using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

public class ContactInformationEntity
{
    [Key]
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }

    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    public string? PhoneNumber { get; set; }

    public UserEntity User { get; set; } = null!;
}



