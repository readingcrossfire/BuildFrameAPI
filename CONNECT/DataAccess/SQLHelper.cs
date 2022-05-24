using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using Dapper.Contrib.Extensions;

namespace CONNECTION.DataAccess
{
    public class SQLHelper
    {
        private static string _strConnection = "";
        public static string ConnectionString()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();
            _strConnection = configuration.GetConnectionString("DB_BuildFrameAPI");
            return _strConnection;
        }
        public SQLHelper()
        {
            ConnectionString();
        }
       
        public  long Insert<T>(T entityToInsert) where T : class
        {

            using (var connection = new SqlConnection(_strConnection))
            {
                connection.Open();
                long re = 0;
                try
                {
                    re = connection.Insert(entityToInsert);
                }
                catch (Exception ex)
                {
                    re = 0;
                    
                }
                return re;
            }
        }

        public  bool Update<T>(T entityToUpdate) where T : class
        {
            using (var connection = new SqlConnection(_strConnection))
            {
                connection.Open();
                var result = false;
                try
                {
                    result = connection.Update(entityToUpdate);
                }
                catch (Exception ex)
                {
                    result = false;
                    //XtraMessageBox.Show(ex.Message);
                }
                return result;
            }
        }

        public  bool Delete<T>(T entityToDelete) where T : class
        {
            using (var connection = new SqlConnection(_strConnection))
            {
                connection.Open();
                bool success = false;
                try
                {
                    success = connection.Delete(entityToDelete);
                }
                catch (Exception ex)
                {
                    //XtraMessageBox.Show(ex.Message);
                }
                return success;
            }
        }
        public  DataTable ExecProcedureDataAsDataTable(string ProcedureName, object parametter = null)
        {
            using (var connection = new SqlConnection(_strConnection))
            {
                connection.Open();
                DataTable table = new DataTable();
                var reader = connection.ExecuteReader(ProcedureName, param: parametter, commandType: CommandType.StoredProcedure);
                table.Load(reader);
               
                return table;
            }
        }



        public  async Task<DataTable> ExecProcedureDataAsyncAsDataTable(string ProcedureName, object parametter = null)
        {
            return await WithConnection(async c =>
            {
                var reader = await c.ExecuteReaderAsync(ProcedureName, param: parametter, commandType: CommandType.StoredProcedure);
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            });

        }


        public  DataTable ExecQueryDataAsDataTable(string T_SQL, object parametter = null)
        {
            using (var connection = new SqlConnection(_strConnection))
            {
                connection.Open();
                var reader = connection.ExecuteReader(T_SQL, param: parametter, commandType: CommandType.Text);
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
        }

        public  async Task<DataTable> ExecQueryDataAsyncAsDataTable(string T_SQL, object parametter = null)
        {
            return await WithConnection(async c =>
            {
                var reader = await c.ExecuteReaderAsync(T_SQL, param: parametter, commandType: CommandType.Text);
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            });

        }

        public  IEnumerable<T> ExecProcedureData<T>(string ProcedureName, object parametter = null)
        {
            using (var connection = new SqlConnection(_strConnection))
            {
                connection.Open();
                return connection.Query<T>(ProcedureName, param: parametter, commandType: CommandType.StoredProcedure);
            }
        }

        public  T ExecProcedureDataFistOrDefault<T>(string ProcedureName, object parametter = null)
        {
            using (var connection = new SqlConnection(_strConnection))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<T>(ProcedureName, parametter, commandType: CommandType.StoredProcedure);
            }
        }

        public  async Task<IEnumerable<T>> ExecProcedureDataAsync<T>(string ProcedureName, object parametter = null)
        {

            return await WithConnection(async c =>
            {
                return await c.QueryAsync<T>(ProcedureName, parametter, commandType: CommandType.StoredProcedure);
            });


        }

        public  T ExecProcedureDataFirstOrDefaultAsync<T>(string ProcedureName, object parametter = null)
        {
            using (var connection = new SqlConnection(_strConnection))
            {
                connection.Open();
                return connection.QueryFirstOrDefaultAsync<T>(ProcedureName, parametter, commandType: CommandType.StoredProcedure).Result;
            }
        }

        public  int ExecProcedureNonData(string ProcedureName, object parametter = null)
        {
            using (var connection = new SqlConnection(_strConnection))
            {
                connection.Open();
                //return affectedRows 
                return connection.Execute(ProcedureName, parametter, commandType: CommandType.StoredProcedure);
            }
        }

        public  int ExecProcedureNonDataAsync(string ProcedureName, object parametter = null)
        {
            using (var connection = new SqlConnection(_strConnection))
            {
                connection.Open();
                //return affectedRows 
                return connection.ExecuteAsync(ProcedureName, parametter, commandType: CommandType.StoredProcedure).Result;
            }
        }

        public  IEnumerable<T> ExecQueryData<T>(string T_SQL, object parametter = null)
        {
            using (var connection = new SqlConnection(_strConnection))
            {
                connection.Open();
                return connection.Query<T>(T_SQL, parametter, commandType: CommandType.Text);
            }
        }

        public  T ExecQueryDataFistOrDefault<T>(string T_SQL, object parametter = null)
        {
            using (var connection = new SqlConnection(_strConnection))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<T>(T_SQL, parametter, commandType: CommandType.Text);
            }
        }

        public  async Task<IEnumerable<T>> ExecQueryDataAsync<T>(string T_SQL, object parametter = null)
        {

            return await WithConnection(async c =>
            {
                return await c.QueryAsync<T>(T_SQL, parametter, commandType: CommandType.Text);
            });


        }

        public  T ExecQueryDataFirstOrDefaultAsync<T>(string T_SQL, object parametter = null)
        {
            using (var connection = new SqlConnection(_strConnection))
            {
                connection.Open();
                return connection.QueryFirstOrDefaultAsync<T>(T_SQL, parametter, commandType: CommandType.Text).Result;
            }
        }

        public  int ExecQueryNonData(string T_SQL, object parametter = null)
        {
            using (var connection = new SqlConnection(_strConnection))
            {
                connection.Open();
                return connection.Execute(T_SQL, parametter, commandType: CommandType.Text);
            }
        }

        public  async Task<int> ExecQueryNonDataAsync(string T_SQL, object parametter = null)
        {

            return await WithConnection(async c =>
            {
                return await c.ExecuteAsync(T_SQL, parametter, commandType: CommandType.Text);
            });



        }

        public  async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {

            using (var connection = new SqlConnection(_strConnection))
            {
                await connection.OpenAsync(); // Asynchronously open a connection to the database
                return await getData(connection); // Asynchronously execute getData, which has been passed in as a Func<IDBConnection, Task<T>>
            }

        }

        public  object ExecProcedureSacalar(string ProcedureName, object parametter = null)
        {
            using (var connection = new SqlConnection(_strConnection))
            {
                connection.Open();
                return connection.ExecuteScalar<object>(ProcedureName, parametter, commandType: CommandType.StoredProcedure);
            }

        }

        public  object ExecProcedureSacalarAsync(string ProcedureName, object parametter = null)
        {
            using (var connection = new SqlConnection(_strConnection))
            {
                connection.Open();
                return connection.ExecuteScalarAsync<object>(ProcedureName, parametter, commandType: CommandType.StoredProcedure).Result;
            }

        }

        public  object ExecQuerySacalar(string T_SQL, object parametter = null)
        {
            using (var connection = new SqlConnection(_strConnection))
            {
                connection.Open();
                return connection.ExecuteScalar<object>(T_SQL, parametter, commandType: CommandType.Text);
            }

        }

        public  object ExecQuerySacalarAsync(string T_SQL, object parametter = null)
        {
            using (var connection = new SqlConnection(_strConnection))
            {
                connection.Open();
                return connection.ExecuteScalarAsync<object>(T_SQL, parametter, commandType: CommandType.Text).Result;
            }

        }

    
    }
}
