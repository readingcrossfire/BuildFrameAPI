using System.Text;
using System.Text.Json;
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
                if (this._connectionMultiplexer == null || !this._connectionMultiplexer.IsConnected)
                {
                    await this.Init();
                }

                string strDataCache = JsonSerializer.Serialize(objValue);
                byte[] arrDataToCache = Encoding.UTF8.GetBytes(strDataCache);
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

        public async Task<MessageResult<T>> Get<T>(string strKey)
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
                    string strDataCache = Encoding.UTF8.GetString(valueResult);
                    T objDataCache = JsonSerializer.Deserialize<T>(strDataCache);
                    return new MessageResult<T>
                    {
                        IsError = false,
                        Message = "Lấy thành công",
                        ResultObject = objDataCache
                    };
                }
                else
                {
                    return new MessageResult<T>
                    {
                        IsError = false,
                        Message = "Không có dữ liệu",
                        ResultObject = default(T)
                    };
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