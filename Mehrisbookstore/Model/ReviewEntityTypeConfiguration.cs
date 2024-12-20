using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mehrisbookstore;

public class ReviewEntityTypeConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__Reviews__3214EC2796AC13F5");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.CustomerId).HasColumnName("Customer ID");
        builder.Property(e => e.OriginalBookId).HasColumnName("OriginalBookID");
        builder.Property(e => e.ReviewText)
            .HasMaxLength(400)
            .HasColumnName("Review text");

        builder.HasOne(d => d.Customer).WithMany(p => p.Reviews)
            .HasForeignKey(d => d.CustomerId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Reviews__Custome__6383C8BA");

        builder.HasOne(d => d.OriginalBook).WithMany(p => p.Reviews)
            .HasForeignKey(d => d.OriginalBookId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Reviews_OriginalBook");
    }
}
