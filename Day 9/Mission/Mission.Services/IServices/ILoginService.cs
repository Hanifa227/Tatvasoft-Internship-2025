using Mission.Entities.Entities;
using Mission.Entities.Models;
using Mission.Entities.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mission.Services.IServices
{
    public interface ILoginService
    {
        ResponseResult login(LoginUserRequestModel model);
        string Register(RegisterUserModel model);
        UserDetails loginUserDetailsById(int id);
        string UpdateUser(UserDetails model, string webhostpahth);
        Task<(int StatusCode, object Body)> ChangePasswordAsync(ChangePasswordRequestModel model);


    }
}
