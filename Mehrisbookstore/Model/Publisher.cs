using System;
using System.Collections.Generic;

namespace Mehrisbookstore;

public partial class Publisher
{
    public int Id { get; set; }

    public string NameOfPublisher { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
