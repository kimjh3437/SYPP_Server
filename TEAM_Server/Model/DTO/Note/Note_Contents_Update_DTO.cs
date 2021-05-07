using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Model.DB.Components;

namespace TEAM_Server.Model.DTO.Note
{
    public class Note_Contents_Update_DTO
    {
        public string companyID { get; set; }
        public string applicationID { get; set; }
        public string noteID { get; set; }
        public string noteContentsID { get; set; }
        public string Header { get; set; }
        public List<Contents_Text> Contents { get; set; }
    }
}
