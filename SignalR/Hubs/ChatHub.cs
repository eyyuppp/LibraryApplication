using Data.Entity;
using Microsoft.AspNetCore.SignalR;

namespace SignalR.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task SendSingletonMessage(string message ,string connectionId)
        {
            await Clients.Client(connectionId).SendAsync(message, connectionId);
        }
    }
}
