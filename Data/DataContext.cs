using Microsoft.EntityFrameworkCore;
using web.Data.EntityModels;

namespace web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<Rating> Ratings { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<WishList> WishLists { get; set; }
        public DbSet<WishListItem> WishListItems { get; set; }

        public DbSet<Address> Addresses { get; set; }
    }

}