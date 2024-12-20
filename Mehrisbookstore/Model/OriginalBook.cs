using System;
using System.Collections.Generic;

namespace Mehrisbookstore;

public partial class OriginalBook
{
    public int Id { get; set; }

    public string OriginalTitle { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();
}
