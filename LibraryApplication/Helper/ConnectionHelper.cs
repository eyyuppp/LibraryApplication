using Data.Entity;
using Microsoft.AspNetCore.SignalR.Client;

namespace LibraryApplication.Helper
{
    public static class ConnectionHelper
    {
        public static HubConnection Connection;
        public static void Connect()
        {
            Connection = new HubConnectionBuilder()
           .WithUrl("http://localhost:5217/chathub")
           .WithAutomaticReconnect()
           .Build();

            Connection.StartAsync();

        }

        private static User _loggedUser;
        public static User LoggedUser { get => _loggedUser; }

        public static void SetLoggedUser(User user)
        {
            _loggedUser = user;
        }
    }
}
