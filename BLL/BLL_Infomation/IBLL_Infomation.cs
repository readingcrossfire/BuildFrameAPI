using ML;

namespace BLL
{
    public interface IBLL_Infomation
    {
        APIResult<object> GetListInfo();
        APIResult<object> GetListInfoByID(int intID);
    }
}