using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CONNECTION
{
    public class SQLConnection
    {
        private static IDbConnection _sqlConnection { get; set; } = new SqlConnection();
        private static string _connectionString = "";
        public static IDbConnection CreateData()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();
            _connectionString = configuration.GetConnectionString("DB_BuildFrameAPI");
            _sqlConnection.ConnectionString = _connectionString;
            return _sqlConnection;
        }

        public static void Open()
        {
            if (_sqlConnection.State == ConnectionState.Closed)
            {
                _sqlConnection.Open();
            }
        }

        public static void Close()
        {
            if (_sqlConnection.State == ConnectionState.Open)
            {
                _sqlConnection.Close();
            }
        }
    }
}