using CONNECTION.DapperConnection;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SHARED.Atrributes
{
    public class LogAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            IDapperConnection dapperConnection = DapperConnection.CreateConnection();
            dapperConnection.OpenConnect();
            dapperConnection.CreateNewStoredProcedure("LOGS_ADD");
            dapperConnection.AddParameter("@ID", Guid.NewGuid().ToString().ToUpper());
            dapperConnection.AddParameter("@APINAME", context.Controller.ToString().Split(".").Last());
            dapperConnection.AddParameter("@METHODNAME", context.ActionDescriptor.DisplayName.ToString().Split(".").Last().ToUpper());
            dapperConnection.AddParameter("@IP", context.HttpContext.Connection.RemoteIpAddress.ToString() ?? "");
            int result = dapperConnection.ExecuteAsync().Result;
            await next();
        }
    }
}