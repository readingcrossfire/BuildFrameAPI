using System.Text.Json;
using BLL.BLL_Logs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ML.APIResult;
using ML.Logs;
using ML.Paging;
using SHARED.Atrributes;

namespace API.Controllers.LogsController
{
    [Route("Logs")]
    public class LogsController : ControllerBase
    {
        private readonly ILogsService _logsService;

        public LogsController(ILogsService logsService)
        {
            this._logsService = logsService;
        }

        [Route("GetAll")]
        [HttpPost]
        [Authorize]
        [LogAttribute(WebAPIControllerName = "LogsController", WebAPIMethodName = "GetAll", WebAPIMethodDescription = "Lấy danh sách tất cả logs")]
        public async Task<IActionResult> GetAll()
        {
            var result = await this._logsService.LogsGetAll();
            return StatusCode(StatusCodes.Status200OK, JsonSerializer.Serialize<APIResult<List<LogsItem>>>(result));
        }

        [Route("Get")]
        [HttpPost]
        [Authorize]
        [LogAttribute(WebAPIControllerName = "LogsController", WebAPIMethodName = "Get", WebAPIMethodDescription = "Lấy danh sách logs")]
        public async Task<IActionResult> Get([FromBody] Paging paging)
        {
            var result = await this._logsService.LogsGet(paging);
            return StatusCode(StatusCodes.Status200OK, JsonSerializer.Serialize<APIResult<List<LogsItem>>>(result));
        }
    }
}