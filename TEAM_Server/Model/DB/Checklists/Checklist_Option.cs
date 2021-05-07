using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEAM_Server.Model.DB.Checklists
{
    public class Checklist_Option
    {
        public string checkOptionID { get; set; }
        public string checklistID { get; set; }
        public string Content { get; set; }
        public bool IsChecked { get; set; }
    }
}
