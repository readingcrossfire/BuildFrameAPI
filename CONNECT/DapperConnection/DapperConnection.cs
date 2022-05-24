using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace CONNECTION.DapperConnection
{
    public class DapperConnection : IDapperConnection
    {
        private IDbConnection _dbConnection;
        private string _storeName;
        private DynamicParameters _dynamicParameters;
        private IDbTransaction _dbTransaction;

        public static IDapperConnection CreateConnection(string connectionString = "")
        {
            IDbConnection dbConnection;
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();
            if (!string.IsNullOrEmpty(connectionString))
            {
                dbConnection = new SqlConnection(configuration.GetConnectionString(connectionString));
            }
            else
            {
                dbConnection = new SqlConnection(configuration.GetConnectionString("DB_BuildFrameAPI"));
            }
            return new DapperConnection()
            {
                _dbConnection = dbConnection
            };
        }

        public void CreateNewStoredProcedure(string nameStore)
        {
            _storeName = nameStore;
            _dynamicParameters = new DynamicParameters();
        }

        public void AddParameter(string field, object value)
        {
            _dynamicParameters.Add(field, value, DbType.String, ParameterDirection.Input);
        }

        public void OpenConnect() => _dbConnection.Open();

        public void CloseConnect() => _dbConnection.Close();

        public async Task<int> ExecuteAsync() => await _dbConnection.ExecuteAsync(_storeName, _dynamicParameters, commandType: CommandType.StoredProcedure);

        public async Task<IEnumerable<dynamic>> QueryAsync() => await _dbConnection.QueryAsync(_storeName, _dynamicParameters, commandType: CommandType.StoredProcedure);

        public async Task<IEnumerable<T>> QueryAsync<T>() => await _dbConnection.QueryAsync<T>(_storeName, _dynamicParameters, commandType: CommandType.StoredProcedure);

        public async Task<DataTable> QueryToDataTable()
        {
            IDataReader objDataReader = await _dbConnection.ExecuteReaderAsync(_storeName, _dynamicParameters, commandType: CommandType.StoredProcedure);
            DataTable objTable = new DataTable();
            objTable.Load(objDataReader);
            return objTable;
        }

        public void BeginTransaction()
        {
            this._dbTransaction = this._dbConnection.BeginTransaction();
        }

        public void Commit()
        {
            this._dbTransaction.Commit();
        }

        public void RollBack()
        {
            this._dbTransaction.Rollback();
        }

        public void Dispose()
        {
            this._dbTransaction.Dispose();
        }
    }
}