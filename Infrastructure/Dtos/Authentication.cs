using Infrastructure.Entities;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Dtos;

public class Authentication
{
    public Guid UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

}
