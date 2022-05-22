using System.Text.Json;
using BLL;
using BLL.BLL_Drawls;
using Microsoft.AspNetCore.Mvc;
using ML.Entity;
using SHARED.Atrributes;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class DrawlsController : ControllerBase
    {
        private readonly IDrawlsService _drawlsService;
        public DrawlsController(IDrawlsService drawlsService)
        {
            this._drawlsService = drawlsService;
        }
    }
}