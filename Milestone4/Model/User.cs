using System.Collections.Generic;

namespace Milestone4.Model
{
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public List<JobApplied> JobApplied { get; set; }
        public List<Job> SavedJobs { get; set; }


        public List<string> Resumes { get; set; }
        public List<string> CoverLetters { get; set; }
        //public Profile profile;

        public User()
        {
            JobApplied = new List<JobApplied>();
            SavedJobs = new List<Job>();
            Resumes = new List<string>();
            CoverLetters = new List<string>();
        }
    }
}
