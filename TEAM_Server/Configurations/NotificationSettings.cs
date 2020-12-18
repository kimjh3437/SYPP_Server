using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEAM_Server.Configurations
{
    public class NotificationSettings
    {
        public string ConnectionString { get; set; }
        public string HubName { get; set; }
    }
    public interface INotificaitonSettings
    {
        public string ConnectionString { get; set; }

        public string HubName { get; set; }
    }
}
