using Microsoft.AspNetCore.SignalR;

namespace SignalR.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task SendSingletonMessage(string message, string UserID)
        {
            await Clients.User(UserID).SendAsync(message, UserID);
        }
    }
}
