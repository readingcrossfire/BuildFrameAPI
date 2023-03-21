using CACHE;
using DAL.DAL_FireBase;
using FIREBASE.SendNotification.Interface;
using ML.APIResult;
using ML.FireBase;

namespace BLL.BLL_FireBase
{
    public class BLL_FireBase : IFireBaseService
    {
        private readonly ISendNotification _sendNotification;

        public BLL_FireBase(ISendNotification sendNotification)
        {
            this._sendNotification = sendNotification;
        }
        public async Task<APIResultBase> AddToken(FireBaseToken objFireBaseToken)
        {
            try
            {
                DAL_FireBase objDAL_FireBase = new DAL_FireBase();
                int saveResult = await objDAL_FireBase.Add(objFireBaseToken);
                if (saveResult > 0)
                {
                    return new APIResultBase
                    {
                        IsError = false,
                        Message = "Lưu token thành công"
                    };
                }

                return new APIResultBase
                {
                    IsError = true,
                    Message = "Có lỗi xảy ra"
                };
            }
            catch (Exception objEx)
            {
                return new APIResultBase
                {
                    IsError = true,
                    Message = objEx.ToString()
                };
            }
        }

        public async Task<APIResultBase> CheckToken(FireBaseToken objFireBaseToken)
        {
            try
            {
                MessageResult<List<FireBaseToken>> objDataCacheResult = await Cache.Instance.Get<List<FireBaseToken>>(KeysCache.CACHE_FIREBASE_TOKENS);

                Func<FireBaseToken, List<FireBaseToken>, Task<APIResultBase>> deleFuncHandleValidation = async (objNeed, lstSrc) =>
                {
                    bool isExistOrChanged = lstSrc.Any(item => item.DeviceID.Trim() == objNeed.DeviceID.Trim() && !item.Token.Trim().Equals(objNeed.Token));
                    if (isExistOrChanged)
                    {
                        return await this.AddToken(objFireBaseToken);
                    }
                    else
                    {
                        return new APIResultBase { IsError = false, Message = "Token đã tồn tại, không cần thêm" };
                    }
                };

                if (objDataCacheResult.ResultObject != null && !objDataCacheResult.IsError && objDataCacheResult.ResultObject.Count > 0)
                {
                    List<FireBaseToken> lstDataParse = objDataCacheResult.ResultObject;
                    return await deleFuncHandleValidation(objFireBaseToken, lstDataParse);
                }
                else
                {
                    DAL_FireBase objDAL_FireBase = new DAL_FireBase();
                    List<FireBaseToken> lstTokens = await objDAL_FireBase.GetAll() as List<FireBaseToken>;
                    Cache.Instance.Set(KeysCache.CACHE_FIREBASE_TOKENS, lstTokens, DateTimeOffset.Now.AddDays(1));
                    return await deleFuncHandleValidation(objFireBaseToken, lstTokens);
                }
            }
            catch (Exception objEx)
            {
                return new APIResultBase
                {
                    IsError = true,
                    Message = objEx.ToString()
                };
            }
        }

        public async Task<APIResultBase> SendNotification(FireBaseNotification objFireBaseNotification)
        {
            try
            {
                DAL_FireBase objDAL_FireBase = new DAL_FireBase();
                List<FireBaseToken> lstTokens = await objDAL_FireBase.GetAll() as List<FireBaseToken>;
                foreach (FireBaseToken objFireBaseTokenItem in lstTokens)
                {
                    this._sendNotification.Send(objFireBaseTokenItem.Token, objFireBaseNotification.Title, objFireBaseNotification.Content);
                }
                return new APIResultBase { IsError = false, Message = "Gửi thông báo thành công" };
            }
            catch (Exception objEx)
            {
                return new APIResultBase { IsError = true, Message = objEx.ToString() };
            }
        }
    }
}