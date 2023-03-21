using System.Text.Json;
using BLL.BLL_MenuTypes;
using Microsoft.AspNetCore.Mvc;
using ML.APIResult;
using ML.MenuTypes;
using SHARED.Atrributes;

namespace API.Controllers.MenuTypesController
{
    [Route("MenuTypes")]
    public class MenuTypesController : ControllerBase
    {
        private readonly IMenuTypesService _menuTypesService;

        public MenuTypesController(IMenuTypesService menuTypeService)
        {
            this._menuTypesService = menuTypeService;
        }


        [Route("GetAll")]
        [HttpPost]
        [Authen]
        [LogAttribute(WebAPIControllerName = "MenuTypesController", WebAPIMethodName = "GetAll", WebAPIMethodDescription = "Lấy danh sách Menu type")]
        public async Task<IActionResult> GetAll([FromQuery] bool useCache = false)
        {
            var result = await this._menuTypesService.GetAll(useCache);
            return StatusCode(StatusCodes.Status200OK, JsonSerializer.Serialize<APIResult<List<MenuTypesItem>>>(result));
        }
    }
}