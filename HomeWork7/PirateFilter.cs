using Microsoft.AspNetCore.Mvc.Filters;

namespace HomeWork7
{
    public class PirateFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"v2 After {context.HttpContext.Request.Path}");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"v2 Before {context.HttpContext.Request.Path}");
        }
    }
}
