using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Model.DB.Applications;

namespace TEAM_Server.Model.RestRequest.Application
{
    public class Application_Favorite_Change
    {
        public string uID { get; set; }
        public string applicationID { get; set; }
        public bool Status { get; set; }
    }
}
