using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEAM_Server.Model.RestRequest.Auth
{
    public class User_Authenticate
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
