using Microsoft.EntityFrameworkCore;

namespace Finance_Organizer
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Person> Users => Set<Person>();

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=DESKTOP-5T0EOPC\\SQLEXPRESS;Database=Finance_Organizer;Trusted_Connection=True;");
        //}
    }
}
