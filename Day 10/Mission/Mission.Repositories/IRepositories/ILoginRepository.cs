using Mission.Entities.Entities;
using Mission.Entities.Models;
using Mission.Entities.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mission.Repositories.IRepositories
{
    public interface ILoginRepository
    {
        LoginUserResponseModel login(LoginUserRequestModel model);
        string Register(RegisterUserModel Model);
        UserDetails LoginUserDetailsById(int id);
        string UpdateUser(UserDetails model, string webRootPath);
        Task<(int StatusCode, object Body)> ChangePasswordAsync(ChangePasswordRequestModel model);

    }
}
