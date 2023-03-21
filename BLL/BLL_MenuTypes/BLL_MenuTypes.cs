using CACHE;
using DAL.DAL_MenuTypes;
using ML.APIResult;
using ML.MenuTypes;

namespace BLL.BLL_MenuTypes
{
    public class BLL_MenuTypes : IMenuTypesService
    {
        public async Task<APIResult<List<MenuTypesItem>>> GetAll(bool useCache = false)
        {
            try
            {
                DAL_MenuTypes objDAL_MenuTypes = new DAL_MenuTypes();

                if (useCache)
                {
                    MessageResult<List<MenuTypesItem>> lstDataCache = await Cache.Instance.Get<List<MenuTypesItem>>(KeysCache.CACHE_MENUTYPES_GETALL);
                    if (lstDataCache != null && !lstDataCache.IsError && lstDataCache.ResultObject != null && lstDataCache.ResultObject.Count > 0)
                    {
                        List<MenuTypesItem> lstMenuTypes = lstDataCache.ResultObject;
                        return new APIResult<List<MenuTypesItem>>
                        {
                            IsError = false,
                            Message = "Lấy thành công",
                            ResultObject = lstMenuTypes
                        };
                    }
                    else
                    {
                        List<MenuTypesItem> lstMenuTypes = await objDAL_MenuTypes.MenuTypesGetAll() as List<MenuTypesItem> ?? new List<MenuTypesItem>();
                        await Cache.Instance.Set(KeysCache.CACHE_MENUTYPES_GETALL, lstMenuTypes, DateTimeOffset.Now.AddHours(2));
                        return new APIResult<List<MenuTypesItem>>
                        {
                            IsError = false,
                            Message = "Lấy thành công",
                            ResultObject = lstMenuTypes
                        };
                    }
                }
                else
                {
                    List<MenuTypesItem> lstMenuType = await objDAL_MenuTypes.MenuTypesGetAll() as List<MenuTypesItem> ?? new List<MenuTypesItem>();
                    Cache.Instance.Set(KeysCache.CACHE_MENUTYPES_GETALL, lstMenuType, DateTimeOffset.Now.AddHours(2));
                    return new APIResult<List<MenuTypesItem>>
                    {
                        IsError = false,
                        Message = "Lấy thành công",
                        ResultObject = lstMenuType
                    };
                }
            }
            catch (Exception objEx)
            {
                return await Task.FromResult(new APIResult<List<MenuTypesItem>>
                {
                    IsError = true,
                    Message = objEx.Message,
                    ResultObject = null
                });
            }
        }
    }
}