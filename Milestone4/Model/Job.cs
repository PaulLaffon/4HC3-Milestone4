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
        public int Salary { get; set; }
        public int YearOfExperience { get; set; }

        public string Description { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }

        public string City { get; set; }
        public int NbDaysToApply { get; set; }
        public string Category { get; set; }

        public Job(int id, string[] fields)
        {
            double salary = double.Parse(fields[11].Replace('.', ','));
            if (fields[12] == "Hourly") salary *= (150 * 12);
            if (fields[12] == "Daily") salary *= (12 * 20);

            Id = id;
            Title = fields[4];
            Type = fields[2];
            Salary = (int)salary;
            Level = fields[16];
            YearOfExperience = 5;
            Description = fields[15];
            CompanyName = "Google";
            Address = fields[13].Split(',')[0];
            City = "Hamilton";
            NbDaysToApply = 12;
            Category = fields[8];
        }
    }
}
