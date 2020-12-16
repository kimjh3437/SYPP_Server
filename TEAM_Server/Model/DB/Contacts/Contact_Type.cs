using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Model.DB.Notes;

namespace TEAM_Server.Model.DB.Contacts
{
    public class Contact_Type
    {
        public string contactID { get; set; }
        public string Type { get; set; } // Email, Phone, ConvoNotes 
        public List<Contents_Sub> Contents { get; set; }
    }
}
