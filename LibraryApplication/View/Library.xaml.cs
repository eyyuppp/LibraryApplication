using System.Windows;

namespace LibraryApplication.View
{
    public partial class Library : Window
    {
        private readonly Chat _chat = new Chat();
        public Library()
        {
            InitializeComponent();
        }

        private void buton_kapat_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void support_click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _chat.Show();
            this.Close();
        }
    }
}
