using BLL.BLL_LOGS.Interface;
using DAL;
using ML;

namespace BLL.BLL_LOGS
{
    public class BLL_Logs_Handle : ILogsService
    {
        public async Task<IEnumerable<Logs>> LogsGetAll()
        {
            return await DAL_Logs.LogsGetAll();
        }
    }
}