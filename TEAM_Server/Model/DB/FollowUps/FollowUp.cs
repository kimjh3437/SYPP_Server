using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Model.DB.Contacts;

namespace TEAM_Server.Model.DB.FollowUps
{
    public class FollowUp
    {
        public string followUpID { get; set; }
        public string cotactID { get; set; }
        public DateTime Time { get; set; }
        public Contact_Detail Personnel { get; set; }
        public List<string> Description { get; set; }
    }
}
