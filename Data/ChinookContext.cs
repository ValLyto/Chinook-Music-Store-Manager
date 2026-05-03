using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data
{
    // The main class that connects our code to the database file
    public class ChinookContext : DbContext 
    {
        // Define which tables we want to work with
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Track> Tracks { get; set; }

        // Setting up the connection to the SQLite file
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            // Tell the app where the 'chinook.db' file is located
            optionsBuilder.UseSqlite("Data Source=chinook.db");
        }
    }
}