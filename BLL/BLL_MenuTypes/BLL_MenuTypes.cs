using System.Text;
using System.Text.Json;
using DAL.DAL_MenuTypes;
using Microsoft.Extensions.Caching.Distributed;
using ML.APIResult;
using ML.MenuTypes;

namespace BLL.BLL_MenuTypes
{
    public partial class BLL_MenuTypes : IMenuTypesService
    {
        protected readonly IDistributedCache _cache;
        protected readonly DAL_MenuTypes _dal_MenuTypes;
        public BLL_MenuTypes(IDistributedCache cache, DAL_MenuTypes dal_MenuTypes)
        {
            this._cache = cache;
            this._dal_MenuTypes = dal_MenuTypes;
        }

        public async Task<APIResult<List<MenuTypesItem>>> GetAll(bool useCache = false)
        {
            try
            {
                if (useCache)
                {
                    CancellationTokenSource cancel = new CancellationTokenSource();
                    string keyCache = "CACHE_MENUTYPES_GETALL";
                    byte[] cachedData = await this._cache.GetAsync(keyCache, cancel.Token);
                    if (cachedData != null)
                    {
                        var cachedDataString = Encoding.UTF8.GetString(cachedData);
                        List<MenuTypesItem> lstMenuTypes = JsonSerializer.Deserialize<List<MenuTypesItem>>(cachedDataString) ?? new();
                        return new APIResult<List<MenuTypesItem>>
                        {
                            IsError = false,
                            Message = "Lấy thành công",
                            ResultObject = lstMenuTypes
                        };
                    }
                    else
                    {
                        if (cancel.IsCancellationRequested)
                        {
                            return new APIResult<List<MenuTypesItem>>
                            {
                                IsError = true,
                                Message = "Có lỗi xảy ra"
                            };
                        }
                        List<MenuTypesItem> lstMenuTypes = await this._dal_MenuTypes.MenuTypesGetAll() as List<MenuTypesItem>;
                        string cachedDataString = JsonSerializer.Serialize(lstMenuTypes);
                        var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);
                        var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(3));

                        // Add the data into the cache
                        await _cache.SetAsync(keyCache, dataToCache, options);
                        return new APIResult<List<MenuTypesItem>>
                        {
                            IsError = false,
                            Message = "Lấy thành công",
                            ResultObject = lstMenuTypes
                        };
                    }
                }
                else
                {
                    string keyCache = "CACHE_MENUTYPES_GETALL";
                    List<MenuTypesItem> lstMenuType = await this._dal_MenuTypes.MenuTypesGetAll() as List<MenuTypesItem>;
                    string cachedDataString = JsonSerializer.Serialize(lstMenuType);
                    var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);
                    var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3));

                    // Add the data into the cache
                    await _cache.SetAsync(keyCache, dataToCache, options);
                    return new APIResult<List<MenuTypesItem>>
                    {
                        IsError = false,
                        Message = "Lấy thành công",
                        ResultObject = lstMenuType
                    };
                }
            }
            catch (Exception objEx)
            {
                return await Task.FromResult(new APIResult<List<MenuTypesItem>>
                {
                    IsError = true,
                    Message = objEx.Message,
                    ResultObject = null
                });
            }
        }
    }
}