using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace CONNECTION.Hieu_Dapper.DataAccess
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }
        public async Task<IEnumerable<T>> LoadDataAsync<T, U>(
            string storeProcedure,
            U parameters,
            string connectionID = "Default")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionID));
            return await connection.QueryAsync<T>(storeProcedure, parameters, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<T> LoadData<T, U>(
            string storeProcedure,
            U parameters,
            string connectionID = "Default")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionID));
            return connection.Query<T>(storeProcedure, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task SaveDataAsync<T>(string storeProcedure, T parameters, string connectionID = "Default")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionID));
            await connection.ExecuteAsync(storeProcedure, parameters, commandType: CommandType.StoredProcedure);
        }
        public int SaveData<T>(string storeProcedure, T parameters, string connectionID = "Default")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionID));
            return connection.Execute(storeProcedure, parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
