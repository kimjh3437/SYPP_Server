using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Model.DB.Applications;

namespace TEAM_Server.Model.RestRequest.Application
{
    public class Task_Request
    {
        public string uID { get; set; }
        public string applicationID { get; set; }
        public MidTask MidTask { get; set; }
    }
}
