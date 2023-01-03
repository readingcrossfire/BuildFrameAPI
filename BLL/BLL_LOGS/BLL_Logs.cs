using System.Data;
using System.Text;
using System.Text.Json;
using DAL.DAL_Logs;
using Microsoft.Extensions.Caching.Distributed;
using ML.APIResult;
using ML.Logs;

namespace BLL.BLL_Logs
{
    public class BLL_Logs : ILogsService
    {
        protected readonly IDistributedCache _cache;
        protected readonly DAL_Logs _dal_Logs;

        public BLL_Logs(IDistributedCache cache, DAL_Logs dal_Logs)
        {
            this._cache = cache;
            this._dal_Logs = dal_Logs
;
        }

        public async Task<APIResult<List<LogsItem>>> LogsGetAll(bool useCache = false)
        {
            if (useCache)
            {
                CancellationTokenSource cancel = new CancellationTokenSource();
                string keyCache = "CACHE_LOGS_GETALL";
                byte[] cachedData = await this._cache.GetAsync(keyCache, cancel.Token);
                if (cachedData != null)
                {
                    var cachedDataString = Encoding.UTF8.GetString(cachedData);
                    List<LogsItem> lstLogsEntity = JsonSerializer.Deserialize<List<LogsItem>>(cachedDataString) ?? new();
                    return new APIResult<List<LogsItem>>
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
                        return new APIResult<List<LogsItem>>
                        {
                            IsError = true,
                            Message = "Có lỗi xảy ra"
                        };
                    }
                    
                    List<LogsItem> lstLogs = await _dal_Logs.LogsGetAll() as List<LogsItem>;
                    string cachedDataString = JsonSerializer.Serialize(lstLogs);
                    var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);
                    var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3));

                    // Add the data into the cache
                    await _cache.SetAsync(keyCache, dataToCache, options);
                    return new APIResult<List<LogsItem>>
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
                List<LogsItem> lstLogs = await _dal_Logs.LogsGetAll() as List<LogsItem>;
                string cachedDataString = JsonSerializer.Serialize(lstLogs);
                var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);
                var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                .SetSlidingExpiration(TimeSpan.FromMinutes(3));

                // Add the data into the cache
                await _cache.SetAsync(keyCache, dataToCache, options);
                return new APIResult<List<LogsItem>>
                {
                    IsError = false,
                    Message = "Lấy thành công",
                    ResultObject = lstLogs
                };
            }
        }
    }
}