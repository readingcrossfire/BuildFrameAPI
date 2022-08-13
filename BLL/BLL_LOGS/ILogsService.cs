using ML;
using ML.APIResult;

namespace BLL.BLL_Logs
{
    public interface ILogsService
    {
        public Task<APIResult<List<Logs>>> LogsGetAll(bool useCache = false);
    }
}