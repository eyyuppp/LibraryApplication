using Data.Entity;
using System.Windows;

namespace LibraryApplication.View
{
    public partial class Login : Window
    {
        private readonly ICollection<User> users = new List<User>();
        private readonly Library library = new Library();
        public Login()
        {
            InitializeComponent();
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
