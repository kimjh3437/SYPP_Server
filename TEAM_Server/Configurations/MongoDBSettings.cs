using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEAM_Server.Configurations
{
    public class MongoDBSettings : IMongoDBSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
        public string Notifications { get; set; }
        public string Auth { get; set; }
        public string Users { get; set; }
        public string Applications { get; set; }
        public string Companies { get; set; }
        public string Templates { get; set; }
    }

    public interface IMongoDBSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
        public string Notifications { get; set; }
        public string Auth { get; set; }
        public string Users { get; set; }
        public string Applications { get; set; }
        public string Companies { get; set; }
        public string Templates { get; set; }

    }
}
