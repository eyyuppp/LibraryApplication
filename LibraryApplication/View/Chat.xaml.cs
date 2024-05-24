using Data.Entity;
using System.Windows;

namespace LibraryApplication.View
{
    /// <summary>
    /// Interaction logic for Chat.xaml
    /// </summary>
    public partial class Chat : Window
    {
        IList<Message> messageList;
        public Chat()
        {
            InitializeComponent();
            messageList = new List<Message>();
        }

        private void send_message(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string message = Message.Text;
            if (!string.IsNullOrEmpty(message))
            {
                messageList.Add(
                    new Message
                    {
                        DateTime = DateTime.Now,
                        Text = message,
                        User = "Eyyüp"
                    });
                mesage_list.Items.Clear();
                foreach (var item in messageList)
                {
                    mesage_list.Items.Add(item.User + " - " + item.Text);
                }
                Message.Clear();
            }
        }
    }
}
