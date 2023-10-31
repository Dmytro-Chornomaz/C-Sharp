using System.ComponentModel.DataAnnotations;

namespace Tech_Task_IP.Model
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductCategory { get; set; } = string.Empty;
        public string ProductSize { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalCost => Quantity * Price;
    }
}
