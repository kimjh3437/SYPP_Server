using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEAM_Server.Model.DB.Components
{
    public class Contents_Text
    {
        public string textID { get; set; }
        public string noteContentsID { get; set; }
        public string belongingID { get; set; } // noteID, eventID, etc 
        public string Content { get; set; }
    }
}
