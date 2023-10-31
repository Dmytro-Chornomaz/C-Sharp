using System.ComponentModel.DataAnnotations;
using Tech_Task_IP.Enums;

namespace Tech_Task_IP.Model
{
    public class Order
    {
        [Key]
        public int OrderNumber { get; set; }
        public string CustomerName { get; set; } = "No name";
        public string CustomerAddress { get; set; } = "Some address";
        public decimal TotalCost => Products.Sum(a => a.TotalCost);
        public Status Status { get; set; } = Status.New;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string Comment { get; set; } = "No comment";
        public bool IsEnded { get; set; } = false;
        public List<Product> Products { get; set; } = new();
    }
}
