﻿using App.Models.Entities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Services
{
    public class NotificationHubBasic : Hub
    {
        public async Task SendMessage(NotifyMessage message)
        {
            int a = 2;
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task JoinGroup(UserConnection connection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, connection.User);
        }
    }
}