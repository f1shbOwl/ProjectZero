﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Dtos;

namespace Infrastructure.Entities;

public class AuthenticationEntity
{
    [Key]
    [ForeignKey(nameof(UserEntity))]
    public Guid UserId { get; set; }

    [Required]
    public string UserName { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    public virtual UserEntity User { get; set; } = null!;
}



