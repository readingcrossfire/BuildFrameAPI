using BLL.BLL_MenuTypes;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.MenuTypesController
{
    [ApiController]
    [Route("API/MenuTypes")]
    public partial class MenuTypesController : ControllerBase
    {
        private readonly IMenuTypesService _menuTypesService;

        public MenuTypesController(IMenuTypesService menuTypeService)
        {
            this._menuTypesService = menuTypeService;
        }
    }
}