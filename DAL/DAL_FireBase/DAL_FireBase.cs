using CONNECTION.DapperConnection;
using ML.FireBase;

namespace DAL.DAL_FireBase
{
    public class DAL_FireBase
    {
        public async Task<IEnumerable<FireBaseToken>> GetAll()
        {
            IDapperConnection dapperConnection = DapperConnection.CreateConnection();

            try
            {
                dapperConnection.OpenConnect();
                dapperConnection.CreateNewStoredProcedure("FIREBASETOKEN_GETALL");

                return await dapperConnection.QueryAsync<FireBaseToken>();
            }
            finally
            {
                dapperConnection.CloseConnect();
            }
        }

        public async Task<int> Add(FireBaseToken objFireBaseToken)
        {
            IDapperConnection dapperConnection = DapperConnection.CreateConnection();

            try
            {
                dapperConnection.OpenConnect();
                dapperConnection.CreateNewStoredProcedure("FIREBASETOKEN_ADD");
                dapperConnection.AddParameter("@DeviceID", objFireBaseToken.DeviceID);
                dapperConnection.AddParameter("@Token", objFireBaseToken.Token);

                return await dapperConnection.ExecuteAsync();
            }
            finally
            {
                dapperConnection.CloseConnect();
            }
        }
    }
}