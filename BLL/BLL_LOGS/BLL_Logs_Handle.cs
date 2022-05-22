using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using ML;

namespace BLL.BLL_Logs
{
    public partial class BLL_Logs : ILogsService
    {
        public async Task<APIResult<List<Logs>>> LogsGetAll(bool useCache = false)
        {
            if (useCache)
            {
                CancellationTokenSource cancel = new CancellationTokenSource();
                string keyCache = "CACHE_LOGS_GETALL";
                byte[] cachedData = await this._cache.GetAsync(keyCache, cancel.Token);
                if (cachedData != null)
                {
                    var cachedDataString = Encoding.UTF8.GetString(cachedData);
                    List<Logs> lstLogsEntity = JsonSerializer.Deserialize<List<Logs>>(cachedDataString) ?? new();
                    return new APIResult<List<Logs>>
                    {
                        IsError = false,
                        Message = "Lấy thành công",
                        ResultObject = lstLogsEntity
                    };
                }
                else
                {
                    if (cancel.IsCancellationRequested)
                    {
                        return new APIResult<List<Logs>>
                        {
                            IsError = true,
                            Message = "Có lỗi xảy ra"
                        };
                    }
                    var result = await _DAL_Logs.LogsGetAll();
                    string cachedDataString = JsonSerializer.Serialize(result);
                    var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);
                    var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3));

                    // Add the data into the cache
                    await _cache.SetAsync(keyCache, dataToCache, options);
                    return new APIResult<List<Logs>>
                    {
                        IsError = false,
                        Message = "Lấy thành công",
                        ResultObject = result.ToList()
                    };
                }
            }
            else
            {
                string keyCache = "CACHE_LOGS_GETALL";
                var result = await _DAL_Logs.LogsGetAll(); ;
                string cachedDataString = JsonSerializer.Serialize(result);
                var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);
                var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                .SetSlidingExpiration(TimeSpan.FromMinutes(3));

                // Add the data into the cache
                await _cache.SetAsync(keyCache, dataToCache, options);
                return new APIResult<List<Logs>>
                {
                    IsError = false,
                    Message = "Lấy thành công",
                    ResultObject = result.ToList()
                };
            }
        }

        public async Task<APIResult<List<Logs>>> LogsGetAllDI(bool useCache = false)
        {
            if (useCache)
            {
                CancellationTokenSource cancel = new CancellationTokenSource();
                string keyCache = "CACHE_LOGS_GETALLDI";
                byte[] cachedData = await this._cache.GetAsync(keyCache, cancel.Token);
                if (cachedData != null)
                {
                    var cachedDataString = Encoding.UTF8.GetString(cachedData);
                    List<Logs> lstLogsEntity = JsonSerializer.Deserialize<List<Logs>>(cachedDataString) ?? new();
                    return new APIResult<List<Logs>>
                    {
                        IsError = false,
                        Message = "Lấy thành công",
                        ResultObject = lstLogsEntity
                    };
                }
                else
                {
                    if (cancel.IsCancellationRequested)
                    {
                        return new APIResult<List<Logs>>
                        {
                            IsError = true,
                            Message = "Có lỗi xảy ra"
                        };
                    }
                    var result = await _DAL_Logs.LogsGetAllDI();
                    string cachedDataString = JsonSerializer.Serialize(result);
                    var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);
                    var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3));

                    // Add the data into the cache
                    await _cache.SetAsync(keyCache, dataToCache, options);
                    return new APIResult<List<Logs>>
                    {
                        IsError = false,
                        Message = "Lấy thành công",
                        ResultObject = result.ToList()
                    };
                }
            }
            else
            {
                string keyCache = "CACHE_LOGS_GETALLDI";
                var result = await _DAL_Logs.LogsGetAllDI(); ;
                string cachedDataString = JsonSerializer.Serialize(result);
                var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);
                var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                .SetSlidingExpiration(TimeSpan.FromMinutes(3));

                // Add the data into the cache
                await _cache.SetAsync(keyCache, dataToCache, options);
                return new APIResult<List<Logs>>
                {
                    IsError = false,
                    Message = "Lấy thành công",
                    ResultObject = result.ToList()
                };
            }
        }
    }
}