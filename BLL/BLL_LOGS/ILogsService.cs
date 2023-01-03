using ML.APIResult;
using ML.Logs;

namespace BLL.BLL_Logs
{
    public interface ILogsService
    {
        public Task<APIResult<List<LogsItem>>> LogsGetAll(bool useCache = false);
    }
}