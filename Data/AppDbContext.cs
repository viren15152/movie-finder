using Microsoft.EntityFrameworkCore;
using MovieFinder.Models; // ✅ Reference models

namespace MovieFinder.Data // ✅ Updated namespace
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
    }
}


