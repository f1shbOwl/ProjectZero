﻿namespace Infrastructure.Dtos;

public class Address
{
    public int Id { get; set; }

    public string StreetName { get; set; } = null!;

    public string City { get; set; } = null!;

    public string PostalCode { get; set; } = null!; 
}
