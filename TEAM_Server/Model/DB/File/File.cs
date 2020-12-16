using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEAM_Server.Model.DB.File
{
    public class File
    {
        public byte[] Contents { get; set; }
        public string Title { get; set; }
        public bool IsChecked { get; set; }
    }
}
