using Data.Entity;
using Microsoft.AspNetCore.SignalR;

namespace SignalR.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly List<User> users = new List<User>
        {
            new User {UserName = "Eyyup",Password="123", Photo = "../images/eyyup.png"},
            new User {UserName = "Kenan", Password="123",Photo = "../images/kenan.png"},
            new User {UserName = "Aleyna",Password="123", Photo = "../images/aleyna.png"}
        };
        private static readonly List<User> activeUsers = new();

        public async Task SendAllMessage(string senderUserName, string message)
        {
            await Clients.All.SendAsync("ReceiveAllMessage", senderUserName, message);
        }

        public async Task SendMessage(string senderUserName, string targetUserName, string message)
        {
            var targetUser = users.FirstOrDefault(x => x.UserName == targetUserName);
            await Clients.Client(targetUser.Id).SendAsync("ReceiveMessage", senderUserName, message);
        }

        public async Task Login(string userName, string password)
        {
            if (users.Any(x => x.UserName == userName && x.Password == password))
            {
                await Clients.Caller.SendAsync("LoginResult", true);
                var user = users.FirstOrDefault(x => x.UserName == userName);
                user.Id = Context.ConnectionId;
                activeUsers.Add(user);
            }
            else
            {
                await Clients.Caller.SendAsync("LoginResult", false);
            }
            GetAllActiveUsers();
        }
        public async Task GetAllActiveUsers()
        {
            await Clients.Caller.SendAsync("ActiveUsersResult", activeUsers);
        }

    }
}
