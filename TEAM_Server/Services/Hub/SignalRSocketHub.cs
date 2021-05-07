using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEAM_Server.Services.Interface;

namespace TEAM_Server.Services.Hub
{
    public class SignalRSocketHub : Microsoft.AspNetCore.SignalR.Hub
    {
        ISocketService _socket;
        public SignalRSocketHub(
            ISocketService socket)
        {
            _socket = socket;

        }
        public async Task TemplateMethod(string parameter)
        {
            try
            {
                List<Task> Tasks = new List<Task>();

                //DB Update 

                //Socket Call 

                await Task.WhenAll(Tasks);
            }
            catch (Exception ex)
            {
                Console.Write($"<MethodName> : {ex}");
            }
        }

        //___________________________________________________________________________________
        //
        // Initial Connection & Disconnections Related - Below 
        //___________________________________________________________________________________
        public async Task OnConnected(string uID, string connectionID, List<string> courseIDs)
        {
            List<Task> Tasks = new List<Task>();
            var tags = await _socket.UpdateSocketConnectionID(uID, connectionID);

            if (tags != null || tags.Count != 0)
            {
                Tasks.Add(_socket.UpdateSocketConnectionStatus(uID, true));

                List<Task> Task_Subscription = new List<Task>();
                foreach (var item in tags)
                {
                    //Re-register user for all tags subscribed
                    Task_Subscription.Add(Groups.AddToGroupAsync(connectionID, item.correspondenceID));
                }
                await Task.WhenAll(Task_Subscription);
            }
            else if (tags.Count == 0)
            {
                Tasks.Add(_socket.UpdateSocketConnectionStatus(uID, true));
            }

            await Task.WhenAll(Tasks);
        }
        public async Task OnDisconnected(string uID)
        {
            List<Task> Tasks = new List<Task>();
            var tags = await _socket.GetSocketTags(uID);
            //Socket UI Update 

            //Database Update
            Tasks.Add(_socket.UpdateSocketConnectionStatus(uID, false));

            await Task.WhenAll(Tasks);
        }
        public async Task UpdateConnectionID(string uID, string connectionID)
        {
            try
            {
                var tags = await _socket.UpdateSocketConnectionID(uID, connectionID);
                if (tags != null && tags.Count != 0)
                {
                    List<Task> Task_Subscription = new List<Task>();
                    foreach (var item in tags)
                    {
                        //Re-register user for all tags subscribed
                        Task_Subscription.Add(Groups.AddToGroupAsync(connectionID, item.correspondenceID));
                    }
                    Task_Subscription.Add(Groups.AddToGroupAsync(connectionID, uID));
                    await Task.WhenAll(Task_Subscription);
                }
            }
            catch (Exception ex)
            {
                Console.Write($"<UpdateConnectionID> : {ex}");
            }
        }
    }
}
