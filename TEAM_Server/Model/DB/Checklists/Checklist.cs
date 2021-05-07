using MongoDB.Libmongocrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEAM_Server.Model.DB.Checklists
{
    public class Checklist
    {
        public string checklistID { get; set; }
        public string companyID { get; set; }
        public string applicationID { get; set; }
        public string Type { get; set; }
        public bool Submission { get; set; }
        public List<Checklist_Option> Options { get; set; }
    }
}
