using BLL.BLL_Logs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.LogsController
{
    [Route("API/Logs")]
    [ApiController]
    public partial class LogsController : ControllerBase
    {
        public readonly ILogsService _logsService;

        public LogsController(ILogsService logsService)
        {
            this._logsService = logsService;
        }
    }
}