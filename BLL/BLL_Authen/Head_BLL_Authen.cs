using Microsoft.Extensions.Configuration;

namespace BLL.BLL_Authen
{
    public partial class BLL_Authen
    {
        public readonly IConfiguration _configuration;
        public BLL_Authen(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

    }
}