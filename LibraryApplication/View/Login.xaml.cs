using Data.Entity;
using System.Windows;

namespace LibraryApplication.View
{
    public partial class LoginWindow : Window
    {
        private readonly ICollection<User> users;
        public LoginWindow()
        {
            InitializeComponent();
            users = new List<User>();
        }

        private void Loggin_Button_Click(object sender, RoutedEventArgs e)
        {
            users.Add(new User
            {
                Password = "123",
                UserName = "eyyüp"
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
                    Library library = new Library();
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
