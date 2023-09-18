using Finance_Organizer.Model;
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
        public async Task<Person?> GetPersonByNameAsync(string name)
        {
            Person? person = await Users
                .Include(x => x.Transactions).ThenInclude(y => y.Categories).FirstOrDefaultAsync(x => x.Name == name);
            return person;
        }
    }
}
