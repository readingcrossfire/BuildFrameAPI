
using System.Text.Json;
using DAL.DAL_FireBase;
using DAL.DAL_WMSLogs;
using FIREBASE.SendNotification.Interface;
using ML.APIResult;
using ML.FireBase;
using ML.WMS;
namespace BLL.BLL_WMS
{
    public class BLL_WMSLogs : IWMSLogsService
    {
        private readonly ISendNotification _sendNotification;

        public BLL_WMSLogs(ISendNotification sendNotification)
        {
            this._sendNotification = sendNotification;
        }

        public Task<APIResultBase> GetAll(bool useCache = false)
        {
            throw new NotImplementedException();
        }

        public async Task<APIResultBase> SaveLogs(List<WMSLog> lstWMSLog)
        {
            try
            {
                if (lstWMSLog != null && lstWMSLog.Count > 0)
                {
                    DAL_WMSLogs objDAL_WMSLogs = new DAL_WMSLogs();
                    DAL_FireBase objDAL_FireBase = new DAL_FireBase();

                    List<FireBaseToken> lstToken = await objDAL_FireBase.GetAll() as List<FireBaseToken>;

                    foreach (WMSLog item in lstWMSLog)
                    {
                        objDAL_WMSLogs.Add(item);
                        foreach (FireBaseToken firebaseTokenItem in lstToken)
                        {
                            this._sendNotification.Send(firebaseTokenItem.Token, item.Title, JsonSerializer.Serialize(item));
                        }
                    }

                    return new APIResultBase { IsError = false, Message = "Lưu cách thành công" };
                }

                return new APIResultBase { IsError = true, Message = "Có lỗi xảy ra" };
            }
            catch (Exception objEx)
            {
                return new APIResultBase
                {
                    IsError = true,
                    Message = objEx.ToString(),
                };
            }
        }
    }
}