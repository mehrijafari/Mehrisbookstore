using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mehrisbookstore;

public class AverageRatingPerBookEntityTypeConfiguration : IEntityTypeConfiguration<AverageRatingPerBook>
{
    public void Configure(EntityTypeBuilder<AverageRatingPerBook> builder)
    {
            builder
                .HasNoKey()
                .ToView("Average rating per book");

            builder.Property(e => e.AmountOfPeopleWhoReadTheBook).HasColumnName("Amount of people who read the book");
            builder.Property(e => e.AverageRating)
                .HasMaxLength(4000)
                .HasColumnName("Average rating");
            builder.Property(e => e.Genre).HasMaxLength(50);
            builder.Property(e => e.OriginalTitle)
                .HasMaxLength(100)
                .HasColumnName("Original title");
    }
}
