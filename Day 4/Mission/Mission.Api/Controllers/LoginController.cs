using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mission.Entities.Models;
using Mission.Services.IServices;

namespace Mission.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }
        [HttpPost]
        [Route("Login")]
        public ResponseResult Login(LoginUserRequestModel model)
        {
            ResponseResult result = new ResponseResult();
            try
            {
                result = _loginService.login(model);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpGet]
        [Route("Registration")]
        [Authorize(Roles = "admin")]
        public string registration()
        {
            return "registartion api";
        }

        [HttpGet]
        [Route("getUser")]
        [Authorize(Roles = "user")]
        public string getUser()
        {
            return "getUser api";
        }
    }
}
