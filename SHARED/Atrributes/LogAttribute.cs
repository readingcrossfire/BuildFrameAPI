using System.Data;
using CONNECTION;
using CONNECTION.DapperConnection;
using Dapper;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SHARED.Atrributes
{
    public class LogAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            IDapperConnection dapperConnection = new DapperConnection().CreateConnection();
            dapperConnection.OpenConnect();
            dapperConnection.CreateNewStoredProcedure("LOGS_ADD");
            dapperConnection.AddParameter("@ID", Guid.NewGuid().ToString().ToUpper());
            dapperConnection.AddParameter("@APINAME", context.Controller.ToString());
            dapperConnection.AddParameter("@METHODNAME", context.ActionDescriptor.DisplayName.ToString().ToUpper());
            dapperConnection.AddParameter("@IP", context.HttpContext.Connection.RemoteIpAddress.ToString() ?? "");
            int result = dapperConnection.ExecuteAsync().Result;
            await next();
        }
    }
}