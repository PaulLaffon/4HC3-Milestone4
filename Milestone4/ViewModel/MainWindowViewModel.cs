using Milestone4.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milestone4.ViewModel
{
    public class MainWindowViewModel : ViewModel
    {
        private WebsiteData data;
        private User connectedUser;
        private ObservableCollection<Job> jobsToDisplay;
        private ObservableCollection<Job> appliedJobs;


        public ObservableCollection<Job> JobsToDisplay
        {
            get { return jobsToDisplay; }
            set
            {
                jobsToDisplay = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Job> AppliedJobs
        {
            get { return appliedJobs; }
            set
            {
                appliedJobs = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel(WebsiteData data)
        {
            this.data = data;
            AppliedJobs = null;
            JobsToDisplay = null;
            connectedUser = null;
        }

        private void Log()
        {

        }

        private void Register()
        {

        }
    }
}
