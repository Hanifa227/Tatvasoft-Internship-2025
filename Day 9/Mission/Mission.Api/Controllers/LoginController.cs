using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mission.Entities.Models;
using Mission.Entities.Models.Auth;
using Mission.Services.IServices;

namespace Mission.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IWebHostEnvironment _hostEnvironment;
        ResponseResult result = new ResponseResult();
        public LoginController(ILoginService loginService, IWebHostEnvironment hostEnvironment)
        {
            _loginService = loginService;
            _hostEnvironment = hostEnvironment;
        }
        [HttpPost]
        [Route("LoginUser")]
        public ResponseResult Login(LoginUserRequestModel model)
        {
            try
            {
                result.Data = _loginService.login(model);
                result.Result = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                result.Result = ResponseStatus.Error;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterUserModel model)
        {
            try
            {
                var res = _loginService.Register(model);
                return Ok(new ResponseResult() { Data = "User Added!", Result = ResponseStatus.Success, Message = "" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseResult() { Data = ex, Result = ResponseStatus.Success, Message = "Failed to add user" });
            }
        }
        [HttpGet]
        [Route("LoginUserDetailById/{id}")]
        public ResponseResult LoginUserDetailById(int id)
        {
            try
            {
                result.Data = _loginService.loginUserDetailsById(id);
                result.Result = ResponseStatus.Success;
                
            }
            catch (Exception ex)
            {
                result.Data = "null";
                result.Result = ResponseStatus.Error;
            }
            return result;
        }

        [HttpPost]
        [Route("UpdateUser")]
        public ResponseResult UpdateUser(UserDetails model)
        { 
            try
            {
                result.Data = _loginService.UpdateUser(model, _hostEnvironment.WebRootPath);
                result.Result = ResponseStatus.Success;

            }
            catch (Exception ex)
            {
                result.Data = "null";
                result.Result = ResponseStatus.Error;
            }
            return result;
        }

        //[HttpPost("ChangePassword")]
        //public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestModel model)
        //{
        //    if (model == null || model.UserId == 0)
        //        return BadRequest(new { result = 0, message = "Invalid request." });

        //    var response = await _loginService.ChangePasswordAsync(model);
        //    return StatusCode(response.StatusCode, response.Body);
        //}

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestModel model)
        {
            var (statusCode, body) = await _loginService.ChangePasswordAsync(model);
            return StatusCode(statusCode, body);
        }





    }
}
