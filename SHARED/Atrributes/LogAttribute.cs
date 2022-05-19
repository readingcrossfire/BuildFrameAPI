using System.Data;
using CONNECTION;
using CONNECTION.Interface;
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
            dapperConnection.CreateNewStoredProcedure("ADD_LOG");
            dapperConnection.AddParameter("@ID", Guid.NewGuid().ToString().ToUpper());
            dapperConnection.AddParameter("@APINAME", context.Controller.ToString());
            dapperConnection.AddParameter("@METHODNAME", context.ActionDescriptor.DisplayName.ToString().ToUpper());
            dapperConnection.AddParameter("@IP", context.HttpContext.Connection.RemoteIpAddress.ToString() ?? "");
            dapperConnection.AddParameter("@CREATEDDATE", DateTime.Now);
            dapperConnection.Execute();

            //var parameters = new DynamicParameters();
            //parameters.Add("@ID", Guid.NewGuid().ToString().ToUpper(), DbType.String, ParameterDirection.Input);
            //parameters.Add("@APINAME", context.Controller.ToString(), DbType.String, ParameterDirection.Input);
            //parameters.Add("@METHODNAME", context.ActionDescriptor.DisplayName.ToString(), DbType.String, ParameterDirection.Input);
            //parameters.Add("@IP", context.HttpContext.Connection.RemoteIpAddress.ToString() ?? "", DbType.String, ParameterDirection.Input);

            //var result = await dbConnection.ExecuteAsync("ADD_LOG", parameters, commandType: CommandType.StoredProcedure);
            await next();
        }
    }
}