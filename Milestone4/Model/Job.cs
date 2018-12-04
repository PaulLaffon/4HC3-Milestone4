using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milestone4.Model
{
    public class Job
    {
        public static string[] Companies = {
            "Google", "Amazon", "Microsoft", "Apple",
            "Tim Hortons", "FreshBooks", "Dollarama",
            "Rockstar", "Ubisoft", "Shell Canada", 
            "CIBC", "Scotiabank"
        };

        public static string[] Logos = {
            "data/logo/google.png", "data/logo/amazon.jpg", "data/logo/microsoft.jpg", "data/logo/apple.jpg",
            "data/logo/tim_hortons.jpg", "data/logo/fresh.png", "data/logo/dollarama.png",
            "data/logo/rockstar.jpg", "data/logo/ubisoft.jpg", "data/logo/shell.png",
            "data/logo/cibc.png", "data/logo/scotia.jpg"
        };

        public static string[] Cities = {
            "Hamilton", "Toronto", "Waterloo", "London", "Montreal", "Windsor", "Vancouver"
        };


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
        public string Logo { get; set; }

        public Job(int id, string[] fields)
        {
            Random random = new Random();
            int companyIndex = random.Next(0, Job.Companies.Length);
            int cityIndex = random.Next(0, Job.Cities.Length);

            double salary = double.Parse(fields[11].Replace('.', ','));
            if (fields[12] == "Hourly") salary *= (150 * 12);
            if (fields[12] == "Daily") salary *= (12 * 20);

            Id = id;
            Title = fields[4];
            Type = fields[2];
            Salary = (int)salary;
            Level = fields[16];
            YearOfExperience = random.Next(0, 6);
            Description = fields[15];
            CompanyName = Companies[companyIndex];
            Logo = Logos[companyIndex];
            Address = fields[13].Split(',')[0];
            City = Cities[cityIndex];
            NbDaysToApply = random.Next(2, 60);
            Category = fields[8];
        }
    }
}
