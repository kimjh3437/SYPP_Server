using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEAM_Server.Model.DB.Connection.Socket
{
    public class SocketConnection
    {
        public string uID { get; set; }
        public string connectionID { get; set; }
        public bool Connected { get; set; }
        public List<Socket_Tag> Tags { get; set; }
    }
}
