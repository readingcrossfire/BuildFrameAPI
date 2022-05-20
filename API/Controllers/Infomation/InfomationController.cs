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
        public APIResult_New GetInfomationList()
        {
            return _bll.GetListInfo();
        }
    }
}
