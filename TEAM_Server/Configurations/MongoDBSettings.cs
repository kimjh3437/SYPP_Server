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
    }

    public interface IMongoDBSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }

    }
}
