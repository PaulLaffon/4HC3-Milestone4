using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milestone4.Model
{
    public class WebsiteData
    {
        public List<User> Users { get; set; }

        public List<Job> Jobs { get; set; }

        public WebsiteData()
        {
            Users = new List<User>();
            Jobs = new List<Job>();
        }

       public List<Job> Search(string keywords, 
                               List<string> salaries, 
                               List<string> companies,
                               List<string> levels,
                               List<string> cities,
                               List<string> categories,
                               List<string> types)
        {
            IEnumerable<Job> matching = Jobs;
            IEnumerable<Job> matchingCurr = new List<Job>();

            foreach (string keyword in keywords.Split(' '))
            {
                matchingCurr = matchingCurr.Union(Jobs.Where(j => j.Title.Contains(keyword) 
                                                                  || j.Description.Contains(keyword)));
            }

            matching = matching.Intersect(matchingCurr);
            matchingCurr = new List<Job>();

            foreach(string salary in salaries)
            {
                int lower = 0;
                int upper = 0;

                if(salary == "Less than $20,000") {
                    lower = 0; upper = 20000;
                } else if(salary == "$20,000 - $40,000") {
                    lower = 20000; upper = 40000;
                } else if (salary == "$40,000 - $60,000") {
                    lower = 40000; upper = 60000;
                } else if (salary == "$60,000 - $80,000") {
                    lower = 60000; upper = 80000;
                } else if (salary == "$80,000 - $100,000") {
                    lower = 80000; upper = 100000;
                } else {
                    lower = 100000; upper = (int)1e9;
                }

                matchingCurr = matchingCurr.Union(Jobs.Where(j => j.Salary <= upper && j.Salary >= lower));
            }

            foreach(string company in companies)
            {
                matchingCurr = matchingCurr.Union(Jobs.Where(j => j.CompanyName.Contains(company)));
            }

            matching = matching.Intersect(matchingCurr);
            matchingCurr = new List<Job>();

            foreach (string level in levels)
            {
                matchingCurr = matchingCurr.Union(Jobs.Where(j => j.Level.Contains(level)));
            }

            matching = matching.Intersect(matchingCurr);
            matchingCurr = new List<Job>();

            foreach (string city in cities)
            {
                matchingCurr = matchingCurr.Union(Jobs.Where(j => j.City.Contains(city)));
            }

            matching = matching.Intersect(matchingCurr);
            matchingCurr = new List<Job>();

            foreach (string category in categories)
            {
                matchingCurr = matchingCurr.Union(Jobs.Where(j => j.Category.Contains(category)));
            }

            matching = matching.Intersect(matchingCurr);
            matchingCurr = new List<Job>();

            foreach (string type in types)
            {
                matchingCurr = matchingCurr.Union(Jobs.Where(j => j.Type.Contains(type)));
            }

            matching = matching.Intersect(matchingCurr);
            matchingCurr = new List<Job>();


            return matching.ToList();
        }
    }
}
