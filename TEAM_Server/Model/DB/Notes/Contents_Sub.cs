using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Model.DB.Components;

namespace TEAM_Server.Model.DB.Notes
{
    public class Contents_Sub
    {
        public string belongingID { get; set; } //this is unique ID where it belongs to -> ex) noteID, contactID, etc 
        public string noteContentsID { get; set; }
        public string Header { get; set; }
        //public List<string> Contents_Text { get; set; }
        public List<Contents_Text> Contents { get; set; }
    }
}
