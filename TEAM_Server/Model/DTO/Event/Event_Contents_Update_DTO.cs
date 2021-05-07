using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEAM_Server.Model.DTO.Event
{
    public class Event_Contents_Update_DTO
    {
        public string companyID { get; set; }
        public string applicationID { get; set; }
        public string eventID { get; set; }
        public string noteContentsID { get; set; }
        public string Header { get; set; }
        public string textID { get; set; }
        public string Content { get; set; }
    }
}
