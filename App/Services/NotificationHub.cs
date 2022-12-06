using App.Models.Entities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Services
{
    public class NotificationHub : Hub
    {
        private readonly IDictionary<string, UserConnection> _connections;

        public NotificationHub(IDictionary<string, UserConnection> connections)
        {
            _connections = connections;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                _connections.Remove(Context.ConnectionId);
            }

            return base.OnDisconnectedAsync(exception);
        }


        public async Task JoinRoom(UserConnection userConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "devices");

            _connections[Context.ConnectionId] = userConnection;

            //await Clients.Group("devices").SendAsync("ReceiveMessage", $"{userConnection.Username} has joined");

        }


        public async Task SendMessage(string message)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                // await Clients.Group("devices").SendAsync("ReceiveMessage", userConnection.Username, message);
            }
        }

    }
}
