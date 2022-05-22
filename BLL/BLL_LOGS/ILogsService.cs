using ML;

namespace BLL.BLL_Logs
{
    public interface ILogsService
    {
        public Task<APIResult<List<Logs>>> LogsGetAll(bool useCache = false);

        public Task<APIResult<List<Logs>>> LogsGetAllDI(bool useCache = false);
    }
}