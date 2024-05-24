using LibraryApplication;
using System.Windows;
using System.Windows.Controls;

namespace LoginApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Loggin_Button_Click(object sender, RoutedEventArgs e)
        {
            
            if (PasswordBox.Password.Equals("1234") && UserNameTextBox.Text.Equals("user"))
            {
                LibraryMainWindow library=new LibraryMainWindow();
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
