using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Utilities.Notification;

namespace TEAM_Server.Model.Notification
{
    public class NotificationSubscription
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string uID { get; set; }

        public DeviceInstallation Installation { get; set; }
    }
}
