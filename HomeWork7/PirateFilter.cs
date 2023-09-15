using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace HomeWork7
{
    public class PirateFilter : Attribute, IActionFilter
    {
        public ILogger<PirateFilter> Logger { get; }
        public PirateFilter(ILogger<PirateFilter> logger)
        {
            Logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {            
            Logger.LogInformation($"v2 After {context.HttpContext.Request.Path}");
            //Console.WriteLine($"v2 After {context.HttpContext.Request.Path}");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Logger.LogInformation($"v2 Before {context.HttpContext.Request.Path}");
            //Console.WriteLine($"v2 Before {context.HttpContext.Request.Path}");
        }

        //123
    }
}
