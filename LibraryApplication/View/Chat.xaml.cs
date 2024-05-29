using Data.Entity;
using LibraryApplication.Helper;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows;

namespace LibraryApplication.View
{
    public partial class Chat : Window
    {
        public Chat()
        {
            InitializeComponent();
            ConnectionHelper.Connection.On<string, string>("ReceiveMessage", (senderUserName, message) =>
            {
                Dispatcher.Invoke(() =>
                {
                    mesage_list.Items.Add($"{senderUserName}:{message}");
                    Message.Clear();
                });
            });

            ConnectionHelper.Connection.On<List<User>>("ActiveUsersResult", (activeUsers) =>
            {
                Dispatcher.Invoke(() =>
                {
                    UserListView.Items.Clear();
                    foreach (var item in activeUsers)
                    {
                        UserListView.Items.Add(item);
                    }
                });
            });

            ConnectionHelper.Connection.SendAsync("GetAllActiveUsers");
        }


        private async void send_message(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                var selectedUser = UserListView.SelectedItem as User;
                if (selectedUser != null && Message.Text!=null)
                {
                    await ConnectionHelper.Connection.SendAsync("SendMessage", ConnectionHelper.LoggedUser.UserName, selectedUser.UserName, Message.Text);
                }
            }
            catch (Exception ex)
            {
                mesage_list.Items.Add(ex.Message);
                throw;
            }
        }
    }
}
