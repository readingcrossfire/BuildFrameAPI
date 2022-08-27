using System.Text.Json;
using CONNECTION.DapperConnection;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace SHARED.Atrributes
{
    public class LogAttribute : Attribute, IAsyncActionFilter
    {

        private readonly IConfigurationRoot _configBuilder;

        public string? WebAPIControllerName { get; set; }
        public string? WebAPIMethodName { get; set; }
        public string? WebAPIMethodDescription { get; set; }

        //public LogAttribute(string WebAPIControllerName, string WebAPIMethodName, string WebAPIMethodDescription)
        //{
        //    this.WebAPIControllerName = WebAPIControllerName;
        //    this.WebAPIMethodName = WebAPIMethodName;
        //    this.WebAPIMethodDescription = WebAPIMethodDescription;
        //}

        public LogAttribute()
        {
            this._configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Dictionary<string, object> dicparams = context.ActionArguments.ToDictionary(x => x.Key, x => x.Value);
            string strJsonParams = JsonSerializer.Serialize(dicparams);
            string connectionStrings = this._configBuilder.GetConnectionString("DB_NEWSAPI");
            IDapperConnection dapperConnection = DapperConnection.CreateConnection(connectionStrings);
            dapperConnection.OpenConnect();
            dapperConnection.CreateNewStoredProcedure("LOGS_ADD");
            dapperConnection.AddParameter("@ID", Guid.NewGuid().ToString().ToUpper());
            dapperConnection.AddParameter("@APINAME", this.WebAPIControllerName ?? "");
            dapperConnection.AddParameter("@METHODNAME", this.WebAPIMethodName ?? "");
            dapperConnection.AddParameter("@METHODNAME", this.WebAPIMethodName ?? "");
            dapperConnection.AddParameter("@PARAMS", strJsonParams ?? "");
            dapperConnection.AddParameter("@DESCRIPTION", this.WebAPIMethodDescription ?? "");
           
            int result = dapperConnection.ExecuteAsync().Result;
            await next();
        }
    }
}