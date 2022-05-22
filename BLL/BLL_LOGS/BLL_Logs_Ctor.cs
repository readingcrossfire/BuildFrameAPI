
using DAL.DAL_Logs;
using Microsoft.Extensions.Caching.Distributed;

namespace BLL.BLL_Logs
{
    public partial class BLL_Logs
    {
        protected readonly IDistributedCache _cache;
        protected readonly DAL_Logs _DAL_Logs;

        public BLL_Logs(IDistributedCache cache, DAL_Logs DAL_Logs)
        {
            this._cache = cache;
            this._DAL_Logs = DAL_Logs;
        }
    }
}