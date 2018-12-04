using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milestone4.Model
{
    public class Job
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Level { get; set; }
        public string Salary { get; set; }
        public string YearOfExperience { get; set; }

        public string Description { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }

        public string City { get; set; }
        public string NbDaysToApply { get; set; }
    }
}
