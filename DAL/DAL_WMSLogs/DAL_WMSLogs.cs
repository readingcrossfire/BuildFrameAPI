using CONNECTION.DapperConnection;
using ML.WMS;

namespace DAL.DAL_WMSLogs
{
    public class DAL_WMSLogs
    {
        public async Task<int> Add(WMSLog objWMSLog)
        {
            IDapperConnection dapperConnection = DapperConnection.CreateConnection();

            try
            {
                dapperConnection.OpenConnect();
                dapperConnection.CreateNewStoredProcedure("WMS_LOGS_ADD");
                dapperConnection.AddParameter("@Title", objWMSLog.Title);
                dapperConnection.AddParameter("@Content", objWMSLog.Content);
                dapperConnection.AddParameter("@Event", objWMSLog.Event);
                dapperConnection.AddParameter("@UserName", objWMSLog.UserName);
                dapperConnection.AddParameter("@StoreID", objWMSLog.StoreID);
                dapperConnection.AddParameter("@Module", objWMSLog.Module);
                return await dapperConnection.ExecuteAsync();
            }
            finally
            {
                dapperConnection.CloseConnect();
            }
        }
    }
}