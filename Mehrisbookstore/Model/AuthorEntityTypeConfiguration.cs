using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mehrisbookstore;

public class AuthorEntityTypeConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__Authors__3214EC27D89DB599");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.BirthDate).HasColumnName("Birth date");
        builder.Property(e => e.FirstName)
            .HasMaxLength(100)
            .HasColumnName("First name");
        builder.Property(e => e.LastName)
            .HasMaxLength(100)
            .HasColumnName("Last name");

        builder.HasMany(d => d.OriginalBooks).WithMany(p => p.Authors)
            .UsingEntity<Dictionary<string, object>>(
                "AuthorBook",
                r => r.HasOne<OriginalBook>().WithMany()
                    .HasForeignKey("OriginalBookId")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AuthorBook_OriginalBook"),
                l => l.HasOne<Author>().WithMany()
                    .HasForeignKey("AuthorId")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AuthorBoo__Autho__68487DD7"),
                j =>
                {
                    j.HasKey("AuthorId", "OriginalBookId");
                    j.ToTable("AuthorBook");
                    j.IndexerProperty<int>("AuthorId").HasColumnName("Author ID");
                    j.IndexerProperty<int>("OriginalBookId").HasColumnName("OriginalBookID");
                });
    }
}
