using Microsoft.EntityFrameworkCore;

namespace HomeWork7
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Controllers.PiratesController.Pirate> PiratesDB => Set<Controllers.PiratesController.Pirate>();
        public ApplicationContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Pirates.db");
        }
    }
}
