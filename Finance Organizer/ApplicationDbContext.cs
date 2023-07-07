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
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<Categories> Categories => Set<Categories>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Person>().HasKey(p => p.Id);
            modelBuilder.Entity<Transaction>().HasKey(p => p.Id);
            modelBuilder.Entity<Categories>().HasKey(p => p.Id);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=DESKTOP-5T0EOPC\\SQLEXPRESS;Database=Finance_Organizer;Trusted_Connection=True;");
        //}
    }
}
