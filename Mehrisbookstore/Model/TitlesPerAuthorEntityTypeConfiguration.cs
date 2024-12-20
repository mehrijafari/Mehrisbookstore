using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mehrisbookstore;

public class TitlesPerAuthorEntityTypeConfiguration : IEntityTypeConfiguration<TitlesPerAuthor>
{
    public void Configure(EntityTypeBuilder<TitlesPerAuthor> builder)
    {
        builder
            .HasNoKey()
            .ToView("Titles Per Author");

        builder.Property(e => e.InventoryValue)
            .HasColumnType("decimal(38, 2)")
            .HasColumnName("Inventory value");
        builder.Property(e => e.Name).HasMaxLength(201);
    }
}
