namespace Infrastructure.Dtos;

public class ContactInformation
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public Guid Userid { get; set; }
}
