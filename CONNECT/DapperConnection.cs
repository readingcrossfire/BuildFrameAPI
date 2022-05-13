using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace CONNECTION
{
    public interface IData
    {
    }

    public class DapperConnection
    {
        private static IDbConnection _dbConnection;
        private static String _storeName;
        private static DynamicParameters _dynamicParameters;

        public static IDbConnection CreateConnection()
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();
            _dbConnection = new SqlConnection(configuration.GetConnectionString("DB_BuildFrameAPI"));
            return _dbConnection;
        }
        public static void CreateNewStoredProcedure(string nameStore)
        {
            _storeName = nameStore;
            _dynamicParameters = new DynamicParameters();
        }
        public static void AddParameter(string fiedl, object value)
        {
            _dynamicParameters.Add(fiedl, value);
        }

        public static void OpenConnect() => _dbConnection.Open();

        public static void CloseConnect() => _dbConnection.Close();
    }
}