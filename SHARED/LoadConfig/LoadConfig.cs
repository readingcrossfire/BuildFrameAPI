using Microsoft.Extensions.Configuration;

namespace SHARED.LoadConfig
{
    public class LoadConfig
    {
        private IConfigurationRoot _configBuilder;
        public static LoadConfig Instance { get => new LoadConfig(); }

        public LoadConfig()
        {
            _configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        }

        public LoadConfig(string strPathFile)
        {
            this._configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(strPathFile)
            .Build();
        }

        public T GetValue<T>(string strKey)
        {
            object objConverted = Convert.ChangeType(this._configBuilder[strKey].ToString(), typeof(T));
            return (T)objConverted;
        }
    }
}