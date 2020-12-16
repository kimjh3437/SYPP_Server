using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Model.DB.Notes;

namespace TEAM_Server.Model.DB.Events
{
    public class Event
    {
        public string eventID { get; set; }
        public Event_Detail Detail { get; set; }
        public List<Contents_Sub> Contents { get; set; }
    }
}
