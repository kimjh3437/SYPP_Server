using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Model.DB.Analytics.Applicaiton;

namespace TEAM_Server.Model.DB.Applications
{
    public class Application_Analytics
    {
        public string applicationID { get; set; }
        public List<Application_UpdateHistory> UpdateHistory { get; set; }
    }
}
