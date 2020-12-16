using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Model.DB.Checklists;
using TEAM_Server.Model.DB.Contacts;
using TEAM_Server.Model.DB.Events;
using TEAM_Server.Model.DB.FollowUps;
using TEAM_Server.Model.DB.Notes;

namespace TEAM_Server.Model.DB.Companies
{
    public class Company
    {
        public string companyID { get; set; }
        public string uID { get; set; }
        public Company_Detail Detail { get; set; }
        public List<Event> Events { get; set; }
        public List<Note> Notes { get; set; }
        public List<Contact> Contacts { get; set; }
        public List<FollowUp> FollowUps { get; set; }
        public List<Checklist> Checklists { get; set; }
    }
}
