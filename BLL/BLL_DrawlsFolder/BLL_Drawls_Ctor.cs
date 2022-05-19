﻿using BLL.BLL_DrawlsFolder.Interface;
using Microsoft.Extensions.Caching.Distributed;

namespace BLL
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