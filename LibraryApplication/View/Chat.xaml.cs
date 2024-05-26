using Data.Entity;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows;

namespace LibraryApplication.View
{
    /// <summary>
    /// Interaction logic for Chat.xaml
    /// </summary>
    public partial class Chat : Window
    {
        private readonly ICollection<Message> messageList = new List<Message>();
        private HubConnection connection;
        public Chat()
        {
            InitializeComponent();

            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7109/chathub")
                .WithAutomaticReconnect()
                .Build();

            connection.Reconnecting += (sender) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = "Yeniden Bağlanıyor";
                    mesage_list.Items.Add(newMessage);
                });
                return Task.CompletedTask;
            };

            connection.Reconnected += (sender) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = "Yeniden bağlandı";
                    mesage_list.Items.Clear();
                    mesage_list.Items.Add(newMessage);
                });
                return Task.CompletedTask;
            };

            connection.Closed += (sender) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = "Bağlantı kesildi";
                    mesage_list.Items.Add(newMessage);
                    button_connect.IsEnabled=true;
                });
                return Task.CompletedTask;
            };
        }
      

       

        private async void connect_Click(object sender, RoutedEventArgs e)
        {
            connection.On<string>("ReceiveMessage", (message) => {
                this.Dispatcher.Invoke(() => {
                    var newNessage = $"{message}";
                    mesage_list.Items.Add(newNessage);
                    Message.Clear();
                });

            });

            try
            {
                await connection.StartAsync();
                user_list.Items.Add(connection.ConnectionId);
                button_connect.IsEnabled = false;
            }
            catch (Exception ex)
            {
                mesage_list.Items.Add(ex.Message);
                throw;
            }
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
    }
}
