using BLL.BLL_LOGS.Interface;
using Microsoft.Extensions.Caching.Distributed;

namespace BLL.BLL_LOGS
{
    public class BLL_Logs_Ctor
    {
        protected readonly IDistributedCache _cache;

        public BLL_Logs_Ctor(IDistributedCache cache)
        {
            this._cache = cache;
        }
    }
}