using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp_GroupProject
{
    internal class LogInManager
    {
        private readonly List<User> _users;

        public LogInManager()
        {
            // Fyll på med användare här
            _users = new List<User>()
                {
                    new Admin("admin", "Password1@"),
                    new Customer("olov", "Hej123@"),
                    new Customer("olof", "Hej123@"),
                    new Customer("eric", "Hej123@"),
                    new Customer("patrik", "Hej123@"),
                    new Customer("hany", "Hej123@")
                };
        }

        public void AddUser(User user)
        {
            _users.Add(user);
        }

        public bool IsUsernameUnique(string enteredUsername)
        {
            return !_users.Exists(user => user.Username == enteredUsername);
        }

        public bool Login(string enteredUsername, string enteredPassword)
        {
            foreach (User user in _users)
            {
                if (user.Username == enteredUsername && user.CheckPassword(enteredPassword))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
