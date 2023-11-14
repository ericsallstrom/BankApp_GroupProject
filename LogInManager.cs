using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp_GroupProject
{
    internal class LogInManager
    {
        private List<User> _users;

        public LogInManager()
        {
            _users = new List<User>();
            // Fyll på med användare här
            _users.Add(new User("admin", "password"));
            _users.Add(new User("olov", "hej123"));
            _users.Add(new User("olof", "hej123"));
            _users.Add(new User("eric", "hej123"));
            _users.Add(new User("patrik", "hej123"));
        }

        public bool Login(string username, string password)
        {
            foreach (var user in _users)
            {
                if(user.UserName == username && user.Password == password)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
