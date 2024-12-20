using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mehrisbookstore;

public class GenreEntityTypeConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__Genre__3214EC274BC270D3");

        builder.ToTable("Genre");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.NameOfGenre)
            .HasMaxLength(50)
            .HasColumnName("Name of genre");
    }
}
