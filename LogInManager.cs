using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp_GroupProject
{
    public class LogInManager
    {
        private readonly List<User> _users;
        private readonly List<User> _blockedUsers;

        public LogInManager()
        {
            // Fyll på med användare här
            _users = new List<User>()
                {
                    new Admin("admin", "Password1@"),
                    new Customer("olov", "Hej123@", "Olov", "Olsson"),
                    new Customer("olof", "Hej123@", "Olof", "Nordin"),
                    new Customer("eric", "Hej123@", "Eric", "Sällström"),
                    new Customer("patrik", "Hej123@", "Patrik", "Petterson"),
                    new Customer("hany", "Hej123@", "Hany", "Alhabboby")
                };

            _blockedUsers = new List<User>();
        }

        // Metod för att spärra en användare efter tre misslyckade försök.
        public void BlockUser(string username)
        {           
            foreach (var user in _users)
            {
                if (user.Username == username)
                {
                    _blockedUsers.Add(user);
                    DeleteUser(user);                  
                }
            }
        }

        // Metod för att kolla om en användare är spärrad och returnerar sedan användarnamnet
        public bool IsBlocked(string username)
        {
            foreach(var user in _blockedUsers)
            {
                if (user.Username == username)
                {
                    return true;
                }
            }
            return false;
        }

      
        // Publik metod som lägger till en användare i listan.
        public void AddUser(User user)
        {
            _users.Add(user);
        }

        // Här kan man ta bort användare från listan
        public void DeleteUser(User user)
        {
            _users.Remove(user);
        }

        //Skapade en metod som visar alla användare
        public void PrintUsers(List <User> _users)
        {
            foreach (var user in _users)
            {
                Console.WriteLine(user);
            }

        }

        // Metod för att kontrollera att varje användarnamn är unikt.
        public bool IsUsernameUnique(string username)
        {
            return _users.Exists(user => user.Username == username);
        }

        public bool ConfirmUser(string username, string password)
        {
            foreach (var user in _users)
            {
                if (user.Username == username && user.CheckPassword(password))
                {
                    return true;
                }
            }
            return false;
        }


        public Customer GetCustomerByUsername(string username)
        {
            return _users.OfType<Customer>().FirstOrDefault(c => c.Username == username);
        }
    }
}
