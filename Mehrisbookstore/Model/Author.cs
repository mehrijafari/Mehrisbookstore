using System;
using System.Collections.Generic;

namespace Mehrisbookstore;

public partial class Author
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly? BirthDate { get; set; }

    public virtual ICollection<OriginalBook> OriginalBooks { get; set; } = new List<OriginalBook>();
}
