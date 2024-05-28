using LibraryApplication.Helper;
using System.Windows;

namespace LibraryApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ConnectionHelper.Connect();
        }
    }

}
