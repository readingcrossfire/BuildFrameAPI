using ML.APIResult;
using ML.FireBase;

namespace BLL.BLL_FireBase
{
    public interface IFireBaseService
    {
        public Task<APIResultBase> AddToken(FireBaseToken objFireBaseToken);
        public Task<APIResultBase> CheckToken(FireBaseToken objFireBaseToken);
        public Task<APIResultBase> SendNotification(FireBaseNotification objFireBaseNotification);
    }
}