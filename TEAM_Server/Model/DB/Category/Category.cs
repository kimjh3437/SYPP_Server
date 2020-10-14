using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEAM_Server.Model.DB.Category
{
    public class Category
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public List<string> SuggestionsOrSeleceted { get; set; }
    }
}
