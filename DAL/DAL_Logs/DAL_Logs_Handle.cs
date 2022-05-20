using CONNECTION;
using CONNECTION.Interface;
using ML;

namespace DAL.DAL_Logs
{
    public partial class DAL_Logs
    {
        public async Task<IEnumerable<Logs>> LogsGetAll()
        {
            IDapperConnection dapperConnection = new DapperConnection();
            dapperConnection.CreateConnection();
            dapperConnection.OpenConnect();
            dapperConnection.CreateNewStoredProcedure("LOGS_GET_ALL");
            var result = await dapperConnection.QueryAsync<Logs>();
            dapperConnection.CloseConnect();
            return result;
        }

        public async Task<IEnumerable<Logs>> LogsGetAllDI()
        {
            _dapperConnectionDI.CreateConnection();
            _dapperConnectionDI.OpenConnect();
            _dapperConnectionDI.CreateNewStoredProcedure("LOGS_GET_ALL");
            var result = await _dapperConnectionDI.QueryAsync<Logs>();
            _dapperConnectionDI.CloseConnect();
            return result;
        }
    }
}