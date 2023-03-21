using System.Text.Json;
using BLL.BLL_Drawls;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ML.APIResult;
using ML.Drawls;
using ML.Paging;
using SHARED.Atrributes;

namespace API.Controllers.DrawlsController
{
    [Route("Drawls")]
    public partial class DrawlsController : ControllerBase
    {
        private readonly IDrawlsService _drawlsService;

        public DrawlsController(IDrawlsService drawlsService)
        {
            this._drawlsService = drawlsService;
        }

        [Route("CrawlDataCodeMaze")]
        [HttpPost]
        [Authorize]
        [LogAttribute(WebAPIControllerName = "DrawlsController", WebAPIMethodName = "CrawlDataCodeMaze", WebAPIMethodDescription = "Lấy danh sách CODEMAZE")]
        public async Task<IActionResult> CrawlDataCodeMaze([FromBody] Paging paging, [FromQuery] bool useCache = false)
        {
            var result = await this._drawlsService.CrawlDataCodeMaze(paging, useCache);
            return StatusCode(StatusCodes.Status200OK, JsonSerializer.Serialize<APIResult<List<DrawlsItem>>>(result));
        }

        //[Route("EndjinCategory")]
        //[LogAttribute(WebAPIControllerName = "DrawlsController", WebAPIMethodName = "EndjinCategory", WebAPIMethodDescription = "Lấy danh sách danh mục Endjin")]
        //[HttpPost]
        //public async Task<IActionResult> EndjinCategory([FromQuery] bool useCache = false)
        //{
        //    var result = await this._drawlsService.CrawlDataCategoryEndjin(useCache);
        //    return StatusCode(StatusCodes.Status200OK, JsonSerializer.Serialize<APIResult<List<EndjinCategoryItem>>>(result));
        //}
    }
}