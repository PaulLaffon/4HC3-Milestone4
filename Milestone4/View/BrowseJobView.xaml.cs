using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Logique d'interaction pour BrowseJobView.xaml
    /// </summary>
    public partial class BrowseJobView : UserControl
    {
        ObservableCollection<Button> Filters = new ObservableCollection<Button>();

        public BrowseJobView()
        {
            InitializeComponent();
        }

        private bool handleSelect = true;
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (handleSelect && e.RemovedItems.Count > 0)
                {
                    ComboBox combo = (ComboBox)sender;
                    handleSelect = false;
                    combo.SelectedItem = e.RemovedItems[0];
                }
                handleSelect = true;
            }
            catch { }
        }

        private void SearchTextKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                SearchButton.Command.Execute(null);
            }
        }
    }
}


