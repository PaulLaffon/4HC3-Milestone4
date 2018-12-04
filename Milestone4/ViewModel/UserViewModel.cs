using Milestone4.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milestone4.ViewModel
{
    public class UserViewModel : ViewModel
    {
        public User ManagedUser { get; private set; }


        public UserViewModel(User user)
        {
            ManagedUser = user;
        }
    }
}
