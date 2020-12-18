using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Model.DB.Users;
using TEAM_Server.Model.RestRequest.Auth;

namespace TEAM_Server.Services.Interface
{
    public interface IAuthService
    {
        User Register(User_Register register);

        User Authenticate(User_Authenticate authenticate);

        Boolean NameCheck(string username);

        List<User_Personal> GetAllUsers();

        bool UpdatePersonalInfo(User_Personal personal);
    }
}
