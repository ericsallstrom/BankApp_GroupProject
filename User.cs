using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp_GroupProject
{
    internal class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public User(string username, string password)
        {
            UserName = username;
            Password = password;
        }
    }
}
