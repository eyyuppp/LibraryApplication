using Data.Entity;
using Microsoft.AspNetCore.SignalR;

namespace SignalR.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly List<User> users = new List<User>
        {
            new User {UserName = "eyyup",Password="123", Photo = "../images/eyyup.png"},
            new User {UserName = "kenan", Password="123",Photo = "../images/kenan.png"},
            new User {UserName = "aleyna",Password="123", Photo = "../images/aleyna.png"}
        };
        private static readonly List<User> activeUsers = new();
        /// <summary>
        /// tüm cilientlara mesaj gönderir
        /// </summary>
        /// <param name="senderUserName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendAllMessage(string senderUserName, string message)
        {
            await Clients.All.SendAsync("ReceiveAllMessage", senderUserName, message);
        }
        /// <summary>
        /// tek bir clienta mesaj gönderir
        /// </summary>
        /// <param name="senderUserName"></param>
        /// <param name="targetUserName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string senderUserName, string targetUserName, string message)
        {
            var targetUser = users.FirstOrDefault(x => x.UserName == targetUserName);
            await Clients.Client(targetUser.Id).SendAsync("ReceiveMessage", senderUserName, message);
        }
        /// <summary>
        /// login olan kullanıcıyı çağırır
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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
        }
        /// <summary>
        /// logout yapan kullanıcıyı çağırır
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task LogOut(string userName, string password)
        {
            if (users.Any(x => x.UserName == userName && x.Password == password))
            {
                await Clients.Caller.SendAsync("LogOutResult", true);
                var user = users.FirstOrDefault(x => x.UserName == userName);
                user.Id = Context.ConnectionId;
                activeUsers.Remove(user);
            }
        }
        /// <summary>
        /// tüm clientları getirir
        /// </summary>
        /// <returns></returns>
        public async Task GetAllActiveUsers()
        {
            await Clients.All.SendAsync("ActiveUsersResult", activeUsers);
        }

        //public async Task NotifyAllUsers()
        //{
        //    await Clients.Group().SendAsync("ActiveUsersResult", activeUsers);
        //}


        //Clients.Caller: Client bir istek yaptığı zaman hangi client istek yapmışsa sadece o clientın metodunu bilgilendirmek istediğimizde kullanırız.
        //Clients.All: Tüm clientlara bildiri göndermek istediğimizde kullanırız.
        //Clients.AllExcept: Belirlediğimiz clientlar hariç bildiri göndermek istediğimizde kullanırız.
        //Clients.Client: Belirlediğimiz clienta bildiri göndermek için kullanırız. (connectionId)
        //Clients.Group: Belirli bir gruba dahil olmuş clientlara bildiri göndermek için kullanırız. (Sadece LTunes takımına bildiri gönder gibi.)
    }
}
