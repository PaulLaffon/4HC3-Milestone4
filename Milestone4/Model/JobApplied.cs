using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milestone4.Model
{
    public class JobApplied
    {
        public string ApplicationDate { get; set; }

        public Job Job { get; private set; }

        public JobApplied(Job j)
        {
            Job = j;
        }
    }
}
