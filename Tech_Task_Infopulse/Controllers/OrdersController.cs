using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tech_Task_IP.Model;
using Tech_Task_IP.Enums;

namespace Tech_Task_IP.Controllers
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
        public async Task<ActionResult<Order>> AddProductsToOrderAsync([FromBody] int productId, string productName,
            string productCategory, string productSize, int quantity, decimal price)
        {            
            Order order;

            if (_Context.Orders.Count() == 0)
            {
                order = new Order() { OrderNumber = 1 };
                await _Context.Orders.AddAsync(order);
                await _Context.SaveChangesAsync();
                _Logger.LogInformation("*** Created first order. ***");
            }
            else
            {

                Order? lastOrder = await _Context.ReturnLastOrderAsync();

                if (lastOrder is null)
                {
                    _Logger.LogWarning("*** Error accessing the database. ***");
                    return StatusCode(500);
                }

                if (lastOrder.IsEnded)
                {
                    order = new Order() { OrderNumber = lastOrder.OrderNumber + 1 };
                    await _Context.Orders.AddAsync(order);
                    await _Context.SaveChangesAsync();
                    _Logger.LogInformation($"*** Created new order {order.OrderNumber}. ***");
                }
                else
                {                    
                    order = lastOrder;
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
            await _Context.Products.AddAsync(product);
            _Context.Orders.Update(order);
            await _Context.SaveChangesAsync();
            _Logger.LogInformation($"*** The product {productId} was saved in the order {order.OrderNumber}. ***");
            return order;
        }

        [HttpPost("SaveOrder")]
        public async Task<ActionResult<Order>> SaveOrderAsync([FromBody] string comment, Status status, Customers customer)
        {
            Order? lastOrder = await _Context.ReturnLastOrderAsync();

            if (lastOrder is null)
            {
                _Logger.LogWarning("*** Error accessing the database. ***");
                return StatusCode(500);
            }

            if (lastOrder.IsEnded)
            {
                _Logger.LogWarning("*** Returned the closed order! ***");
                return BadRequest();
            }

            lastOrder.Comment = comment;
            lastOrder.Status = status;
            lastOrder.CustomerName = customer.ToString();
            lastOrder.OrderDate = DateTime.Now;
            lastOrder.IsEnded = true;
            
            _Context.Orders.Update(lastOrder);
            await _Context.SaveChangesAsync();
            _Logger.LogInformation($"*** Order {lastOrder.OrderNumber} was closed and saved. ***");
            return lastOrder;

        }

        [HttpDelete("CancelOrder")]
        public async Task<ActionResult> CancelOrderAsync()
        {
            Order? lastOrder = await _Context.ReturnLastOrderAsync();

            if (lastOrder is null)
            {
                _Logger.LogWarning("*** Error accessing the database. ***");
                return StatusCode(500);
            }

            if (lastOrder.IsEnded)
            {
                _Logger.LogWarning("*** Returned the closed order! ***");
                return NoContent();
            }
            else
            {
                lastOrder.Products.Clear();
                _Context.Orders.Update(lastOrder);
                await _Context.SaveChangesAsync();
                _Logger.LogInformation("*** The order was canceled. ***");
                return NoContent();
            }

        }

        [HttpGet("GetOrder")]
        public async Task<ActionResult<Order>> GetOrderAsync([FromQuery] int orderNumber)
        {
            Order? lastOrder = await _Context.ReturnLastOrderAsync();

            if (lastOrder != null)
            {
                _Logger.LogInformation($"*** The order {orderNumber} was returned. ***");
                return lastOrder;
            }
            else
            {
                _Logger.LogWarning($"*** The order {orderNumber} does not exist! ***");
                return BadRequest();
            }
        }

        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<List<Order>>> GetOrdersAsync()
        {
            if (_Context.Orders.Count() != 0)
            {
                _Logger.LogInformation("*** Returned all the orders. ***");
                return await _Context.Orders.Include(a => a.Products).ToListAsync();
            }
            else
            {
                _Logger.LogInformation("*** There are no orders! ***");
                return NotFound();
            }
        }        
    }
}
