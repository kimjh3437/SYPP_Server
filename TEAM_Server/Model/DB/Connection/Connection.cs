using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Model.DB.Connection.Socket;

namespace TEAM_Server.Model.DB.Connection
{
    public class Connection
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string uID { get; set; }
        public SocketConnection SocketConnections { get; set; }
    }
}
