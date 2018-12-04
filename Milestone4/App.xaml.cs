using Milestone4.Model;
using Milestone4.View;
using Milestone4.ViewModel;
using System.Windows;
using Microsoft.VisualBasic.FileIO;

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
            wnd.DataContext = new MainWindowViewModel(GetData()); 
            wnd.Show();
        }

        /// <summary>
        /// Load the jobs from the file jobs.csv
        /// 
        /// </summary>
        /// <returns></returns>
        private WebsiteData GetData()
        {
            string filename = "data/jobs.csv";
            WebsiteData websiteData = new WebsiteData();

            using (TextFieldParser csvParser = new TextFieldParser(filename))
            {
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvParser.ReadLine();
                int id = 0;

                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    string[] fields = csvParser.ReadFields();

                    Job job = new Job(id, fields);
                    id++;
                    websiteData.Jobs.Add(job);
                }
            }

            User u = new User();
            u.Name = "Admin";
            u.Password = "1234";

            websiteData.Users.Add(u);

            return websiteData;
        }
    }
}
