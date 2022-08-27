using DAL.DAL_MenuTypes;
using Microsoft.Extensions.Caching.Distributed;

namespace BLL.BLL_MenuTypes
{
    public partial class BLL_MenuTypes
    {
        protected readonly IDistributedCache _cache;
        protected readonly DAL_MenuTypes _dal_MenuTypes;
        public BLL_MenuTypes(IDistributedCache cache, DAL_MenuTypes dal_MenuTypes)
        {
            this._cache = cache;
            this._dal_MenuTypes = dal_MenuTypes;
        }
    }
}