using BLL.BLL_Drawls;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.DrawlsController
{
    [ApiController]
    [Route("API/Drawls")]
    public partial class DrawlsController : ControllerBase
    {
        private readonly IDrawlsService _drawlsService;

        public DrawlsController(IDrawlsService drawlsService)
        {
            this._drawlsService = drawlsService;
        }
    }
}