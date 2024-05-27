using Data.Entity;
using LibraryApplication.Helper;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;
using System.Windows;

namespace LibraryApplication.View
{
    public partial class Chat : Window
    {
        private readonly ICollection<Message> messageList = new List<Message>();
        public ObservableCollection<User> Users { get; set; }
        private HubConnection connection;
        public Chat()
        {
            InitializeComponent();
            Connection.ConnectionSignalR(connection, this);


            Users = new ObservableCollection<User>
        {
            new User { UserName = "Eyyup", Photo = "../images/eyyup.png"},
            new User { UserName = "Kenan", Photo = "../images/kenan.png"},
            new User { UserName = "Aleyna", Photo = "../images/aleyna.png"}
        };
            UserListView.ItemsSource = Users;
        }


        private async void send_message(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                await connection.InvokeAsync("SendMessage", Message.Text);
            }
            catch (Exception ex)
            {
                mesage_list.Items.Add(ex.Message);
                throw;
            }
        }

        private async void UserListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                await connection.InvokeAsync("SendSingletonMessage", Message.Text,connection.ConnectionId);
            }
            catch (Exception ex)
            {
                mesage_list.Items.Add(ex.Message);
                throw;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            connection.On<string>("ReceiveMessage", (message) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newNessage = $"{message}";
                    mesage_list.Items.Add(newNessage);
                    Message.Clear();
                });

            });
            try
            {
                await connection.StartAsync();
            }
            catch (Exception ex)
            {
                mesage_list.Items.Add(ex.Message);
                throw;
            }
        }
    }
}
