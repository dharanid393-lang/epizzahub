using Microsoft.AspNetCore.Mvc.Filters;

namespace ePizza.UI.Filters
{
    public class LoggingFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // You can add logging logic here
            base.OnActionExecuting(context);
        }
    }
}
