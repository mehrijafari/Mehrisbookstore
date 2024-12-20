using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace Mehrisbookstore;

public partial class MehrisbookstoreContext : DbContext
{
    public MehrisbookstoreContext()
    {
    }

    public MehrisbookstoreContext(DbContextOptions<MehrisbookstoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<AverageRatingPerBook> AverageRatingPerBooks { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<InventoryBalance> InventoryBalances { get; set; }

    public virtual DbSet<OriginalBook> OriginalBooks { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<TitlesPerAuthor> TitlesPerAuthors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Initial Catalog=Mehrisbookstore;Integrated Security=True;Trust Server Certificate=True;Server SPN=localhost");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new StoreEntityTypeConfiguration().Configure(modelBuilder.Entity<Store>());
        new ReviewEntityTypeConfiguration().Configure(modelBuilder.Entity<Review>());
        new PublisherEntityTypeConfiguration().Configure(modelBuilder.Entity<Publisher>());
        new OriginalBookEntityTypeConfiguration().Configure(modelBuilder.Entity<OriginalBook>());
        new InventoryBalanceEntityTypeConfiguration().Configure(modelBuilder.Entity<InventoryBalance>());
        new GenreEntityTypeConfiguration().Configure(modelBuilder.Entity<Genre>());
        new CustomerEntityTypeConfiguration().Configure(modelBuilder.Entity<Customer>());
        new BookEntityTypeConfiguration().Configure(modelBuilder.Entity<Book>());
        new TitlesPerAuthorEntityTypeConfiguration().Configure(modelBuilder.Entity<TitlesPerAuthor>());
        new AverageRatingPerBookEntityTypeConfiguration().Configure(modelBuilder.Entity<AverageRatingPerBook>());
        new AuthorEntityTypeConfiguration().Configure(modelBuilder.Entity<Author>());
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
