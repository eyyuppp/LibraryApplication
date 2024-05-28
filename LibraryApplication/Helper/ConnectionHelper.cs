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
            //connection.Reconnecting += (sender) =>
            //{
            //    chat.Dispatcher.Invoke(() =>
            //    {
            //        var newMessage = "Yeniden Bağlanıyor";
            //        chat.mesage_list.Items.Add(newMessage);
            //    });
            //    return Task.CompletedTask;
            //};

            //connection.Reconnected += (sender) =>
            //{
            //    chat.Dispatcher.Invoke(() =>
            //    {
            //        var newMessage = "Yeniden bağlandı";
            //       chat.mesage_list.Items.Clear();
            //        chat.mesage_list.Items.Add(newMessage);
            //    });
            //    return Task.CompletedTask;
            //};

            //connection.Closed += (sender) =>
            //{
            //    chat.Dispatcher.Invoke(() =>
            //    {
            //        var newMessage = "Bağlantı kesildi";
            //        chat.mesage_list.Items.Add(newMessage);
            //    });
            //    return Task.CompletedTask;
            //};
        }

        private static User _loggedUser;
        public static User LoggedUser { get => _loggedUser; }

        public static void SetLoggedUser(User user)
        {
            _loggedUser = user;
        }
    }
}
