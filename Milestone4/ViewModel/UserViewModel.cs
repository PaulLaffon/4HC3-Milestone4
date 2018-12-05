using Milestone4.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Milestone4.ViewModel
{
    public class UserViewModel : ViewModel
    {
        private string selectedResume;
        private string selectedCoverLetter;

        public User ManagedUser { get; private set; }

        public string SelectedResume
        {
            get { return selectedResume; }
            set
            {
                selectedResume = value;
                OnPropertyChanged();
            }
        }

        public string SelectedCoverLetter
        {
            get { return selectedCoverLetter; }
            set
            {
                selectedCoverLetter = value;
                OnPropertyChanged();
            }
        }

        public List<JobApplied> JobsApplied { get; private set; }
        public List<string> Resumes { get; private set; }
        public List<string> CoverLetters { get; private set; }

        public UserViewModel(User user)
        {
            ManagedUser = user;
            UpdateResumes();
            UpdateCoverLetters();
            selectedResume = null;
            selectedCoverLetter = null;
        }

        public static string Extract(string filename)
        {
            return string.Format("{0} ({1})", Path.GetFileNameWithoutExtension(filename), Path.GetExtension(filename).ToUpper());
        }

        public void AddResume(string filename)
        {
            ManagedUser.Resumes.Add(filename);
            Resumes.Add(Extract(filename));
        }

        public void AddCoverLetter(string filename)
        {
            ManagedUser.CoverLetters.Add(filename);
            CoverLetters.Add(Extract(filename));
        }

        public void UpdateResumes()
        {
            Resumes = new List<string>();
            foreach(string s in ManagedUser.Resumes)
                Resumes.Add(Extract(s));
            OnPropertyChanged("Resumes");
        }

        public void UpdateCoverLetters()
        {
            CoverLetters = new List<string>();
            foreach (string s in ManagedUser.CoverLetters)
                CoverLetters.Add(Extract(s));
            OnPropertyChanged("CoverLetters");
        }

        public void UpdateJobsApplied()
        {
            JobsApplied = new List<JobApplied>(ManagedUser.JobApplied);
            OnPropertyChanged("JobsApplied");
        }
    }
}
