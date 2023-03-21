using System.Data;
using CONNECTION.DapperConnection;
using ML.Logs;
using ML.Paging;

namespace DAL.DAL_Logs
{
    public class DAL_Logs
    {
        public async Task<IEnumerable<LogsItem>> LogsGetAll()
        {
            IDapperConnection dapperConnection = DapperConnection.CreateConnection();

            try
            {
                dapperConnection.OpenConnect();
                dapperConnection.CreateNewStoredProcedure("LOGS_GETALL");
                var result = await dapperConnection.QueryAsync<LogsItem>();
                return result;
            }
            finally
            {
                dapperConnection.CloseConnect();
            }
        }

        public async Task<IEnumerable<LogsItem>> LogsGet(Paging paging)
        {
            IDapperConnection dapperConnection = DapperConnection.CreateConnection();

            try
            {
                dapperConnection.OpenConnect();

                dapperConnection.CreateNewStoredProcedure("LOGS_GET");

                dapperConnection.AddParameter("@V_PAGEINDEX", paging.PageIndex);
                dapperConnection.AddParameter("@V_PAGESIZE", paging.PageSize);

                var result = await dapperConnection.QueryAsync<LogsItem>();
                return result;
            }
            finally
            {
                dapperConnection.CloseConnect();
            }
        }
    }
}