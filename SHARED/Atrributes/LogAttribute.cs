using CONNECTION.DapperConnection;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SHARED.Atrributes
{
    public class LogAttribute : Attribute, IAsyncActionFilter
    {
        public string? WebAPIControllerName { get; set; }
        public string? WebAPIMethodName { get; set; }
        public string? WebAPIMethodDescription { get; set; }

        //public LogAttribute(string WebAPIControllerName, string WebAPIMethodName, string WebAPIMethodDescription)
        //{
        //    this.WebAPIControllerName = WebAPIControllerName;
        //    this.WebAPIMethodName = WebAPIMethodName;
        //    this.WebAPIMethodDescription = WebAPIMethodDescription;
        //}

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            IDapperConnection dapperConnection = DapperConnection.CreateConnection();
            dapperConnection.OpenConnect();
            dapperConnection.CreateNewStoredProcedure("LOGS_ADD");
            dapperConnection.AddParameter("@ID", Guid.NewGuid().ToString().ToUpper());
            dapperConnection.AddParameter("@APINAME", this.WebAPIControllerName ?? "");
            dapperConnection.AddParameter("@METHODNAME", this.WebAPIMethodName ?? "");
            dapperConnection.AddParameter("@DESCRIPTION", this.WebAPIMethodDescription ?? "");
            int result = dapperConnection.ExecuteAsync().Result;
            await next();
        }
    }
}