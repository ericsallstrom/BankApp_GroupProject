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
                    new Admin("admin", "password"),
                    new Customer("olov", "hej123"),
                    new Customer("olof", "hej123"),
                    new Customer("eric", "hej123"),
                    new Customer("patrik", "hej123"),
                    new Customer("hany", "hej123")
                };
        }

        public void AddUser(User user)
        {
            _users.Add(user);
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
