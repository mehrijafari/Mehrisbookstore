using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mehrisbookstore;

public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__Customer__3214EC2749886F01");

        builder.ToTable("Customer");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Birthdate).HasColumnName("birthdate");
        builder.Property(e => e.City).HasMaxLength(100);
        builder.Property(e => e.FirstName)
            .HasMaxLength(100)
            .HasColumnName("First name");
        builder.Property(e => e.LastName)
            .HasMaxLength(100)
            .HasColumnName("Last name");
        builder.Property(e => e.PhoneNumber)
            .HasMaxLength(10)
            .HasColumnName("Phone number");
        builder.Property(e => e.PostalCode).HasColumnName("Postal code");
        builder.Property(e => e.StreetName)
            .HasMaxLength(100)
            .HasColumnName("Street name");
    }
}
