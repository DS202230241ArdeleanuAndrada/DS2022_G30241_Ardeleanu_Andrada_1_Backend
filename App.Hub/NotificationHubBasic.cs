using App.Models.Entities;
using Microsoft.AspNetCore.SignalR;

namespace App.Hub
{
    public class NotificationHubBasic : DynamicHub
    {
        public async Task SendMessage(NotifyMessage message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task JoinGroup(UserConnection connection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, connection.User);
        }
    }
}
