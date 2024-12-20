using System;
using System.Collections.Generic;

namespace Mehrisbookstore;

public partial class Review
{
    public int Id { get; set; }

    public int Rating { get; set; }

    public int CustomerId { get; set; }

    public string? ReviewText { get; set; }

    public int OriginalBookId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual OriginalBook OriginalBook { get; set; } = null!;
}
