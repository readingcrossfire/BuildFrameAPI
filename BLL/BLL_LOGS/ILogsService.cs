using ML.APIResult;
using ML.Logs;
using ML.Paging;

namespace BLL.BLL_Logs
{
    public interface ILogsService
    {
        public Task<APIResult<List<LogsItem>>> LogsGetAll();

        public Task<APIResult<List<LogsItem>>> LogsGet(Paging paging);
    }
}