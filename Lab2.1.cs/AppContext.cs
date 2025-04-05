using ConsoleApp6.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp6
{
    class AppContext : DbContext
    {
        public DbSet<Course> Courses { get; set; } = null!;

        private readonly string _connectionString;
        public AppContext(string connectionString)
        {
            _connectionString = connectionString;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
        }
    }
}
