using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEAM_Server.Model.DB.Templates
{
    public class Template
    {
        public string templateID { get; set; }
        public string uID { get; set; }
        public string Title { get; set; }
        public List<Word_Template> Contents { get; set; }
    }
}
