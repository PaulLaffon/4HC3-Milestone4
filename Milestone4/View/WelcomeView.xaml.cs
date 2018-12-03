using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Milestone4.View
{
    /// <summary>
    /// Logique d'interaction pour WelcomeView.xaml
    /// </summary>
    public partial class WelcomeView : UserControl
    {
        public WelcomeView()
        {
            InitializeComponent();
        }

        private void BrowseJobsButton_Click(object sender, RoutedEventArgs e)
        {
            BrowseJobsButton.IsEnabled = false;
            //BrowseGrid
            QuickSearchButton.IsEnabled = true;
            QuickSearchGrid.Visibility = Visibility.Collapsed;
        }

        private void QuickSearchButton_Click(object sender, RoutedEventArgs e)
        {
            BrowseJobsButton.IsEnabled = true;
            //BrowseGrid
            QuickSearchButton.IsEnabled = false;
            QuickSearchGrid.Visibility = Visibility.Visible;
        }

        private void CreateProfButton_Click(object sender, RoutedEventArgs e)
        {
            CreateProfButton.IsEnabled = false;
            CreateProfGrid.Visibility = Visibility.Visible;
            SignInButton.IsEnabled = true;
            SignInGrid.Visibility = Visibility.Collapsed;
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            CreateProfButton.IsEnabled = true;
            CreateProfGrid.Visibility = Visibility.Collapsed;
            SignInButton.IsEnabled = false;
            SignInGrid.Visibility = Visibility.Visible;
        }
    }
    
}
