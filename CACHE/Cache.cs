using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using ML.APIResult;
using SHARED.LoadConfig;
using StackExchange.Redis;

namespace CACHE
{
    public class Cache
    {
        private int _databaseNumber = Convert.ToInt32(LoadConfig.Instance.GetValue<string>("Redis:RedisDB"));
        private ConnectionMultiplexer? _connectionMultiplexer = null;
        private IDatabase? _dataBase = null;

        public static Cache Instance { get => new Cache(); }

        private async Task Init()
        {
            string strHost = LoadConfig.Instance.GetValue<string>("Redis:Host");
            int intPort = LoadConfig.Instance.GetValue<int>("Redis:Port");

            ConfigurationOptions configOptions = new ConfigurationOptions();
            configOptions.EndPoints.Add(strHost, intPort);
            configOptions.AllowAdmin = true;
            configOptions.Password = LoadConfig.Instance.GetValue<string>("Redis:Password");

            this._connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(configOptions);
            this._dataBase = this._connectionMultiplexer.GetDatabase(_databaseNumber);
        }

        public async Task<MessageResultBase> Set(string strKey, object objValue, DateTimeOffset dtoExpired)
        {
            try
            {
                if (this._connectionMultiplexer == null ||  !this._connectionMultiplexer.IsConnected)
                {
                    await this.Init();
                }

                string strCachedDataString = JsonSerializer.Serialize(objValue);
                byte[] arrDataToCache = Encoding.UTF8.GetBytes(strCachedDataString);
                // Add the data into the cache
                bool setResult = await this._dataBase.StringSetAsync(strKey, arrDataToCache, dtoExpired.TimeOfDay);
                if (setResult)
                {
                    return new MessageResult<object>
                    {
                        IsError = false,
                        Message = "Thêm cache thành công"
                    };
                }
                return new MessageResult<object>
                {
                    IsError = true,
                    Message = "Thêm cache thất bại"
                };
            }
            catch (Exception objEx)
            {
                return new MessageResult<object>
                {
                    IsError = true,
                    Message = $"Thêm cache thất bại -> Exception: {objEx.Message}"
                };
            }
        }

        public async Task<MessageResultBase> Get<T>(string strKey)
        {
            try
            {
                if (this._connectionMultiplexer == null || !this._connectionMultiplexer.IsConnected) 
                {
                    await this.Init();
                }

                RedisValue valueResult = await this._dataBase.StringGetAsync(strKey);
                if (!valueResult.IsNullOrEmpty)
                {
                    object resultParse = Convert.ChangeType(valueResult, typeof(T));
                    return await Task.FromResult(new MessageResult<T>
                    {
                        IsError = false,
                        Message = "Lấy thành công",
                        ResultObject = (T)resultParse
                    });
                }
                else
                {
                    return await Task.FromResult(new MessageResult<T>
                    {
                        IsError = true,
                        Message = "Có lỗi xảy ra, lấy dữ liệu cache thất bại",
                        ResultObject = default(T)
                    });
                }
            }
            catch (Exception objEx)
            {
                return await Task.FromResult(new MessageResult<T>
                {
                    IsError = true,
                    Message = $"Lấy cache thất bại -> Exception: {objEx.Message}",
                    ResultObject = default(T)
                });
            }
        }

        
    }
}