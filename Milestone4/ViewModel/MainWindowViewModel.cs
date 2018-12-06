using Milestone4.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Milestone4.ViewModel
{
    public class MainWindowViewModel : ViewModel
    {
        private static Regex EmailRegex = new Regex(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");
        private WebsiteData data;
        private UserViewModel userVM;
        private List<Job> jobsToDisplay;
        private ObservableCollection<Job> appliedJobs;
        private Job selectedJob;
        private Job uniqJobToDisplay;
        private Job jobApplyingTo;

        private SystemState state;
        private string userName = "";
        private string userNamelog = "";
        private string password = "";
        private string passwordlog = "";
        private string passwordConfirmation = "";
        private string email = "";
        private string errorMessage = "";
        public string comment = "";
        public string searchValue = "";

        public SystemState State
        {
            get { return state; }
            set
            {
                state = value;
                OnPropertyChanged();
                OnPropertyChanged("AlreadyApplied");
                OnPropertyChanged("IsCurrentJobSaved");
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
                OnPropertyChanged("IsCurrentJobSaved");
            }
        }

        public Job UniqJobToDisplay
        {
            get { return uniqJobToDisplay; }
            set
            {
                uniqJobToDisplay = value;
                OnPropertyChanged();
                OnPropertyChanged("IsUniqJobSaved");
                OnPropertyChanged("UniqAlreadyApplied");
            }
        }

        public Job JobApplyingTo
        {
            get { return jobApplyingTo; }
            set
            {
                jobApplyingTo = value;
                OnPropertyChanged();
            }
        }

        public bool IsCurrentJobSaved
        {
            get { return UserVM != null ? UserVM.ManagedUser.SavedJobs.FirstOrDefault(j => j.Id == SelectedJob.Id) != null : false; }
        }

        public bool IsUniqJobSaved
        {
            get { return UserVM != null ? UserVM.ManagedUser.SavedJobs.FirstOrDefault(j => j.Id == UniqJobToDisplay.Id) != null : false; }
        }

        public Visibility JobDetailsVisibility { get { return SelectedJob == null ? Visibility.Collapsed : Visibility.Visible; } }
        public Visibility JobDMessageVisibility { get { return SelectedJob == null ? Visibility.Visible : Visibility.Collapsed; } }

        public List<Job> JobsToDisplay
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

        public bool AlreadyApplied { get { return UserVM != null ? UserVM.ManagedUser.JobApplied.FirstOrDefault(j => j.JobId == SelectedJob.Id) != null : false; } }
        public bool UniqAlreadyApplied { get { return UserVM != null ? UserVM.ManagedUser.JobApplied.FirstOrDefault(j => j.JobId == UniqJobToDisplay.Id) != null : false; } }

        #region Search
        public string SearchValue
        {
            get { return searchValue; }
            set
            {
                searchValue = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands
        public ICommand ProfileCommand { get { return new ButtonCommand(() => { State = SystemState.Profile; }); } }
        public ICommand BrowseCommand { get { return new ButtonCommand(() => { State = SystemState.Browse; }); } }
        public ICommand ApplyViewCommand { get { return new ButtonCommand(() => { JobApplyingTo = SelectedJob; State = SystemState.Apply; }); } }
        public ICommand UniqApplyViewCommand { get { return new ButtonCommand(() => { JobApplyingTo = UniqJobToDisplay; State = SystemState.Apply; }); } }
        public ICommand AppliedJobsCommand { get { return new ButtonCommand(() => { State = SystemState.AppliedJobs; }); } }
        public ICommand SavedJobsCommand { get { return new ButtonCommand(() => { State = SystemState.SavedJobs; }); } }
        public ICommand FilesCommand { get { return new ButtonCommand(() => { State = SystemState.Files; }); } }

        public ICommand SearchCommand { get { return new ButtonCommand(Search); } }

        public ICommand LogCommand { get { return new ButtonCommand(Log); } }
        public ICommand RegisterCommand { get { return new ButtonCommand(Register); } }

        public ICommand ApplyCommand { get { return new ButtonCommand(Apply); } }
        public ICommand SaveJobCommand { get { return new ButtonCommand(()=> {
            if (IsCurrentJobSaved)
                UserVM.Unsave(SelectedJob);
            else
                UserVM.Save(SelectedJob);
            OnPropertyChanged("IsCurrentJobSaved");
            });
            }
        }
        public ICommand SaveUniqJobCommand
        {
            get
            {
                return new ButtonCommand(() => {
                    if (IsUniqJobSaved)
                        UserVM.Unsave(UniqJobToDisplay);
                    else
                        UserVM.Save(UniqJobToDisplay);
                    OnPropertyChanged("IsUniqJobSaved");
                });
            }
        }

        public ICommand AddNewRCommand { get { return new ButtonCommand(AddNewResume); } }
        public ICommand AddNewCLCommand { get { return new ButtonCommand(AddNewCoverLetter); } }
        #endregion

        #region Filters
        public List<FilterViewModel> JobTypeFilters { get; private set; }
        public List<FilterViewModel> SalaryFilters { get; private set; }
        public List<FilterViewModel> LevelFilters { get; private set; }
        public List<FilterViewModel> CategoryFilters { get; private set; }
        public List<FilterViewModel> CompanyFilters { get; private set; }
        public List<FilterViewModel> CityFilters { get; private set; }

        public void InitializeFilters()
        {
            JobTypeFilters = new List<FilterViewModel>();
            JobTypeFilters.Add(new FilterViewModel(this, "Type", "Job type", false));
            JobTypeFilters.AddRange(FilterViewModel.TypeFilterValues.Select(val => new FilterViewModel(this, "Type", val)));

            SalaryFilters = new List<FilterViewModel>();
            SalaryFilters.Add(new FilterViewModel(this, "Salary", "Salary", false));
            SalaryFilters.AddRange(FilterViewModel.SalaryFilterValues.Select(val => new FilterViewModel(this, "Salary", val)));

            LevelFilters = new List<FilterViewModel>();
            LevelFilters.Add(new FilterViewModel(this, "Level", "Level", false));
            LevelFilters.AddRange(FilterViewModel.LevelFilterValues.Select(val => new FilterViewModel(this, "Level", val)));

            CategoryFilters = new List<FilterViewModel>();
            CategoryFilters.Add(new FilterViewModel(this, "Category", "Category", false));
            CategoryFilters.AddRange(FilterViewModel.CategoryFilterValues.Select(val => new FilterViewModel(this, "Category", val)));

            CompanyFilters = new List<FilterViewModel>();
            CompanyFilters.Add(new FilterViewModel(this, "Company", "Company", false));
            CompanyFilters.AddRange(FilterViewModel.CompanyFilterValues.Select(val => new FilterViewModel(this, "Company", val)));

            CityFilters = new List<FilterViewModel>();
            CityFilters.Add(new FilterViewModel(this, "City", "City", false));
            CityFilters.AddRange(FilterViewModel.LocationFilterValues.Select(val => new FilterViewModel(this, "City", val)));
        }
        #endregion

        public MainWindowViewModel(WebsiteData data)
        {
            this.data = data;
            State = SystemState.Welcome;
            JobsToDisplay = new List<Job>(data.Jobs);
            UserVM = null;
            InitializeFilters();
        }

        public void Search()
        {
            JobsToDisplay = new List<Job>(data.Jobs.Where(j => j.Title.ToLower().Contains(SearchValue.ToLower()) || j.Description.ToLower().Contains(SearchValue.ToLower())));
        }

        public void SeeJob(Job j)
        {
            UniqJobToDisplay = j;
            State = SystemState.SeeJob;
        }

        private void Log()
        {
            var user = data.Users.FirstOrDefault(u => string.Equals(UserNameLog, u.Name));
            if (user != null && user.Password == PasswordLog)
            {
                UserVM = new UserViewModel(this, user);
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
            UserVM = new UserViewModel(this, user);
            ErrorMessage = null;
            State = SystemState.Browse;
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
                JobId = JobApplyingTo.Id,
                Job = JobApplyingTo
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
                    string dest = GetFilePath(UserVM.ManagedUser.Name, filename, true);
                    File.Copy(path, dest, true);
                    if(!UserVM.ManagedUser.Resumes.Contains(filename))
                        UserVM.ManagedUser.Resumes.Add(filename);
                    UserVM.UpdateResumes();
                    UserVM.SelectedResume = UserVM.Resumes.FirstOrDefault(f => f.FileName == UserViewModel.Extract(filename));
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
                    string dest = GetFilePath(UserVM.ManagedUser.Name, filename, false);
                    File.Copy(path, dest, true);
                    if (!UserVM.ManagedUser.CoverLetters.Contains(filename))
                        UserVM.ManagedUser.CoverLetters.Add(filename);
                    UserVM.UpdateCoverLetters();
                    UserVM.SelectedCoverLetter = UserVM.CoverLetters.FirstOrDefault(f => f.FileName == UserViewModel.Extract(filename));
                }
            }
            catch
            { }
        }

        public static string GetFilePath(string userName, string fileName, bool IsResume)
        {
            if(IsResume)
                return string.Format(@".\Data\users\{0}-R-{1}", userName, fileName);
            else
                return string.Format(@".\Data\users\{0}-CL-{1}", userName, fileName);
        }
    }
}
