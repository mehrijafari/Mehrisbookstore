using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mehrisbookstore;

public class InventoryBalanceEntityTypeConfiguration : IEntityTypeConfiguration<InventoryBalance>
{
    public void Configure(EntityTypeBuilder<InventoryBalance> builder)
    {
        builder.HasKey(e => new { e.StoreId, e.Isbn }).HasName("PK__Inventor__3477B63294406F38");

        builder.ToTable("InventoryBalance");

        builder.Property(e => e.StoreId).HasColumnName("Store ID");
        builder.Property(e => e.Isbn)
            .HasMaxLength(13)
            .HasColumnName("ISBN");

        builder.HasOne(d => d.IsbnNavigation).WithMany(p => p.InventoryBalances)
            .HasForeignKey(d => d.Isbn)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__InventoryB__ISBN__5AEE82B9");

        builder.HasOne(d => d.Store).WithMany(p => p.InventoryBalances)
            .HasForeignKey(d => d.StoreId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Inventory__Store__59FA5E80");       
    }
}
