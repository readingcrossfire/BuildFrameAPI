using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ML;
using SHARED.Atrributes;

namespace API.Controllers.LogsController
{
    public partial class LogsController
    {
        [Route("/GetAll")]
        [HttpPost]
        [LogAttribute]
        public async Task<IActionResult> GetAll([FromQuery] bool useCache = false)
        {
            var result = await this._logsService.LogsGetAll(useCache);
            return StatusCode(StatusCodes.Status200OK, JsonSerializer.Serialize<APIResult<List<Logs>>>(result));
        }
    }
}