using ML.APIResult;
using ML.WMS;

namespace BLL.BLL_WMS
{
    public interface IWMSLogsService
    {
        public Task<APIResultBase> GetAll(bool useCache = false);

        public Task<APIResultBase> SaveLogs(List<WMSLog> lstWMSLog);
    }
}