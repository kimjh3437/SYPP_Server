using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Configurations;

namespace TEAM_Server.Utilities.Notification
{
    public class Notifications
    {
        public static Notifications Instance;
        public NotificationHubClient Hub { get; set; }
        private Notifications(
            IOptions<NotificationSettings> settings)
        {
            Hub = NotificationHubClient.CreateClientFromConnectionString(settings.Value.ConnectionString,settings.Value.HubName);
            Instance = this;
        }
    }
}
