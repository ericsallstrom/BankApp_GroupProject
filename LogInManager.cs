using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace BankApp_GroupProject
{
    public class LogInManager
    {
        private readonly List<Customer> _customers;
        private readonly Admin _admin;        

        public LogInManager()
        {
            _admin = new Admin("admin", "Password1@");            

            _customers = new List<Customer>()
                {
                    new Customer("olov", "Hej123@", "Olov", "Olsson"),
                    new Customer("olof", "Hej123@", "Olof", "Nordin"),
                    new Customer("eric", "Hej123@", "Eric", "Sällström"),
                    new Customer("patrik", "Hej123@", "Patrik", "Petterson"),
                    new Customer("hany", "Hej123@", "Hany", "Alhabboby"),
                    new Customer("hans", "Hej123@", "Hans", "Elofsson") { IsBlocked = true },
                    new Customer("karin", "Hej123@", "Karin", "Andersson") { IsBlocked = true }
                };

            InitUserAccounts();
        }

        //Tilldelar alla users ett lönekonto och ett random saldo
        public void InitUserAccounts()
        {
            Random random = new();           

            foreach (var customer in _customers)
            {
                decimal initialBalance = random.Next(100, 30001);
                Account checkingAccount = new(AccountType.Checking, customer);
                checkingAccount.MakeDeposit(initialBalance);
                customer.CustomerAccounts.Add(checkingAccount);
            }
        }

        public List<Customer> GetAllCustomers()
        {
            return _customers;
        }

        public void DeleteExistingCustomer()
        {
            string username;
            string heading = "VARNING! Denna åtgärd är ej reversibel." +
                           "\nFör att ta bort en användare från systemet behöver du mata in användarens " +
                           "\nanvändarnamn och sedan mata in ditt lösenord för att åtgärden skall fastställas.\n";
            while (true)
            {
                Console.Clear();
                PrintUsers();

                Console.WriteLine(heading);
                Console.Write("Användarnamn: ");
                username = Console.ReadLine();

                if (UsernameExistsInList(username))
                {
                    break;
                }
                else
                {
                    Console.Write("\nFelaktig inamtning! Användaren finns inte i systemet.\n" +
                                    "Tryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                }
            }

            while (true)
            {
                Console.Clear();
                PrintUsers();

                Console.Write($"{heading}" +
                              $"\nAnvändarnamn: {username}" +
                              $"\nAnge ditt lösenord: ");
                string adminPassword = Console.ReadLine();

                if (_admin.CheckPassword(adminPassword))
                {
                    Console.Write($"\nÄr du säker på att du vill ta bort användaren {username}?" +
                                  $"\n[1] Ja" +
                                  $"\n[2] Nej" +
                                  $"\n---" +
                                  $"\nDitt val: ");
                    string answer = Console.ReadLine();

                    if (answer == "1")
                    {
                        var customerToRemove = _customers.Find(c => c.Username == username);
                        DeleteCustomer(customerToRemove);
                        Console.Write($"\nAnvändaren {username} är nu borttagen från systemet." +
                          $"\nTryck \"ENTER\" för att återgå till föregående meny.");
                        Console.ReadKey();
                        break;
                    }
                    else if (answer == "2")
                    {
                        Console.Write("\nDu har valt att avsluta processen." +
                                        "\nTryck \"ENTER\" för att återgå till föregående meny.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    }
                    else
                    {
                        Console.Write("\nFel input! Som säkerhetsåtgärd får du mata in lösenordet igen");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.Write("\nFel lösenord!" +
                                  "\nTryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                }
            }
        }

        // en metod som lägger till nya användare samt kolla om de finns redan
        public void CreateNewUser()
        {
            Console.Clear();
            string heading = "Du kommer nu att få skapa en ny användare genom att fylla i kundens" +
                           "\nförnamn, efternamn samt ange ett användarnamn och ett lösenord.\n";
            string username;
            string firstName = VerifyFirstName(heading);
            string lastName = VerifyLastName(heading, firstName);

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"{heading}" +
                                $"\nFörnamn: {firstName} " +
                                $"\nEfternamn: {lastName}");

                Console.Write("\nEtt nytt användarnamn skall nu anges. Användarnamnet får inte innehålla några" +
                      "\nspecialtecken (t.ex. %, &, !, @ etc.) och vara mellan 4-24 tecken långt.\n" +
                      "\nAnvändarnamn: ");
                username = Console.ReadLine();

                if (IsUsernameTaken(username))
                {
                    Console.Write($"\nEtt konto med användarnamnet {username} existerar redan. " +
                                    $"\nVar god välj ett nytt användarnamn.\n" +
                                    $"\nTryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                }
                else
                {
                    if (User.VerifyNewUsername(username))
                    {
                        break;
                    }
                    else
                    {
                        Console.Write("\nOgiltigt användarnamn. Användarnamnet kan endast vara mellan" +
                                      "\n4-24 tecken långt och får ej innehålla några specialtecken.\n" +
                                      "\nTryck \"ENTER\" och försök igen.");
                        Console.ReadKey();
                    }
                }
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"{heading}" +
                       $"\nFörnamn: {firstName} " +
                       $"\nEfternamn: {lastName}" +
                       $"\nAnvändarnamn: {username}");

                Console.Write("\nNu skall ett nytt lösenord anges. Lösenordet måste innehålla" +
                              "\nminst ett specialtecken (t.ex. %, &, !, @ etc.), minst" +
                              "\nen versal och en siffra samt vara mellan 6-30 tecken långt.\n" +
                              "\nLösenord: ");
                string password = Console.ReadLine();

                if (User.VerifyNewPassword(password))
                {
                    AddCustomer(new Customer(username, password, firstName, lastName));
                    break;
                }
                else
                {
                    Console.Write("\nOgiltigt lösenord. Lösenordet måste vara mellan 6-30 tecken långt" +
                                 "\noch innehålla minst en stor bokstav, en siffra och ett specialtecken.\n" +
                                "\nTryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                }
            }
            Console.Write($"\nBra jobbat! Användaren {username} finns nu inlagd i systemet.\n" +
                            $"\nTryck \"ENTER\" för att återgå till föregående meny.");
            Console.ReadKey();
        }

        public static string VerifyLastName(string heading, string firstName)
        {
            string verifiedLastName;

            while (true)
            {
                Console.Clear();
                Console.WriteLine(heading);
                Console.WriteLine($"Förnamn: {firstName}");

                Console.Write("Efternamn: ");
                string lastName = Console.ReadLine();

                if (lastName == "")
                {
                    Console.Write("\nFältet kan ej lämnas tomt. Var god fyll i ett efternamn." +
                                    "\nTryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    verifiedLastName = lastName.Trim();
                    break;
                }
            }
            return verifiedLastName;
        }

        public static string VerifyFirstName(string heading)
        {
            string verifiedFirstName;

            while (true)
            {
                Console.Clear();
                Console.WriteLine(heading);

                Console.Write("Förnamn: ");
                string firstName = Console.ReadLine();

                if (firstName == "")
                {
                    Console.Write("\nFältet kan ej lämnas tomt. Var god fyll i ett förnamn." +
                                    "\nTryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    verifiedFirstName = firstName.Trim();
                    break;
                }
            }
            return verifiedFirstName;
        }

        // Publik metod som lägger till en användare i listan.
        public void AddCustomer(Customer customer)
        {
            _customers.Add(customer);
        }

        // Här kan man ta bort användare från listan
        public void DeleteCustomer(Customer customer)
        {
            _customers.Remove(customer);
        }

        //Skapade en metod som visar alla användare
        public void PrintUsers()
        {
            Console.Clear();

            if (_customers.Count == 0)
            {
                Console.WriteLine("För tillfället finns inga användare i banken.\n");
            }
            else
            {
                Console.WriteLine("Lista över användare:" +
                          "\n---------------------");

                foreach (var user in _customers)
                {
                    if (user is Customer c)
                    {
                        if (c.IsBlocked)
                        {
                            Console.Write($"Namn: {c.FirstName} {c.LastName}" +
                                        $"\nAnvändarnamn: {c.Username}" +
                                        $"\nKontot är för närvarande spärrat.\n");
                        }
                        else
                        {
                            Console.Write($"Namn: {c.FirstName} {c.LastName}" +
                                        $"\nAnvändarnamn: {c.Username}\n");
                        }
                        Console.WriteLine();
                    }
                }
            }
        }

        // Metod för att ta bort spärren från en kund
        public void UnblockCustomer()
        {
            Console.Clear();
            bool isRunning = true;
            var blockedCustomers = _customers.FindAll(c => c.IsBlocked);

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
                        var customerToUnblock = _customers.Find(c => c.Username == username);
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
                         "\n---" +
                         "\nDitt val: ");
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

        // Skriver ut alla spärra kunder i listan
        public void PrintBlockedCustomers()
        {
            var blockedCustomers = _customers.FindAll(c => c.IsBlocked);

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

        // Metod för att spärra en kund
        public void BlockCustomer(User customer)
        {
            if (_customers.Contains(customer))
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
        public bool IsUsernameTaken(string username)
        {
            return _customers.Exists(user => user.Username == username);
        }

        // Metod som kollar om en användares användarnamn stämmer överens med lösenordet i listan
        public bool ConfirmUserLogin(string username, string password)
        {
            if (_admin.Username == username && _admin.CheckPassword(password))
            {
                return true;
            }

            if (_customers.Exists(customer => customer.Username == username && customer.CheckPassword(password)))
            {
                return true;
            }
            return false;
        }

        // Metod för att returnera en User, returnerar en "tom" användare för att programmet inte skall krascha
        public User GetUserByUsername(string username)
        {
            if (_admin.Username == username)
            {
                return _admin;
            }

            if (!UsernameExistsInList(username))
            {
                return new User("n/a", "n/a"); // returnerar en tom användare som inte kommer kunna logga in
            }

            return _customers.Find(user => user.Username == username);
        }

        // Metod för att kolla om ett användarnamn stämmer överens med ett av de användarnamnen i listan
        private bool UsernameExistsInList(string username)
        {
            return _customers.Exists(user => user.Username == username);
        }

        // Metod för att returnera ett objekt av Customer-klassen
        public Customer GetCustomer(string username)
        {
            return _customers.FirstOrDefault(c => c.Username == username);
        }

        public Admin GetAdmin()
        {
            return _admin;
        }
    }
}
