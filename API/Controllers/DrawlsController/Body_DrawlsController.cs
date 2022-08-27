using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ML.APIResult;
using ML.Drawls;
using SHARED.Atrributes;

namespace API.Controllers.DrawlsController
{
    public partial class DrawlsController : ControllerBase
    {
        [Route("CrawlDataCodeMaze")]
        [LogAttribute(WebAPIControllerName = "DrawlsController", WebAPIMethodName = "CrawlDataCodeMaze", WebAPIMethodDescription = "Lấy danh sách CODEMAZE")]
        [HttpPost]
        public async Task<IActionResult> CrawlDataCodeMaze([FromQuery] bool useCache = false)
        {
            var result = await this._drawlsService.CrawlDataCodeMaze(useCache);
            return StatusCode(StatusCodes.Status200OK, JsonSerializer.Serialize<APIResult<List<Drawls>>>(result));
        }

        [Route("EndjinCategory")]
        [LogAttribute(WebAPIControllerName = "DrawlsController", WebAPIMethodName = "EndjinCategory", WebAPIMethodDescription = "Lấy danh sách danh mục Endjin")]
        [HttpPost]
        public async Task<IActionResult> EndjinCategory([FromQuery] bool useCache = false)
        {
            var result = await this._drawlsService.CrawlDataCategoryEndjin(useCache);
            return StatusCode(StatusCodes.Status200OK, JsonSerializer.Serialize<APIResult<List<EndjinCategory>>>(result));
        }
    }
}