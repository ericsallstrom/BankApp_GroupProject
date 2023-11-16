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

        public void UnblockCustomer()
        {
            while (true)
            {
                Console.Clear();

                if (_blockedUsers.Count > 0)
                {
                    Console.Write("Skriv in användarnamnet från listan på den användare du önskar återställa: ");
                    string username = Console.ReadLine();
                    PrintUsers();

                    foreach (var user in _blockedUsers)
                    {
                        if (user.Username == username)
                        {
                            _users.Add(user);
                            _blockedUsers.Remove(user);
                            Console.WriteLine("\nAnvändaren återställd.");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("\nAnvändaren finns inte i listan." +
                                              "\nTryck \"ENTER\" och försök igen.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Det finns för tillfället inga spärrade användare.\n" +
                                    "\nTryck \"ENTER\" för att återgå till föregående meny.");
                    Console.ReadKey();
                    break;
                }
            }
        }

        // Metod för att spärra en användare efter tre misslyckade försök.
        public void BlockCustomer(User user)
        {
            DeleteUser(user);
            _blockedUsers.Add(user);
        }

        // Metod för att kolla om en användare är spärrad och returnerar sedan användarnamnet
        public bool IsBlocked(User user)
        {
            foreach (var blockedUser in _blockedUsers)
            {
                if (blockedUser == user)
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
        public void PrintUsers()
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
