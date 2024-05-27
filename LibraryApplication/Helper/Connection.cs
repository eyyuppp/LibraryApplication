using LibraryApplication.View;
using Microsoft.AspNetCore.SignalR.Client;

namespace LibraryApplication.Helper
{
    public static class Connection
    {
        public static void ConnectionSignalR(HubConnection connection,Chat chat)
        {
            connection = new HubConnectionBuilder()
           .WithUrl("https://localhost:7109/chathub")
           .WithAutomaticReconnect()
           .Build();

            connection.Reconnecting += (sender) =>
            {
                chat.Dispatcher.Invoke(() =>
                {
                    var newMessage = "Yeniden Bağlanıyor";
                    chat.mesage_list.Items.Add(newMessage);
                });
                return Task.CompletedTask;
            };

            connection.Reconnected += (sender) =>
            {
                chat.Dispatcher.Invoke(() =>
                {
                    var newMessage = "Yeniden bağlandı";
                   chat.mesage_list.Items.Clear();
                    chat.mesage_list.Items.Add(newMessage);
                });
                return Task.CompletedTask;
            };

            connection.Closed += (sender) =>
            {
                chat.Dispatcher.Invoke(() =>
                {
                    var newMessage = "Bağlantı kesildi";
                    chat.mesage_list.Items.Add(newMessage);
                });
                return Task.CompletedTask;
            };
        }
    }
}
