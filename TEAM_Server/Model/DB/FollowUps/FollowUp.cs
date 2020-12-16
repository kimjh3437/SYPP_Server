using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Model.DB.Contacts;
using TEAM_Server.Model.DB.Notes;

namespace TEAM_Server.Model.DB.FollowUps
{
    public class FollowUp
    {
        public string followUpID { get; set; }
        public string cotactID { get; set; }
        public DateTime Time { get; set; }
        public Contact_Detail Personnel { get; set; }
        public List<Contents_Sub> Description { get; set; }
    }
}
