
using Microsoft.Extensions.Caching.Distributed;

namespace BLL.BLL_Drawls
{
    public partial class BLL_Drawls
    {
        protected readonly IDistributedCache _cache;

        public BLL_Drawls(IDistributedCache cache)
        {
            this._cache = cache;
        }
    }
}