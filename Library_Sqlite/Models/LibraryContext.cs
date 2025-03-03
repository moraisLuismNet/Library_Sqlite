using Microsoft.EntityFrameworkCore;

namespace Library.Models
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<PublishingHouse> PublishingHouses { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Action> Actions { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relationship between Book and Author
            modelBuilder.Entity<Book>()
                .HasOne(l => l.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(l => l.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);  // To avoid cascading deletion if you don't need it

            // Relationship between Book and PublishingHouse
            modelBuilder.Entity<Book>()
                .HasOne(l => l.PublishingHouse)
                .WithMany(e => e.Books)
                .HasForeignKey(l => l.PublishingHouseId)
                .OnDelete(DeleteBehavior.Restrict);  // To avoid cascading deletion if you don't need it
        }
    }
}
