using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace HomeWork7
{
    public class PirateFilter : Attribute, IActionFilter
    {
        ILogger logger;
        public void OnActionExecuted(ActionExecutedContext context)
        {            
            logger.LogInformation($"v2 After {context.HttpContext.Request.Path}");
            //Console.WriteLine($"v2 After {context.HttpContext.Request.Path}");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation($"v2 Before {context.HttpContext.Request.Path}");
            //Console.WriteLine($"v2 Before {context.HttpContext.Request.Path}");
        }
    }
}
