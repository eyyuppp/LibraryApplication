using LibraryApplication.Helper;
using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;
using System.Windows;

namespace LibraryApplication.View
{
    public partial class Login : Window
    {

        public Login()
        {
            InitializeComponent();

            ConnectionHelper.Connection.On<bool>("LoginResult", isSuccess =>
            {
                if (isSuccess)
                {
                    
                    this.Dispatcher.Invoke(() =>
                    {
                        ConnectionHelper.SetLoggedUser(new Data.Entity.User() { UserName = UserNameTextBox.Text, Password = PasswordBox.Password });
                        Library library = new Library();
                        library.Show();
                        this.Close();
                    });
                }
                else
                {
                    MessageBox.Show("Kullanıcı username veya password hatalı girdiniz");
                }
            });
        }

        private async void Loggin_Button_Click(object sender, RoutedEventArgs e)
        {
          
            await ConnectionHelper.Connection.InvokeAsync("Login", UserNameTextBox.Text, PasswordBox.Password);
        }
    }
}
