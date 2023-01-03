using ML.APIResult;

namespace FIREBASE.SendNotification.Interface
{
    public interface ISendNotification
    {
        public Task<APIResultBase> Send(string strDevicesId, string strTitle, string strContent);
    }
}