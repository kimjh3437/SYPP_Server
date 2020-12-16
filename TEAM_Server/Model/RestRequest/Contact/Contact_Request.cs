using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEAM_Server.Model.RestRequest.Contact
{
    public class Contact_Request
    {
        public string uID { get; set; }
        public string correspondenceID { get; set; }
        public TEAM_Server.Model.DB.Contacts.Contact Contact { get; set; }
    }
}
