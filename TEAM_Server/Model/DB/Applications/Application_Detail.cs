using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEAM_Server.Model.DB.Applications
{
    public class Application_Detail
    {
        public string applicationID { get; set; }
        public string uID { get; set; }
        public string PositionName { get; set; }
        public string CompanyName { get; set; }
        public string companyID { get; set; }
        public string positionID { get; set; }
        public List<MidTask> Status { get; set; } //this status represent applied and result status static 
        public Location.Location Location { get; set; }

    }
}
