using ML;
using ML.APIResult;

namespace BLL.BLL_Logs.Interface
{
    public interface ILogsService
    {
        public Task<APIListObjectResult<Logs>> LogsGetAll(bool useCache = false);
        public Task<APIListObjectResult<Logs>> LogsGetAllDI(bool useCache = false);
    }
}