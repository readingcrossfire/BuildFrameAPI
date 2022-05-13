using System.Data;
using System.Data.SqlClient;
using CONNECTION;
using Dapper;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SHARED.Atrributes
{
    public class LogAttribute : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //var sql = SQLConnection.CreateData();
            //SQLConnection.Open();

            //var cmd = sql.CreateCommand();
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "ADD_LOG";

            //SqlParameter param1 = new SqlParameter("@ID", Guid.NewGuid().ToString().ToUpper());
            //SqlParameter param2 = new SqlParameter("@APINAME", context.Controller.ToString());
            //SqlParameter param3 = new SqlParameter("@METHODNAME", context.ActionDescriptor.DisplayName.ToString());
            //SqlParameter param4 = new SqlParameter("@IP", context.HttpContext.Connection.RemoteIpAddress.ToString() ?? "");

            //cmd.Parameters.Add(param1);
            //cmd.Parameters.Add(param2);
            //cmd.Parameters.Add(param3);
            //cmd.Parameters.Add(param4);
            //cmd.ExecuteNonQuery();

            //SQLConnection.Close();

            using (IDbConnection dbConnection = DapperConnection.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ID", Guid.NewGuid().ToString().ToUpper(), DbType.String, ParameterDirection.Input);
                parameters.Add("@APINAME", context.Controller.ToString(), DbType.String, ParameterDirection.Input);
                parameters.Add("@METHODNAME", context.ActionDescriptor.DisplayName.ToString(), DbType.String, ParameterDirection.Input);
                parameters.Add("@IP", context.HttpContext.Connection.RemoteIpAddress.ToString() ?? "", DbType.String, ParameterDirection.Input);

                var result = await dbConnection.ExecuteAsync("ADD_LOG", parameters, commandType: CommandType.StoredProcedure);

            }

                await next();
        }
    }
}