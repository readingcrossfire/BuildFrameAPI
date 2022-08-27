using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using ML.APIResult;
using ML.MenuTypes;

namespace BLL.BLL_MenuTypes
{
    public partial class BLL_MenuTypes : IMenuTypesService
    {
        public async Task<APIResult<List<MenuTypes>>> GetAll(bool useCache = false)
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
                        List<MenuTypes> lstMenuTypes = JsonSerializer.Deserialize<List<MenuTypes>>(cachedDataString) ?? new();
                        return new APIResult<List<MenuTypes>>
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
                            return new APIResult<List<MenuTypes>>
                            {
                                IsError = true,
                                Message = "Có lỗi xảy ra"
                            };
                        }
                        List<MenuTypes> lstMenuTypes = await this._dal_MenuTypes.MenuTypesGetAll() as List<MenuTypes>;
                        string cachedDataString = JsonSerializer.Serialize(lstMenuTypes);
                        var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);
                        var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(3));

                        // Add the data into the cache
                        await _cache.SetAsync(keyCache, dataToCache, options);
                        return new APIResult<List<MenuTypes>>
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
                    List<MenuTypes> lstMenuType = await this._dal_MenuTypes.MenuTypesGetAll() as List<MenuTypes>;
                    string cachedDataString = JsonSerializer.Serialize(lstMenuType);
                    var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);
                    var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3));

                    // Add the data into the cache
                    await _cache.SetAsync(keyCache, dataToCache, options);
                    return new APIResult<List<MenuTypes>>
                    {
                        IsError = false,
                        Message = "Lấy thành công",
                        ResultObject = lstMenuType
                    };
                }
            }
            catch (Exception objEx)
            {
                return await Task.FromResult(new APIResult<List<MenuTypes>>
                {
                    IsError = true,
                    Message = objEx.Message,
                    ResultObject = null
                });
            }
        }
    }
}