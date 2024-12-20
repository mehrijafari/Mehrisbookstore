using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mehrisbookstore;

public class PublisherEntityTypeConfiguration : IEntityTypeConfiguration<Publisher>
{
    public void Configure(EntityTypeBuilder<Publisher> builder)
    {
       
        builder.HasKey(e => e.Id).HasName("PK__Publishe__3214EC27CC64715D");

        builder.ToTable("Publisher");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.NameOfPublisher)
            .HasMaxLength(100)
            .HasColumnName("Name of publisher");
       
    }
}
