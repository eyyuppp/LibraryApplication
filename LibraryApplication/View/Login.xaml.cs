using Data.Entity;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows;

namespace LibraryApplication.View
{
    public partial class Login : Window
    {
        private readonly ICollection<User> users = new List<User>();
        private readonly Library library;
        public Login(Library library)
        {
            InitializeComponent();
            this.library = library;
        }

        private void Loggin_Button_Click(object sender, RoutedEventArgs e)
        {
            users.Add(new User
            {
                Password = "123",
                UserName = "eyyüp",
            });

            users.Add(new User
            {
                Password = "123",
                UserName = "kenan"
            });

            foreach (var user in users)
            {
                if (PasswordBox.Password.Equals(user.Password) && user.UserName.Equals(user.UserName))
                {
                    library.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("kullanıcı adı veya şifrenizi hatalı girdiniz", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
