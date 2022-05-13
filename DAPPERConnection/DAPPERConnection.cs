using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DAPPERConnection
{
    public class DAPPERConnection
    {
        private readonly IConfiguration _configuration;
        public DAPPERConnection(IConfiguration configuration)
        {
            this._configuration = configuration;

        }
        protected IDbConnection CreateConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
