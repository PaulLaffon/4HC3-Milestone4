using Milestone4.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
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

        public SystemState State
        {
            get { return state; }
            set
            {
                state = value;
                OnPropertyChanged();
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

        public ObservableCollection<Job> AppliedJobs
        {
            get { return appliedJobs; }
            set
            {
                appliedJobs = value;
                OnPropertyChanged();
            }
        }

        #region Commands
        public ICommand LogCommand { get { return new ButtonCommand(Log); } }
        public ICommand RegisterCommand { get { return new ButtonCommand(Register); } }
        #endregion

        public MainWindowViewModel(WebsiteData data)
        {
            this.data = data;
            State = SystemState.Welcome;
            AppliedJobs = null;
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
            State = SystemState.Browse;
        }
    }
}
