using Data.Entity;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;
using System.Windows;

namespace LibraryApplication.View
{
    public partial class Chat : Window
    {
        public ObservableCollection<User> Users { get; set; }
        private HubConnection _connection;
        public Chat()
        {
            InitializeComponent();
            _connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7109/chathub")
            .WithAutomaticReconnect()
            .Build();
            //Connection.ConnectionSignalR(_connection,);


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
                await _connection.InvokeAsync("SendMessage", Message.Text);
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
                await _connection.InvokeAsync("SendSingletonMessage", Message.Text, _connection.ConnectionId);
            }
            catch (Exception ex)
            {
                mesage_list.Items.Add(ex.Message);
                throw;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            _connection.On<string>("ReceiveMessage", (message) =>
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
                await _connection.InvokeAsync("SendMessage", Message.Text);
                await _connection.StartAsync();
            }
            catch (Exception ex)
            {
                mesage_list.Items.Add(ex.Message);
                throw;
            }
        }
    }
}
