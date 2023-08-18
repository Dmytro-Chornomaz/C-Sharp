using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tech_Task_Infopulse.Business;
using Tech_Task_Infopulse.Enums;

namespace Tech_Task_Infopulse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationContext _Context;
        private readonly ILogger<OrdersController> _Logger;

        public OrdersController(ApplicationContext context, ILogger<OrdersController> logger)
        {
            _Context = context;
            _Logger = logger;
        }

        [HttpPost("AddProductsToOrder")]
        public ActionResult<Order> AddProductsToOrder([FromQuery] int productId, string productName,
            string productCategory, string productSize, int quantity, decimal price)
        {
            Order order;

            if (_Context.Orders.Count() == 0)
            {
                order = new Order() { OrderNumber = 1 };
                _Context.Orders.Add(order);
                _Context.SaveChanges();
                _Logger.LogInformation("*** Created first order. ***");
            }
            else
            {
                if (_Context.ReturnLastOrder().IsEnded)
                {
                    order = new Order() { OrderNumber = _Context.ReturnLastOrder().OrderNumber + 1 };
                    _Context.Orders.Add(order);
                    _Context.SaveChanges();
                    _Logger.LogInformation($"*** Created new order {order.OrderNumber}. ***");
                }
                else
                {                    
                    order = _Context.ReturnLastOrder();
                    _Logger.LogInformation($"*** Returned open order {order.OrderNumber}. ***");
                }
            }

            if ((price - decimal.Round(price, 2)) != 0)
            {
                _Logger.LogWarning("*** Incorrect price was entered! ***");
                return BadRequest();
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
            _Context.Products.Add(product);
            _Context.Orders.Update(order);
            _Context.SaveChanges();
            _Logger.LogInformation($"*** The product {productId} was saved in the order {order.OrderNumber}. ***");
            return order;
        }

        [HttpPost("SaveOrder")]
        public ActionResult<Order> SaveOrder([FromQuery] string comment, Status status, Customers customer)
        {            
            if (_Context.ReturnLastOrder().IsEnded)
            {
                _Logger.LogWarning("*** Returned the closed order! ***");
                return BadRequest();
            }

            var order = _Context.ReturnLastOrder();

            order.Comment = comment;
            order.Status = status;
            order.CustomerName = customer.ToString();
            order.OrderDate = DateTime.Now;
            order.IsEnded = true;
            
            _Context.Orders.Update(order);
            _Context.SaveChanges();
            _Logger.LogInformation($"*** Order {order.OrderNumber} was closed and saved. ***");
            return order;

        }

        [HttpDelete("CancelOrder")]
        public ActionResult CancelOrder()
        {
            if (_Context.ReturnLastOrder().IsEnded)
            {
                _Logger.LogWarning("*** Returned the closed order! ***");
                return NoContent();
            }
            else
            {
                _Context.ReturnLastOrder().Products.Clear();
                _Context.Orders.Update(_Context.ReturnLastOrder());
                _Context.SaveChanges();
                _Logger.LogInformation("*** The order was canceled. ***");
                return NoContent();
            }

        }

        [HttpGet("GetOrder")]
        public ActionResult<Order> GetOrder([FromQuery] int orderNumber)
        {
            var order = _Context.Orders.Include(a => a.Products).FirstOrDefault(a => a.OrderNumber == orderNumber);
            if (order != null)
            {
                _Logger.LogInformation($"*** The order {orderNumber} was returned. ***");
                return order;
            }
            else
            {
                _Logger.LogWarning($"*** The order {orderNumber} does not exist! ***");
                return BadRequest();
            }
        }

        [HttpGet("GetAllOrders")]
        public ActionResult<List<Order>> GetOrders()
        {
            if (_Context.Orders.Count() != 0)
            {
                _Logger.LogInformation("*** Returned all the orders. ***");
                return _Context.Orders.Include(a => a.Products).ToList();
            }
            else
            {
                _Logger.LogInformation("*** There are no orders! ***");
                return NotFound();
            }
        }

        
    }
}
