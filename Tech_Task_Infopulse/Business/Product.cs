using System.ComponentModel.DataAnnotations;

namespace Tech_Task_Infopulse.Business
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSize { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalCost => Quantity * Price;
    }
}
