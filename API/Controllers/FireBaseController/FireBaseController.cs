using System.Text.Json;
using BLL.BLL_FireBase;
using Microsoft.AspNetCore.Mvc;
using ML.APIResult;
using ML.FireBase;
using SHARED.Atrributes;

namespace API.Controllers.FireBaseController
{
    [Route("FireBase")]
    public class FireBaseController : ControllerBase
    {
        private readonly IFireBaseService _firebase;

        public FireBaseController(IFireBaseService firebase)
        {
            this._firebase = firebase;
        }

        [Route("AddToken")]
        [HttpPost]
        [Authen]
        [LogAttribute(WebAPIControllerName = "FireBaseController", WebAPIMethodName = "AddToken", WebAPIMethodDescription = "Lưu token firebase")]
        public async Task<IActionResult> AddToken([FromBody] FireBaseToken objFireBaseToken)
        {
            APIResultBase apiResult = await this._firebase.AddToken(objFireBaseToken);
            return StatusCode(StatusCodes.Status200OK, JsonSerializer.Serialize<APIResultBase>(apiResult));
        }

        [Route("CheckToken")]
        [HttpPost]
        [Authen]
        [LogAttribute(WebAPIControllerName = "FireBaseController", WebAPIMethodName = "CheckToken", WebAPIMethodDescription = "Lưu token firebase")]
        public async Task<IActionResult> CheckToken([FromBody] FireBaseToken objFireBaseToken)
        {
            APIResultBase apiResult = await this._firebase.CheckToken(objFireBaseToken);
            return StatusCode(StatusCodes.Status200OK, JsonSerializer.Serialize<APIResultBase>(apiResult));
        }

        [Route("TestSendNotification")]
        [HttpPost]
        [Authen]
        [LogAttribute(WebAPIControllerName = "FireBaseController", WebAPIMethodName = "TestSendNotification", WebAPIMethodDescription = "Test gửi thông báo")]
        public async Task<IActionResult> TestSendNotification([FromBody] FireBaseNotification objFireBaseNotification)
        {
            APIResultBase apiResult = await this._firebase.SendNotification(objFireBaseNotification);
            return StatusCode(StatusCodes.Status200OK, JsonSerializer.Serialize<APIResultBase>(apiResult));
        }
    }
}