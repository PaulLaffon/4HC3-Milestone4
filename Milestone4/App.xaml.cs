using Milestone4.View;
using Milestone4.ViewModel;
using System.Windows;

namespace Milestone4
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Called when the application starts.
        /// Initialize the application and displays the first window
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e">Not used</param>
        private void Start(object sender, StartupEventArgs e)
        {
            var wnd = new MainWindow();
            wnd.DataContext = new MainWindowViewModel(); 
            wnd.Show();
        }
    }
}
