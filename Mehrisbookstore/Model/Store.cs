using System;
using System.Collections.Generic;

namespace Mehrisbookstore;

public partial class Store
{
    public int Id { get; set; }

    public string StoreName { get; set; } = null!;

    public string? Country { get; set; }

    public string StreetName { get; set; } = null!;

    public string City { get; set; } = null!;

    public string? PostalCode { get; set; }

    public virtual ICollection<InventoryBalance> InventoryBalances { get; set; } = new List<InventoryBalance>();
}
