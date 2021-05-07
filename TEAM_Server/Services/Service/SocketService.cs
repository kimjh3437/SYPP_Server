using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Configurations;
using TEAM_Server.Model.DB.Connection;
using TEAM_Server.Model.DB.Connection.Socket;
using TEAM_Server.Services.Interface;

namespace TEAM_Server.Services.Service
{
    public class SocketService : ISocketService
    {
        IMongoCollection<Connection> _Connections;

        public SocketService(
            IOptions<MongoDBSettings> settings) 
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _Connections = database.GetCollection<Connection>(settings.Value.Connections);
        }


        //___________________________________________________________________________________
        //
        // Socket - Event Handlers - Below
        //___________________________________________________________________________________
        public async Task<bool> CreateInitialConnections(string uID)
        {
            try
            {
                var connections = new Connection
                {
                    uID = uID,
                    SocketConnections = new SocketConnection
                    {
                        uID = uID,
                        Connected = false,
                        connectionID = "",
                        Tags = new List<Socket_Tag>
                        {
                            new Socket_Tag
                            {
                                uID = uID,
                                correspondenceID = "uID",
                                Type = "uID"
                            }
                        }
                    }
                };
                await _Connections.InsertOneAsync(connections);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //___________________________________________________________________________________
        //
        // Socket - Add/Update Type Handlers - Below
        //___________________________________________________________________________________
        public async Task<bool> UpdateSocketConnectionStatus(string uID, bool connected)
        {
            try
            {
                var filter = Builders<Connection>.Filter.Eq(x => x.uID, uID);
                var update = Builders<Connection>.Update.Set(x => x.SocketConnections.Connected, connected);
                var output = await _Connections.UpdateOneAsync(filter, update);
                if (output.IsAcknowledged)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<string> UpdateSocketConnectionStatusByConnectionID(string connectionID, bool connected)
        {
            try
            {
                var filter = Builders<Connection>.Filter.Eq(x => x.SocketConnections.connectionID, connectionID);
                var update = Builders<Connection>.Update.Set(x => x.SocketConnections.Connected, connected);
                var output = await _Connections.UpdateOneAsync(filter, update);
                if (output.IsAcknowledged)
                {
                    var uID = _Connections.Find(x => x.SocketConnections.connectionID == connectionID).FirstOrDefault().uID;
                    return uID;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<Socket_Tag>> UpdateSocketConnectionID(string uID, string connectionID)
        {
            try
            {
                var filter = Builders<Connection>.Filter.Eq(x => x.uID, uID);
                var update = Builders<Connection>.Update.Set(x => x.SocketConnections.connectionID, connectionID);
                var output = await _Connections.UpdateOneAsync(filter, update);
                if (output.IsAcknowledged)
                {
                    var result = await _Connections.Find(x => x.uID == uID).FirstOrDefaultAsync();
                    if (result.SocketConnections.Tags != null)
                        return result.SocketConnections.Tags;
                    else
                        return null;
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task AddSocketTag(Socket_Tag model)
        {
            try
            {
                List<Task> Tasks = new List<Task>();

                var filter = Builders<Connection>.Filter.Eq(x => x.uID, model.uID);
                var update = Builders<Connection>.Update.AddToSet(x => x.SocketConnections.Tags, model);
                Tasks.Add(_Connections.UpdateOneAsync(filter, update));

                await Task.WhenAll(Tasks);
            }
            catch (Exception ex)
            {
                Console.Write($"<MethodName> : {ex}");
            }
        }

        //___________________________________________________________________________________
        //
        // Socket - Get Type Handlers - Below
        //___________________________________________________________________________________
        public async Task<Connection> GetConnections(string uID)
        {
            try
            {
                var output = await _Connections.Find(x => x.uID == uID).FirstOrDefaultAsync();
                return output;
            }
            catch (Exception ex)
            {
                Console.Write($"GetSocketTags : {ex}");
                return null;
            }
        }
        public async Task<List<Socket_Tag>> GetSocketTags(string uID)
        {
            try
            {
                var output = await _Connections.Find(x => x.uID == uID).FirstOrDefaultAsync();
                return output.SocketConnections.Tags;
            }
            catch (Exception ex)
            {
                Console.Write($"GetSocketTags : {ex}");
                return null;
            }
        }
    }
}
