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
        public FollowUp_Detail Detail { get; set; }
        public List<Contents_Sub> Description { get; set; }
    }
}
