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


            Users = new ObservableCollection<User>
        {
            new User {Id="1", UserName = "Eyyup", Photo = "../images/eyyup.png"},
            new User {Id="2",UserName = "Kenan", Photo = "../images/kenan.png"},
            new User {Id="3", UserName = "Aleyna", Photo = "../images/aleyna.png"}
        };
            UserListView.ItemsSource = Users;
            ConnectSignalR();
        }


        private async void send_message(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
               // await _connection.InvokeAsync("SendMessage", Message.Text);
                await _connection.InvokeAsync("SendSingletonMessage", Message.Text, Message.Text);
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
                var selectedUser = UserListView.SelectedItem as User;
                if (selectedUser != null)
                {
                    Message.Text = selectedUser.Id;
                    mesage_list.Items.Clear();
                }

            }
            catch (Exception ex)
            {
                mesage_list.Items.Add(ex.Message);
                throw;
            }
        }

        private async void ConnectSignalR()
        {
            try
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
