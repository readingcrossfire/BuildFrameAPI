using BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ML;

namespace API.Controllers.Infomation
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfomationController : ControllerBase
    {
        private readonly IBLL_Infomation _bll;

        public InfomationController(IBLL_Infomation bll)
        {
            _bll = bll;
        }
        [HttpGet, Route("GetInfomationLists")]
        public APIResult<object> GetInfomationList()
        {
            return _bll.GetListInfo();
        }
        [HttpPost, Route("GetInfomationByID")]
        public APIResult<object> GetInfomationList([FromBody] int intID)
        {
            return _bll.GetListInfoByID(intID);
        }
    }
}
