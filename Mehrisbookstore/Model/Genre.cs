using System;
using System.Collections.Generic;

namespace Mehrisbookstore;

public partial class Genre
{
    public int Id { get; set; }

    public string NameOfGenre { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
