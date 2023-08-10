using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tech_Task_Infopulse.Business;

namespace Tech_Task_Infopulse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationContext Context;

        public OrdersController(ApplicationContext context)
        {
            Context = context;
        }

        [HttpPost("AddProductsToOrder")]
        public ActionResult<Order> AddProductsToOrder([FromQuery] int productId, string productName,
            string productCategory, string productSize, int quantity, decimal price)
        {
            Order order;

            if (Context.Orders.Count() == 0)
            {
                order = new Order() { OrderNumber = 1 };
                Context.Orders.Add(order);
                Context.SaveChanges();
            }
            else
            {
                if (Context.ReturnLastOrder().IsEnded)
                {
                    order = new Order() { OrderNumber = Context.ReturnLastOrder().OrderNumber + 1 };
                    Context.Orders.Add(order);
                    Context.SaveChanges();
                }
                else
                {                    
                    order = Context.ReturnLastOrder();
                }
            }

            Product product = new Product()
            {
                ProductId = productId,
                OrderId = order.OrderNumber,
                ProductName = productName,
                ProductCategory = productCategory,
                ProductSize = productSize,
                Quantity = quantity,
                Price = price
            };
            Context.Products.Add(product);
            //order.QuantityTypesOfProducts += 1;
            Context.Orders.Update(order);
            Context.SaveChanges();
            return order;
        }

        [HttpPost("SaveOrder")]
        public ActionResult<Order> SaveOrder([FromQuery] string comment, Status status, Customers customer)
        {
            if (Context.Orders.Count() == 0)
            {
                return BadRequest();
            }
            if (Context.ReturnLastOrder().IsEnded)
            {
                return BadRequest();
            }

            var order = Context.ReturnLastOrder();

            order.Comment = comment;
            order.Status = status;
            order.CustomerName = customer.ToString();
            order.OrderDate = DateTime.Now;
            order.IsEnded = true;
            
            Context.Orders.Update(order);
            Context.SaveChanges();
            return order;

        }

        [HttpDelete("CancelOrder")]
        public ActionResult CancelOrder()
        {
            if (Context.Orders.Count() == 0)
            {
                return BadRequest();
            }

            if (Context.ReturnLastOrder().IsEnded)
            {
                return NoContent();
            }
            else
            {
                Context.ReturnLastOrder().Products.Clear();
                Context.Orders.Update(Context.ReturnLastOrder());
                Context.SaveChanges();
                return NoContent();
            }

        }

        [HttpGet("GetOrder")]
        public ActionResult<Order> GetOrder([FromQuery] int orderNumber)
        {
            var order = Context.Orders.Include(a => a.Products).FirstOrDefault(a => a.OrderNumber == orderNumber);
            if (order != null)
            {
                return order;
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("GetAllOrders")]
        public ActionResult<List<Order>> GetOrders()
        {
            if (Context.Orders.Count() != 0)
            {
                return Context.Orders.Include(a => a.Products).ToList();
            }
            else
            {
                return NotFound();
            }
        }

        
    }
}
