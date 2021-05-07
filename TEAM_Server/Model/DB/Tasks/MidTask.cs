using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEAM_Server.Model.DB.Applications
{
    public class MidTask
    {
        public string midTaskID { get; set; }
        public DateTime Time { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public bool Status { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsVisible { get; set; }
        public string companyID { get; set; }
        public string applicationID { get; set; }
    }
}
