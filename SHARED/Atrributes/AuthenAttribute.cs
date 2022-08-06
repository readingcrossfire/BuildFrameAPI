using Microsoft.AspNetCore.Mvc.Filters;

namespace SHARED.Atrributes
{
    public class AuthenAttribute : Attribute, IAsyncActionFilter
    {
        public string FunctionID { get; set; }
        public string FunctionName { get; set; }
        public AuthenAttribute()
        {

        }

        public AuthenAttribute(string functionID, string functionName)
        {
            this.FunctionID = functionID;
            this.FunctionName = functionName;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (string.IsNullOrEmpty(this.FunctionID) && string.IsNullOrEmpty(this.FunctionName))
            {
                
            }
            await next();
        }
    }
}