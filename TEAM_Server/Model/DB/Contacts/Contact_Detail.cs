using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEAM_Server.Model.DB.Contacts
{
    public class Contact_Detail
    {
        public string contactID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        //TODO Maybe add company coordinates?
    }
}
