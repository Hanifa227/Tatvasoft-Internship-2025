using Mission.Entities.Entities;
using Mission.Entities.Models;
using Mission.Repositories.IRepositories;
using Mission.Services.Helpers;
using Mission.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mission.Services.Services
{
    public class LoginService: ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        private readonly JwtService _jwtService;
        public LoginService(ILoginRepository loginRepository, JwtService jwtService)
        {
            _loginRepository = loginRepository;
            _jwtService = jwtService;
        }
        public ResponseResult login(LoginUserRequestModel model)
        {
            var userObj = this._loginRepository.login(model);
            ResponseResult result = new ResponseResult();
            if(userObj.Message == "Login Successfully" ) {
                result.Message = userObj.Message;
                result.Result = ResponseStatus.Success;
                result.Data = _jwtService.GetToken(userObj.FirstName,userObj.EmailAddress,userObj.UserType);
            }
            else
            {
                result.Message = userObj.Message;
                result.Result = ResponseStatus.Error;
            }
            return result;
        }
    }
}
