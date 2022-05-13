using System.Text.Json;
using BLL;
using Microsoft.AspNetCore.Mvc;
using ML.APIResult;
using ML.Entity;
using SHARED.Atrributes;

namespace API.Controllers
{
    public partial class DrawlsController
    {
        [Route("/CrawlDataCodeMaze")]
        [HttpPost]
        [TypeFilter(typeof(LogAttribute))]
        public async Task<IActionResult> CrawlDataCodeMaze([FromQuery] bool useCache = false)
        {
            var result = await this._drawlsService.CrawlDataCodeMaze(useCache);
            return StatusCode(StatusCodes.Status200OK, JsonSerializer.Serialize<APIListObjectResult<Drawls>>(result));
        }
    }
}