using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mehrisbookstore;

public class StoreEntityTypeConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        
        builder.HasKey(e => e.Id).HasName("PK__Stores__3214EC27F3500ADA");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.City).HasMaxLength(50);
        builder.Property(e => e.Country).HasMaxLength(100);
        builder.Property(e => e.PostalCode)
            .HasMaxLength(50)
            .HasColumnName("Postal code");
        builder.Property(e => e.StoreName)
            .HasMaxLength(100)
            .HasColumnName("Store name");
        builder.Property(e => e.StreetName)
            .HasMaxLength(100)
            .HasColumnName("Street name");
        
    }
}
