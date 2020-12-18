using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEAM_Server.Model.DB.Users
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string uID { get; set; }
        public string authID { get; set; }
        public User_Personal Personal { get; set; }
        public List<string> ApplicationIDs { get; set; }
        public List<string> TemplateIDs { get; set; }
        public List<string> CompanyIDs { get; set; }
        public List<Category.Category> Preferences { get; set; }
        public string Token { get; set; }
    }
}
