using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEAM_Server.Model.DB.Templates
{
    public class Word_Template
    {
        public string wordID { get; set; }
        public string Text { get; set; }
        public bool IsCustom { get; set; }
        public bool IsTab { get; set; }
        public bool IsNewLine { get; set; }
    }
}
