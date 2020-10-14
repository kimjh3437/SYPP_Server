using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEAM_Server.Model.DB.Notes
{
    public class Contents_Sub
    {
        public string noteContentsID { get; set; }
        public string Header { get; set; }
        public List<string> Contents_Text { get; set; }
    }
}
