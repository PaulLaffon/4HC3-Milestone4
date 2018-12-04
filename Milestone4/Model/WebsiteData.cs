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
    }
}
