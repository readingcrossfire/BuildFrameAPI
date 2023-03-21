using System.Text.Json;
using BLL.BLL_Authen;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ML.APIResult;
using ML.Authen.SignIn;
using ML.Authen.SignUp;
using SHARED.Atrributes;
using SignInResult = ML.Authen.SignIn.SignInResult;

namespace API.Controllers.AuthenController
{
    [Route("Authen")]
    public partial class AuthenController : ControllerBase
    {
        private readonly IAuthenService _authenService;

        public AuthenController(IAuthenService authenService)
        {
            this._authenService = authenService;
        }

        [Route("SignUp")]
        [HttpPost]
        [Authorize]
        [LogAttribute(WebAPIControllerName = "AuthenController", WebAPIMethodName = "SignUp", WebAPIMethodDescription = "Tạo tài khoản")]
        public async Task<IActionResult> SignUp([FromBody] SignUp signUp)
        {
            APIResultBase signUpResult = await this._authenService.SignUp(signUp);
            return StatusCode(StatusCodes.Status200OK, JsonSerializer.Serialize<APIResultBase>(signUpResult));
        }


        [Route("SignIn")]
        [HttpPost]
        [LogAttribute(WebAPIControllerName = "AuthenController", WebAPIMethodName = "SignIn", WebAPIMethodDescription = "Đăng nhập")]
        public async Task<IActionResult> SignIn([FromBody] SignIn signIn)
        {
            APIResult<SignInResult> signInResult = await this._authenService.SignIn(signIn);
            return StatusCode(StatusCodes.Status200OK, JsonSerializer.Serialize<APIResult<SignInResult>>(signInResult));
        }
    }
}