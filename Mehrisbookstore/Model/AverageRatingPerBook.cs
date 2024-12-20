using System;
using System.Collections.Generic;

namespace Mehrisbookstore;

public partial class AverageRatingPerBook
{
    public string OriginalTitle { get; set; } = null!;

    public string? AverageRating { get; set; }

    public string Genre { get; set; } = null!;

    public int? AmountOfPeopleWhoReadTheBook { get; set; }
}
