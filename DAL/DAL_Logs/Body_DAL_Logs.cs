using System.Data;
using CONNECTION.DapperConnection;
using ML.Logs;

namespace DAL.DAL_Logs
{
    public partial class DAL_Logs
    {
        public async Task<IEnumerable<Logs>> LogsGetAll()
        {
            IDapperConnection dapperConnection = DapperConnection.CreateConnection();

            try
            {
                dapperConnection.OpenConnect();
                dapperConnection.CreateNewStoredProcedure("LOGS_GETALL");
                var result = await dapperConnection.QueryAsync<Logs>();
                return result;
            }
            finally
            {
                dapperConnection.CloseConnect();
            }
        }

        public async Task<DataTable> LogsGetAllToDataTable()
        {
            IDapperConnection dapperConnection = DapperConnection.CreateConnection();
            try
            {
                dapperConnection.OpenConnect();
                dapperConnection.CreateNewStoredProcedure("LOGS_GETALL");
                var result = await dapperConnection.QueryToDataTable();
                return result;
            }
            finally
            {
                dapperConnection.CloseConnect();
            }
        }
    }
}