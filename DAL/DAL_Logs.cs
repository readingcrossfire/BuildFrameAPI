using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CONNECTION;
using CONNECTION.Interface;
using ML;

namespace DAL
{
    public class DAL_Logs
    {
        public static async Task<IEnumerable<Logs>> LogsGetAll()
        {
            IDapperConnection dapperConnection = new DapperConnection();
            dapperConnection.CreateConnection();
            dapperConnection.OpenConnect();
            dapperConnection.CreateNewStoredProcedure("LOGS_GET_ALL");
            var result = await dapperConnection.QueryAsync<Logs>();
            dapperConnection.CloseConnect();
            return result;
        }
    }
}
