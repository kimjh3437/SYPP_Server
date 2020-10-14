﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Model.DB.Notes;

namespace TEAM_Server.Model.DB.Contacts
{
    public class Contact
    {
        public string contactID { get; set; }
        public Contact_Detail PersonalDetail { get; set; }
        public Contact_Email Email { get; set; }
        public Contact_Phone Phone { get; set; }
        public List<Contents_Sub> ConvoNote { get; set; }

    }
}