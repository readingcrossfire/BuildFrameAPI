using EF;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SHARED.Atrributes
{
    public class LogAttribute : IActionFilter
    {
        private readonly MyDbContext _dbContext;

        public LogAttribute(MyDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("OK"); 
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine(context.Controller.ToString());
            Console.WriteLine(context.ActionDescriptor.DisplayName);
        }
    }
}