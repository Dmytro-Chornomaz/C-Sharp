using Microsoft.EntityFrameworkCore;
using Tech_Task_Infopulse.Business;

namespace Tech_Task_Infopulse
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Product> Products => Set<Product>();

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
        {            
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>().HasKey(p => p.OrderNumber);
            modelBuilder.Entity<Product>().HasKey(p => p.ProductId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Pirates.db");
        }
        
        public Order ReturnLastOrder()
        {
            int lastId = Orders.Max(a => a.OrderNumber);
            return Orders.Include(a => a.Products).FirstOrDefault(a => a.OrderNumber == lastId)!;
        }
    }
}
