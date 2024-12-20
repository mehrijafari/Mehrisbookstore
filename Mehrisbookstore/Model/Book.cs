using System;
using System.Collections.Generic;

namespace Mehrisbookstore;

public partial class Book
{
    public string Isbn13 { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Language { get; set; } = null!;

    public decimal Price { get; set; }

    public DateOnly? PublishDate { get; set; }

    public int? Pages { get; set; }

    public int? PublisherId { get; set; }

    public int? OriginalTitleId { get; set; }

    public virtual ICollection<InventoryBalance> InventoryBalances { get; set; } = new List<InventoryBalance>();

    public virtual OriginalBook? OriginalTitle { get; set; }

    public virtual Publisher? Publisher { get; set; }

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
}
