using System.Data;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using ML.APIResult;
using ML.Logs;

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
                    List<Logs> lstLogs = await _dal_Logs.LogsGetAll() as List<Logs>;
                    string cachedDataString = JsonSerializer.Serialize(lstLogs);
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
                        ResultObject = lstLogs.ToList()
                    };
                }
            }
            else
            {
                string keyCache = "CACHE_LOGS_GETALL";
                List<Logs> lstLogs = await _dal_Logs.LogsGetAll() as List<Logs>;
                string cachedDataString = JsonSerializer.Serialize(lstLogs);
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
                    ResultObject = lstLogs
                };
            }
        }
    }
}