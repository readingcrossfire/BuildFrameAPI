using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ML.APIResult;
using ML.Logs;
using SHARED.Atrributes;

namespace API.Controllers.LogsController
{
    public partial class LogsController : ControllerBase
    {
        [Route("GetAll")]
        [HttpPost]
        [LogAttribute(WebAPIControllerName = "LogsController", WebAPIMethodName = "GetAll", WebAPIMethodDescription = "Lấy danh sách tất cả logs")]
        public async Task<IActionResult> GetAll([FromQuery] bool useCache = false)
        {
            var result = await this._logsService.LogsGetAll(useCache);
            return StatusCode(StatusCodes.Status200OK, JsonSerializer.Serialize<APIResult<List<Logs>>>(result));
        }
    }
}