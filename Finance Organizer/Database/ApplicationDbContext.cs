using Finance_Organizer.Business;
using Microsoft.EntityFrameworkCore;

namespace Finance_Organizer.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Person> Users => Set<Person>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<Categories> Categories => Set<Categories>();

        // The method that specifies primary tables keys when creating the database.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Person>().HasKey(p => p.Id);
            modelBuilder.Entity<Transaction>().HasKey(p => p.Id);
            modelBuilder.Entity<Categories>().HasKey(p => p.Id);
        }

        // The function that returns the specified user from the database.
        public Person? GetPersonByName(string name)
        {
            Person? person = Users
                .Include(x => x.Transactions).ThenInclude(y => y.Categories).FirstOrDefault(x => x.Name == name);
            return person;
        }
    }
}
