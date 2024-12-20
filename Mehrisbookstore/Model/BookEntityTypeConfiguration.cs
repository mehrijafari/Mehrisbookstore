using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mehrisbookstore;

public class BookEntityTypeConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(e => e.Isbn13).HasName("PK__Books__3BF79E03863A4C47");

        builder.Property(e => e.Isbn13)
            .HasMaxLength(13)
            .HasColumnName("ISBN13");
        builder.Property(e => e.Language).HasMaxLength(50);
        builder.Property(e => e.OriginalTitleId).HasColumnName("OriginalTitleID");
        builder.Property(e => e.Price).HasColumnType("decimal(6, 2)");
        builder.Property(e => e.PublishDate).HasColumnName("Publish date");
        builder.Property(e => e.PublisherId).HasColumnName("Publisher ID");
        builder.Property(e => e.Title).HasMaxLength(100);

        builder.HasOne(d => d.OriginalTitle).WithMany(p => p.Books)
            .HasForeignKey(d => d.OriginalTitleId)
            .HasConstraintName("FK__Books__BookWorks__797309D9");

        builder.HasOne(d => d.Publisher).WithMany(p => p.Books)
            .HasForeignKey(d => d.PublisherId)
            .HasConstraintName("FK__Books__Publisher__5441852A");

        builder.HasMany(d => d.Genres).WithMany(p => p.Books)
            .UsingEntity<Dictionary<string, object>>(
                "BookGenre",
                r => r.HasOne<Genre>().WithMany()
                    .HasForeignKey("GenreId")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BookGenre__Genre__778AC167"),
                l => l.HasOne<Book>().WithMany()
                    .HasForeignKey("BookId")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BookGenre__BookI__76969D2E"),
                j =>
                {
                    j.HasKey("BookId", "GenreId").HasName("PK__BookGenr__CDD892727D175E17");
                    j.ToTable("BookGenres");
                    j.IndexerProperty<string>("BookId")
                        .HasMaxLength(13)
                        .HasColumnName("BookID");
                    j.IndexerProperty<int>("GenreId").HasColumnName("GenreID");
                });
    }
}
