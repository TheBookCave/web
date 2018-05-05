using Microsoft.EntityFrameworkCore;
using web.Data.EntityModels;

namespace web.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(
                    ""
                );
        }

    }

}