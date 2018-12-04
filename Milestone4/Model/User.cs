using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milestone4.Model
{
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public List<JobApplied> JobApplied;
        public List<Job> SavedJobs;


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
