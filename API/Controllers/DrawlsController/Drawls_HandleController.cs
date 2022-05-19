using System.Text.Json;
using BLL;
using CONNECTION;
using CONNECTION.Interface;
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
        [LogAttribute]
        public async Task<IActionResult> CrawlDataCodeMaze([FromQuery] bool useCache = false)
        {
            var result = await this._drawlsService.CrawlDataCodeMaze(useCache);
            return StatusCode(StatusCodes.Status200OK, JsonSerializer.Serialize<APIListObjectResult<Drawls>>(result));
        }

        //[Route("/LoadData")]
        //[HttpPost]
        //[LogAttribute]
        //public async Task<IActionResult> LoadData([FromQuery] bool useCache = false)
        //{
        //    //IDapperConnection dapperConnection = new DapperConnection();
        //    //dapperConnection.CreateConnection();
        //    //dapperConnection.OpenConnect();
        //    //dapperConnection.CreateNewStoredProcedure("")

        //}
    }
}