using BLL.BLL_Logs.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.LogsController
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class LogsController: ControllerBase
    {
        public readonly ILogsService _logsService;

        public LogsController(ILogsService logsService)
        {
            this._logsService = logsService;
        }
    }
}