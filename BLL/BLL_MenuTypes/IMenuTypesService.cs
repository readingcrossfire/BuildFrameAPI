using ML.APIResult;
using ML.MenuTypes;

namespace BLL.BLL_MenuTypes
{
    public interface IMenuTypesService
    {
        public Task<APIResult<List<MenuTypes>>> GetAll(bool useCache = false);
    }
}