using Microsoft.EntityFrameworkCore;

namespace MovieFinder.Models // Make sure this matches your project name
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; } // Movie model needs to exist
    }
}
