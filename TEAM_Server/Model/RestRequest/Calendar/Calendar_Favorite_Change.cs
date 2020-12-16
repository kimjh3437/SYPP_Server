using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEAM_Server.Model.RestRequest.Calendar
{
    public class Calendar_Favorite_Change
    {
        public string uID { get; set; }
        public string applicationID { get; set; }
        public string midTaskID { get; set; }
        public bool Status { get; set; }
    }
}
