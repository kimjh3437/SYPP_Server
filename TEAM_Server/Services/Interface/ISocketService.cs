using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Model.DB.Connection;
using TEAM_Server.Model.DB.Connection.Socket;

namespace TEAM_Server.Services.Interface
{
    public interface ISocketService
    {
        Task<bool> CreateInitialConnections(string uID);
        Task<bool> UpdateSocketConnectionStatus(string uID, bool connected);
        Task<string> UpdateSocketConnectionStatusByConnectionID(string connectionID, bool connected);
        Task<List<Socket_Tag>> UpdateSocketConnectionID(string uID, string connectionID);
        Task AddSocketTag(Socket_Tag model);
        Task<Connection> GetConnections(string uID);
        Task<List<Socket_Tag>> GetSocketTags(string uID);
    }
}
