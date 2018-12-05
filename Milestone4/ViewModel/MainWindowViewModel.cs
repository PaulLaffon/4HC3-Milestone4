using Milestone4.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Milestone4.ViewModel
{
    public class MainWindowViewModel : ViewModel
    {
        private static Regex EmailRegex = new Regex(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");
        private WebsiteData data;
        private UserViewModel userVM;
        private ObservableCollection<Job> jobsToDisplay;
        private ObservableCollection<Job> appliedJobs;
        private Job selectedJob;

        private SystemState state;
        private string userName = "";
        private string userNamelog = "";
        private string password = "";
        private string passwordlog = "";
        private string passwordConfirmation = "";
        private string email = "";
        private string errorMessage = "";
        public string comment = "";

        //Search and Filter variables
        private String searchText = "";
        private String searchCategory = "";
        private String searchLocation = "";

        private ObservableCollection<Filter> filters;


        public SystemState State
        {
            get { return state; }
            set
            {
                state = value;
                OnPropertyChanged();
                OnPropertyChanged("AlreadyApplied");
            }
        }

        public UserViewModel UserVM
        {
            get { return userVM; }
            set
            {
                userVM = value;
                OnPropertyChanged();
            }
        }

        public string UserNameLog
        {
            get { return userNamelog; }
            set
            {
                userNamelog = value;
                OnPropertyChanged();
            }
        }

        public string PasswordLog
        {
            get { return passwordlog; }
            set
            {
                passwordlog = value;
                OnPropertyChanged();
            }
        }

        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        public string PasswordConfirmation
        {
            get { return passwordConfirmation; }
            set
            {
                passwordConfirmation = value;
                OnPropertyChanged("PasswordConfirmation");
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }

        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get { return string.IsNullOrEmpty(errorMessage) ? null : errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        public Job SelectedJob
        {
            get { return selectedJob; }
            set
            {
                selectedJob = value;
                OnPropertyChanged();
                OnPropertyChanged("JobDetailsVisibility");
                OnPropertyChanged("JobDMessageVisibility");
                OnPropertyChanged("AlreadyApplied");
            }
        }

        public Visibility JobDetailsVisibility { get { return SelectedJob == null ? Visibility.Collapsed : Visibility.Visible; } }
        public Visibility JobDMessageVisibility { get { return SelectedJob == null ? Visibility.Visible : Visibility.Collapsed; } }

        public ObservableCollection<Job> JobsToDisplay
        {
            get { return jobsToDisplay; }
            set
            {
                jobsToDisplay = value;
                OnPropertyChanged();
                if (jobsToDisplay.Count > 0)
                    SelectedJob = jobsToDisplay[0];
                else
                    SelectedJob = null;
            }
        }


        private ObservableCollection<string> salaries = new ObservableCollection<string> { "Less than $20,000", "$20,000 - $40,000", "$40,000 - $60,000", "$60,000 - $80,000", "$80,000 - $100,000", "Over $100,000" };
        public ObservableCollection<string> Salaries
        {
            get { return salaries; }
            set
            { salaries = value;
                OnPropertyChanged();
                if (salaries.Count > 0)
                    SalaryFilter = salaries[0];
                else
                    SalaryFilter = null;
            }
        }
        public ObservableCollection<string> companies = new ObservableCollection<string> { "Apple", "Tim Hortons", "CIBC", "Rockstar", "Google"};
        public ObservableCollection<string> Companies
        {
            get { return companies; }
            set
            {
                companies = value;
                OnPropertyChanged();
                if (companies.Count > 0)
                    CompanyFilter = companies[0];
                else
                    CompanyFilter = null;
            }
        }
        public ObservableCollection<string> levels = new ObservableCollection<string> { "0", "1", "2", "3", "4" };
        public ObservableCollection<string> Levels
        {
            get { return levels; }
            set
            {
                levels = value;
                OnPropertyChanged();
                if (levels.Count > 0)
                    LevelFilter = levels[0];
                else
                    LevelFilter = null;
            }
        }
        public ObservableCollection<string> locations = new ObservableCollection<string> { "Vancouver", "Toronto", "Montreal", "Hamilton", "Ottawa", "New York" };
        public ObservableCollection<string> Locations
        {
            get { return locations; }
            set
            {
                locations = value;
                OnPropertyChanged();
                if (locations.Count > 0)
                    LocationFilter = locations[0];
                else
                    LocationFilter = null;
            }
        }
        public ObservableCollection<string> categories = new ObservableCollection<string> { "Finance", "Human Resources", "Research", "Engineering", "Maintenance" };
        public ObservableCollection<string> Categories
        {
            get { return categories; }
            set
            {
                categories = value;
                OnPropertyChanged();
                if (categories.Count > 0)
                    CategoryFilter = categories[0];
                else
                    CategoryFilter = null;
            }
        }

        public String SalaryFilter { get; set; }
        public String CompanyFilter { get; set; }
        public String LevelFilter { get; set; }
        public String LocationFilter { get; set; }
        public String CategoryFilter { get; set; }

        public Boolean FilterActive { get; set; }

        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged();
            }

        }

        public string SearchLocation
        {
            get { return searchLocation; }
            set
            {
                searchLocation = value;
                OnPropertyChanged();
            }

        }

        public bool AlreadyApplied { get { return UserVM != null ? UserVM.ManagedUser.JobApplied.FirstOrDefault(j=>j.JobId == SelectedJob.Id) != null : false; } }

        #region Commands
        public ICommand ProfileCommand { get { return new ButtonCommand(()=> { State = SystemState.Profile; }); } }
        public ICommand BrowseCommand { get { return new ButtonCommand(()=> { State = SystemState.Browse; }); } }
        public ICommand ApplyViewCommand { get { return new ButtonCommand(() => { State = SystemState.Apply; }); } }
        public ICommand AppliedJobsCommand { get { return new ButtonCommand(() => { State = SystemState.AppliedJobs; }); } }
        public ICommand SavedJobsCommand { get { return new ButtonCommand(() => { State = SystemState.SavedJobs; }); } }
        public ICommand FilesCommand { get { return new ButtonCommand(() => { State = SystemState.Files; }); } }
        public ICommand SearchJobsCommand { get { return new ButtonCommand(SearchJobs); } }
        public ICommand FilterJobsCommand { get { return new ButtonCommand(FilterJobs); } }

        public ICommand LogCommand { get { return new ButtonCommand(Log); } }
        public ICommand RegisterCommand { get { return new ButtonCommand(Register); } }

        public ICommand ApplyCommand { get { return new ButtonCommand(Apply); } }

        public ICommand AddNewRCommand { get { return new ButtonCommand(AddNewResume); } }
        public ICommand AddNewCLCommand { get { return new ButtonCommand(AddNewCoverLetter); } }
        #endregion

        public MainWindowViewModel(WebsiteData data)
        {
            this.data = data;
            State = SystemState.Welcome;
            JobsToDisplay = new ObservableCollection<Job>(data.Jobs);
            UserVM = null;
        }

        private void Log()
        {
            var user = data.Users.FirstOrDefault(u => string.Equals(UserNameLog, u.Name));
            if (user != null && user.Password == PasswordLog)
            {
                UserVM = new UserViewModel(user);
                State = SystemState.Browse;
            }
            else
                ErrorMessage = "Authentication failed, please try again";
            ErrorMessage = null;
        }

        private void Register()
        {
            var user = data.Users.FirstOrDefault(u => string.Equals(UserName, u.Name));
            if (user != null)
            {
                ErrorMessage = "Username not available, please try again";
                return;
            }
            if(string.IsNullOrWhiteSpace(UserName))
            {
                ErrorMessage = "Enter a username";
                return;
            }
            if (string.IsNullOrWhiteSpace(Email) || !EmailRegex.IsMatch(Email))
            {
                ErrorMessage = "Enter a valid email";
                return;
            }
            if (UserName.Replace(" ", "") != UserName)
            {
                ErrorMessage = "Unallowed characters in username";
                return;
            }
            if (string.IsNullOrEmpty(Password) || Password != PasswordConfirmation)
            {
                ErrorMessage = "Password confirmation failed";
                return;
            }
            user = new User() { Name = UserName, Password = Password };
            data.Users.Add(user);
            //PersistenceManager.Save(data);
            UserVM = new UserViewModel(user);
            ErrorMessage = null;
            State = SystemState.Browse;
        }

        public void SearchJobs()
        {
            IEnumerable<Job> matchingJobs = FilterActive ? FilteredJobs() : data.Jobs; //Search all jobs unless a filter is active 
            if (!String.IsNullOrEmpty(searchText))
            {
                matchingJobs = matchingJobs.Where(x => x.CompanyName == searchText || x.Level == searchText || x.Salary.ToString() == searchText || x.City == searchText || x.Description.Contains(searchText));
            }
            if (!String.IsNullOrEmpty(searchLocation))
            {
                 matchingJobs = matchingJobs.Where(x => x.City == searchLocation);
            }
            JobsToDisplay = new ObservableCollection<Job>(matchingJobs);
        }
        public void FilterJobs()
        {
            JobsToDisplay = new ObservableCollection<Job>(FilteredJobs());
        }
        public List<Job> FilteredJobs()
        {

            IEnumerable<Job> matchingJobs = data.Jobs;
            if (!String.IsNullOrEmpty(CategoryFilter))
            {
                matchingJobs = matchingJobs.Where(x => x.Category.Contains(CategoryFilter));
                FilterActive = true;
            };
            if (!String.IsNullOrEmpty(SalaryFilter))
            {
                //Not Implemented
                FilterActive = true;
            };
            if (!String.IsNullOrEmpty(LevelFilter))
            {
                //Not Working
                matchingJobs = matchingJobs.Where(x => x.Level == LevelFilter);
                FilterActive = true;
            };
            if (!String.IsNullOrEmpty(LocationFilter))
            {
                matchingJobs = matchingJobs.Where(x => x.City == LocationFilter);
                FilterActive = true;
            };
            if (!String.IsNullOrEmpty(CompanyFilter))
            {
                matchingJobs = matchingJobs.Where(x => x.CompanyName == CompanyFilter);
                FilterActive = true;
            };
            return new List<Job>(matchingJobs);
        }


        public void Apply()
        {
            if(UserVM.SelectedResume == null)
            {
                ErrorMessage = "Please use the dropdown to pick up a resume";
                return;
            }
            if(UserVM.SelectedCoverLetter == null)
            {
                ErrorMessage = "Please use the dropdown to pick up a cover letter or upload a new one.";
                return;
            }
            ErrorMessage = null;
            UserVM.ManagedUser.JobApplied.Add(new JobApplied()
            {
                ApplicationDate = DateTime.Now.ToString("M", CultureInfo.InvariantCulture),
                JobId = SelectedJob.Id,
                Job = SelectedJob
            });
            UserVM.SelectedCoverLetter = null;
            UserVM.SelectedResume = null;
            Comment = null;
            UserVM.UpdateJobsApplied();
            State = SystemState.AppliedJobs;
        }

        public void AddNewResume()
        {
            try
            {
                // Configure open file dialog box
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.Title = "Upload a new resume";
                dlg.Multiselect = false;
                dlg.CheckFileExists = true;
                dlg.CheckPathExists = true;
                dlg.Filter = "All supported files (.pdf, .jpg, .jpeg, .png)|*.jpg;*.png;*.jpeg;*.pdf|PDF files (.pdf)|*.jpg|Images files (.jpg, .jpeg, .png)|*.jpg;*.png;*.jpeg"; // Filter files by extension 

                bool? result = dlg.ShowDialog();

                if (result == true)
                {
                    string path = dlg.FileName;
                    string filename = Path.GetFileName(path);
                    string dest = string.Format(@".\Data\users\{0}-R-{1}", UserVM.ManagedUser.Name, filename);
                    File.Copy(path, dest, true);
                    UserVM.ManagedUser.Resumes.Add(filename);
                    UserVM.UpdateResumes();
                    UserVM.SelectedResume = UserViewModel.Extract(filename);
                }
            }
            catch
            { }
        }

        public void AddNewCoverLetter()
        {
            try
            {
                // Configure open file dialog box
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.Title = "Upload a new cover letter";
                dlg.Multiselect = false;
                dlg.CheckFileExists = true;
                dlg.CheckPathExists = true;
                dlg.Filter = "All supported files (.pdf, .jpg, .jpeg, .png)|*.jpg;*.png;*.jpeg;*.pdf|PDF files (.pdf)|*.jpg|Images files (.jpg, .jpeg, .png)|*.jpg;*.png;*.jpeg"; // Filter files by extension 

                bool? result = dlg.ShowDialog();

                if (result == true)
                {
                    string path = dlg.FileName;
                    string filename = Path.GetFileName(path);
                    string dest = string.Format(@".\Data\users\{0}-CL-{1}", UserVM.ManagedUser.Name, filename);
                    File.Copy(path, dest, true);
                    UserVM.ManagedUser.CoverLetters.Add(filename);
                    UserVM.UpdateCoverLetters();
                    UserVM.SelectedCoverLetter = UserViewModel.Extract(filename);
                }
            }
            catch
            { }
        }
    }
}
