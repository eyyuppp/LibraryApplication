using Data.Entity;
using Microsoft.AspNetCore.SignalR;

namespace SignalR.Hubs
{
    public class ChatHub : Hub
    {
        public async Task ChatMessage(Message message)
        {
            await Clients.All.SendAsync("sendmessage", message);
        }
    }
}
