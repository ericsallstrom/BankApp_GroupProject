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
        public List<User> Users { get => _users; set => _users = value; }

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

        public bool Login(string username, string password)
        {
            foreach (var user in _users)
            {
                if (user.Username == username && user.Password == password)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
