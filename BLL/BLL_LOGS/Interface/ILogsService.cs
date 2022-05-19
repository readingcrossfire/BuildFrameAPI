using ML;

namespace BLL.BLL_LOGS.Interface
{
    public interface ILogsService
    {
        public Task<IEnumerable<Logs>> LogsGetAll();
    }
}