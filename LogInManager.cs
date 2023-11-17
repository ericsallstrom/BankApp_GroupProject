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

        public LogInManager()
        {
            // Fyll på med användare här
            _users = new List<User>()
                {
                    new("admin", "Password1@") {IsAdmin = true},
                    new Customer("olov", "Hej123@", "Olov", "Olsson"),
                    new Customer("olof", "Hej123@", "Olof", "Nordin"),
                    new Customer("eric", "Hej123@", "Eric", "Sällström"),
                    new Customer("patrik", "Hej123@", "Patrik", "Petterson"),
                    new Customer("hany", "Hej123@", "Hany", "Alhabboby"),
                    new Customer("amanda", "Hej123@", "Amanda", "Jönsson") { IsBlocked = true },
                    new Customer("hans", "Hej123@", "Hans", "Elofsson") { IsBlocked = true },
                    new Customer("karin", "Hej123@", "Karin", "Andersson") { IsBlocked = true }
                };
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
            Console.Clear();

            foreach (var user in _users)
            {
                if (user is Customer c)
                {
                    Console.WriteLine($"Lista över användare:" +
                                    $"\n---------------------" +
                                    $"\nNamn: {c.FirstName} {c.LastName}" +
                                    $"\nAnvändarnamn: {c.Username}\n");
                }
            }
        }

        public void UnblockCustomer()
        {
            Console.Clear();
            bool isRunning = true;
            var blockedCustomers = _users.FindAll(c => c.IsBlocked);

            if (blockedCustomers.Count > 0)
            {
                while (isRunning)
                {
                    Console.Clear();

                    PrintBlockedCustomers();

                    Console.Write("Skriv in användarnamnet på den användare du önskar återställa från listan ovan." +
                                "\nHar du ändrat dig? Tryck \"ENTER\" och avsluta sedan processen i menyn.\n" +
                                "\nAnvändarnamn: ");
                    string username = Console.ReadLine();

                    if (UsernameExistsInList(username))
                    {
                        var customerToUnblock = _users.Find(c => c.Username == username);
                        customerToUnblock.Unblock();
                        Console.WriteLine($"\nAnvändaren {customerToUnblock.Username} är nu återställd.");
                        Console.Write("\nTryck \"ENTER\" för att återgå till föregående meny.");
                        Console.ReadKey();
                        isRunning = false;
                    }
                    else
                    {
                        Console.Write($"\nAnvändaren finns inte registrerad i banken." +
                         "\nVill du försöka igen?" +
                         "\n[1] Ja" +
                         "\n[2] Nej" +
                         "\nSvar: ");
                        string answer = Console.ReadLine();

                        switch (answer)
                        {
                            case "2":
                                isRunning = false;
                                break;
                            default:
                                Console.Write("\nOgiltigt menyval! Tryck \"ENTER\" två gånger för att återkomma till menyn.");
                                Console.ReadKey();
                                break;
                        }
                    }
                }
            }
            else
            {
                Console.Write("Det finns för tillfället inga spärrade användare.\n" +
                           "\nTryck \"ENTER\" för att återgå till föregående meny.");
                Console.ReadKey();
            }
        }

        public void PrintBlockedCustomers()
        {
            var blockedCustomers = _users.FindAll(c => c.IsBlocked);

            Console.WriteLine("Lista över spärrade användare" +
                      "\n-----------------------------");
            foreach (var user in blockedCustomers)
            {
                if (user is Customer c)
                {
                    Console.WriteLine($"Namn: {c.FirstName} {c.LastName}" +
                                    $"\nAnvändarnamn: {c.Username}\n");
                }
            }
        }

        public void BlockCustomer(User customer)
        {
            if (_users.Contains(customer))
            {
                customer.Block();
                Console.Write("\nFör många felaktiga försök har genomförts" +
                         "\noch kontot kommer nu att spärras.\n" +
                         "\nTryck \"ENTER\" för att återgå till huvudmenyn.");
            }
            else
            {
                Console.Write("\nAnvändaren okänd...\n" +
                    "\nTryck \"ENTER\" för att återgå till huvudmenyn.");
            }
            Console.ReadKey();
        }

        // Metod för att kontrollera att varje användarnamn är unikt.
        public bool IsUsernameUnique(string username)
        {
            return UsernameExistsInList(username);
        }

        public bool ConfirmUserLogin(string username, string password)
        {
            if (_users.Exists(user => user.Username == username && user.CheckPassword(password)))
            {
                return true;
            }
            return false;
        }

        public User GetUserByUsername(string username)
        {
            if (!UsernameExistsInList(username))
            {
                return new User("n/a", "n/a"); // returnerar en tom användare som inte kommer kunna logga in
            }
            return _users.Find(user => user.Username == username);
        }

        private bool UsernameExistsInList(string username)
        {
            return _users.Exists(user => user.Username == username);
        }

        public Customer GetCustomer(string username)
        {
            return _users.OfType<Customer>().FirstOrDefault(c => c.Username == username);
        }
    }
}
