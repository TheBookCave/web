using Microsoft.EntityFrameworkCore;
using web.Data.EntityModels;

namespace web.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<Rating> Ratings { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(
""
                );
        }

    }

}