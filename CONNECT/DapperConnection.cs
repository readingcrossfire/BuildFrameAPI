using System.Data;
using System.Data.SqlClient;
using CONNECTION.Interface;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace CONNECTION
{
    public class DapperConnection : IDapperConnection
    {
        private IDbConnection _dbConnection;
        private String _storeName;
        private DynamicParameters _dynamicParameters;
        private IDbTransaction _dbTransaction;

        public IDapperConnection CreateConnection(string connectionString = "")
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();
            if (!string.IsNullOrEmpty(connectionString))
            {
                this._dbConnection = new SqlConnection(configuration.GetConnectionString(connectionString));
            }
            else
            {
                this._dbConnection = new SqlConnection(configuration.GetConnectionString("DB_BuildFrameAPI"));
            }
            return this;
        }

        public void CreateNewStoredProcedure(string nameStore)
        {
            this._storeName = nameStore;
            this._dynamicParameters = new DynamicParameters();
        }

        public void AddParameter(string field, object value)
        {
            _dynamicParameters.Add(field, value, DbType.String, ParameterDirection.Input);
        }

        public void OpenConnect() => _dbConnection.Open();

        public void CloseConnect() => _dbConnection.Close();

        public async Task<int> ExecuteAsync() => await _dbConnection.ExecuteAsync(this._storeName, this._dynamicParameters, commandType: CommandType.StoredProcedure);

        public async Task<IEnumerable<dynamic>> QueryAsync() => await _dbConnection.QueryAsync(_storeName, _dynamicParameters, commandType: CommandType.StoredProcedure);

        public async Task<IEnumerable<T>> QueryAsync<T>() => await _dbConnection.QueryAsync<T>(_storeName, _dynamicParameters, commandType: CommandType.StoredProcedure);
    }
}