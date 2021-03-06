﻿using Milestone4.Model;
using System.Collections.Generic;
using System.IO;
using System;

namespace Milestone4.ViewModel
{
    public class UserViewModel : ViewModel
    {
        private MainWindowViewModel mainVM;
        private UserFileVM selectedResume;
        private UserFileVM selectedCoverLetter;
        private string stringName;
        private string description;
        private string experiences;
        private string studies;
        private string projects;

        public User ManagedUser { get; private set; }

        public UserFileVM SelectedResume
        {
            get { return selectedResume; }
            set
            {
                selectedResume = value;
                OnPropertyChanged();
            }
        }

        public UserFileVM SelectedCoverLetter
        {
            get { return selectedCoverLetter; }
            set
            {
                selectedCoverLetter = value;
                OnPropertyChanged();
            }
        }

        public string StringName
        {
            get { return stringName; }
            set
            {
                stringName = value;
                OnPropertyChanged();
            }
        }
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }
        public string Experiences
        {
            get { return experiences; }
            set
            {
                experiences = value;
                OnPropertyChanged();
            }
        }
        public string Studies
        {
            get { return studies; }
            set
            {
                studies = value;
                OnPropertyChanged();
            }
        }
        public string Projects
        {
            get { return projects; }
            set
            {
                projects = value;
                OnPropertyChanged();
            }
        }

        public List<JobApplied> JobsApplied { get; private set; }
        public List<Job> SavedJobs { get; private set; }
        public List<UserFileVM> Resumes { get; private set; }
        public List<UserFileVM> CoverLetters { get; private set; }

        public UserViewModel(MainWindowViewModel mainVM, User user)
        {
            this.mainVM = mainVM;
            ManagedUser = user;
            UpdateResumes();
            UpdateCoverLetters();
            UpdateJobsSaved();
            selectedResume = null;
            selectedCoverLetter = null;
            stringName = ManagedUser.StringName;
            description = ManagedUser.Description;
            experiences = ManagedUser.Experiences;
            studies = ManagedUser.Studies;
            projects = ManagedUser.Projects;
        }

        public void SaveProfile()
        {
            ManagedUser.StringName = StringName;
            ManagedUser.Description = Description;
            ManagedUser.Experiences = Experiences;
            ManagedUser.Studies = Studies;
            ManagedUser.Projects = Projects;
        }

        public static string Extract(string filename)
        {
            return string.Format("{0} ({1})", Path.GetFileNameWithoutExtension(filename), Path.GetExtension(filename).ToUpper());
        }

        public void UpdateResumes()
        {
            Resumes = new List<UserFileVM>();
            foreach(string s in ManagedUser.Resumes)
                Resumes.Add(new UserFileVM(()=>DeleteResume(s), ()=>Open(s, true), Extract(s)));
            OnPropertyChanged("Resumes");
        }

        private void Open(string fileName, bool isResume)
        {
            try {
                System.Diagnostics.Process.Start(MainWindowViewModel.GetFilePath(ManagedUser.Name, fileName, isResume));
            }
            catch(Exception e)
            { }
        }

        private void DeleteResume(string fileName)
        {
            ManagedUser.Resumes.Remove(fileName);
            UpdateResumes();
        }

        public void UpdateCoverLetters()
        {
            CoverLetters = new List<UserFileVM>();
            foreach (string s in ManagedUser.CoverLetters)
                CoverLetters.Add(new UserFileVM(() => DeleteCoverLetter(s), () => Open(s, false), Extract(s)));
            OnPropertyChanged("CoverLetters");
        }

        private void DeleteCoverLetter(string fileName)
        {
            ManagedUser.CoverLetters.Remove(fileName);
            UpdateCoverLetters();
        }

        public void UpdateJobsApplied()
        {
            foreach (var j in ManagedUser.JobApplied)
            {
                j.Job.SeeJobCommand = new ButtonCommand(() => mainVM.SeeJob(j.Job));
            }
            JobsApplied = new List<JobApplied>(ManagedUser.JobApplied);
            OnPropertyChanged("JobsApplied");
        }

        public void UpdateJobsSaved()
        {
            foreach(Job j in ManagedUser.SavedJobs)
            {
                j.SeeJobCommand = new ButtonCommand(() => mainVM.SeeJob(j));
                j.UnsaveCommand = new ButtonCommand(()=>Unsave(j));
            }
            SavedJobs = new List<Job>(ManagedUser.SavedJobs);
            OnPropertyChanged("SavedJobs");
        }

        internal void Unsave(Job job)
        {
            ManagedUser.SavedJobs.RemoveAll(j=>j.Id == job.Id);
            UpdateJobsSaved();
        }

        internal void Save(Job job)
        {
            ManagedUser.SavedJobs.Add(job);
            UpdateJobsSaved();
        }
    }
}
