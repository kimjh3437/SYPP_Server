using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEAM_Server.Model.RestRequest.Company
{
    public class Company_Favorite_Change
    {
        public string uID { get; set; }
        public string companyID { get; set; }
        public bool Status { get; set; }
    }
}
