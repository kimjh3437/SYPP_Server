using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEAM_Server.Model.DB.Notes
{
    public class Note
    {
        public string noteID { get; set; }
        //  public string applicationID { get; set; }
        public Note_Detail Detail { get; set; }
        public List<Contents_Sub> Contents { get; set; }
    }
}
