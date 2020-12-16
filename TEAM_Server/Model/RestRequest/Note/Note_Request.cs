using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEAM_Server.Model.RestRequest.Note
{
    public class Note_Request
    {
        public string uID { get; set; }
        public string correspondenceID { get; set; }
        public TEAM_Server.Model.DB.Notes.Note Note { get; set; }
    }
}
