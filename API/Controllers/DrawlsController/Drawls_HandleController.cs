using System.Data;
using System.Text.Json;
using BLL;
using CONNECTION;
using Microsoft.AspNetCore.Mvc;
using ML;
using ML.Entity;
using SHARED.Atrributes;

namespace API.Controllers
{
    public partial class DrawlsController
    {
        [Route("/CrawlDataCodeMaze")]
        [HttpPost]
        [LogAttribute]
        public async Task<IActionResult> CrawlDataCodeMaze([FromQuery] bool useCache = false)
        {
            var result = await this._drawlsService.CrawlDataCodeMaze(useCache);
            return StatusCode(StatusCodes.Status200OK, JsonSerializer.Serialize<APIResult<List<Drawls>>>(result));
        }
    }
}