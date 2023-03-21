using Microsoft.Extensions.Configuration;

namespace SHARED.LoadConfig
{
    public class LoadConfig
    {
        public static LoadConfig Instance { get => new LoadConfig(); }

        public T GetValue<T>(string strKey, string strFileName = "appsettings.json")
        {
            IConfigurationRoot configBuilder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile(strFileName)
           .Build();

            object objConverted = Convert.ChangeType(configBuilder[strKey].ToString(), typeof(T));
            return (T)objConverted;
        }
    }
}