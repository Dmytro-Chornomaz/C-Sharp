using Microsoft.EntityFrameworkCore;
using Tech_Task_IP.Model;

namespace Tech_Task_IP
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
            optionsBuilder.UseSqlite("Data Source=Tech_Task_IP.db");
        }
        
        public async Task<Order?> ReturnLastOrderAsync()
        {
            int lastId = Orders.Max(a => a.OrderNumber);
            return await Orders.Include(a => a.Products).FirstOrDefaultAsync(a => a.OrderNumber == lastId);
        }
    }
}
