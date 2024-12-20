using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mehrisbookstore;

public class OriginalBookEntityTypeConfiguration : IEntityTypeConfiguration<OriginalBook>
{
    public void Configure(EntityTypeBuilder<OriginalBook> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__BookWork__3214EC277AD14DC0");

        builder.ToTable("OriginalBook");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.OriginalTitle)
            .HasMaxLength(100)
            .HasColumnName("Original title");       
    }
}
