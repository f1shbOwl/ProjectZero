using Infrastructure.Entities;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Dtos;

public class User
{
    public Guid Id { get; set; }
    public string RoleName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string StreetName { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;

    //public Role Role { set; get; } = null!;
    //public Address Address { set; get; } = null!;
    //public Authentication Authentication{ set; get; } = null!;
    //public ContactInformation ContactInformation { set; get; } = null!;

}
 