using System.Text.Json;
using BLL.BLL_WMS;
using Microsoft.AspNetCore.Mvc;
using ML.APIResult;
using ML.WMS;
using SHARED.Atrributes;

namespace API.Controllers.WMSController
{
    [Route("/WMSLogs")]
    public class WMSLogsController : ControllerBase
    {
        private readonly IWMSLogsService _wmsLogs;

        public WMSLogsController(IWMSLogsService wmsLogs)
        {
            this._wmsLogs = wmsLogs;
        }

        [Route("SaveLog")]
        [LogAttribute(WebAPIControllerName = "WMSLogsController", WebAPIMethodName = "SaveLog", WebAPIMethodDescription = "Lưu log WMS")]
        [HttpPost]
        public async Task<IActionResult> SaveLog([FromBody] List<WMSLog> lstWMSLog)
        {
            APIResultBase apiResult = await this._wmsLogs.SaveLogs(lstWMSLog);
            return StatusCode(StatusCodes.Status200OK, JsonSerializer.Serialize<APIResultBase>(apiResult));
        }
    }
}