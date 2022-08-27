
using DAL.DAL_Logs;
using Microsoft.Extensions.Caching.Distributed;

namespace BLL.BLL_Logs
{
    public partial class BLL_Logs
    {
        protected readonly IDistributedCache _cache;
        protected readonly DAL_Logs _dal_Logs;

        public BLL_Logs(IDistributedCache cache, DAL_Logs dal_Logs)
        {
            this._cache = cache;
            this._dal_Logs = dal_Logs
;
        }
    }
}