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
            dapperConnection.OpenConnect();
            dapperConnection.CreateNewStoredProcedure("LOGS_GET_ALL");
            var result = await dapperConnection.QueryAsync<Logs>();
            dapperConnection.CloseConnect();
            return result;
        }

        public async Task<DataTable> LogsGetAllToDataTable()
        {
            IDapperConnection dapperConnection = DapperConnection.CreateConnection();
            dapperConnection.OpenConnect();
            dapperConnection.CreateNewStoredProcedure("LOGS_GET_ALL");
            var result = await dapperConnection.QueryToDataTable();
            dapperConnection.CloseConnect();
            return result;
        }
    }
}