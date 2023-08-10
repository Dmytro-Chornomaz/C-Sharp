using System.ComponentModel.DataAnnotations;

namespace Tech_Task_Infopulse.Business
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
