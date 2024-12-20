using System;
using System.Collections.Generic;

namespace Mehrisbookstore;

public partial class Customer
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? StreetName { get; set; }

    public string? City { get; set; }

    public int? PostalCode { get; set; }

    public string? PhoneNumber { get; set; }

    public DateOnly? Birthdate { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
