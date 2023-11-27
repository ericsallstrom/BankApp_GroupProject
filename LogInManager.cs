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
        // Private field that holds a list of customers.
        private readonly List<Customer> _customers;

        // Private field that holds a object from the Admin-class.
        private readonly Admin _admin;

        readonly AsciiArt ascii = new();

        // Constructor for the LogInManager. When the program starts the bank
        // already holds a number of users (one admin and seven customers).        
        public LogInManager()
        {
            // Creates a new admin upon execution with already set username and password.
            _admin = new Admin("admin", "Password1@");

            // Adds seven customers to the list upon execution with already set username, password and name.
            _customers = new List<Customer>()
                {
                    new("olov", "Hej123@", "Olov", "Olsson"),
                    new("olof", "Hej123@", "Olof", "Nordin"),
                    new("eric", "Hej123@", "Eric", "Sällström"),
                    new("patrik", "Hej123@", "Patrik", "Petterson"),
                    new("hany", "Hej123@", "Hany", "Alhabboby"),
                    new("hans", "Hej123@", "Hans", "Elofsson") { IsBlocked = true }, // This customer is blocked from start.
                    new("karin", "Hej123@", "Karin", "Andersson") { IsBlocked = true } // This customer is blocked from start.
                };

            // This method is called upon execution.
            InitUserAccounts();
        }

        // Method for assigning every customer a checking account and a balance with a random value.
        private void InitUserAccounts()
        {
            Random random = new();

            foreach (var customer in _customers)
            {
                decimal initialBalance = random.Next(100, 30001);
                Account checkingAccount = new(AccountType.Checking, customer);
                checkingAccount.Deposit(initialBalance);
                customer.CustomerAccounts.Add(checkingAccount);
            }
        }

        // Method that allows admin to delete an already existing customer.
        public void DeleteExistingCustomer()
        {
            string username;
            string heading = "VARNING! Denna åtgärd är ej reversibel." +
                           "\nFör att ta bort en användare från systemet behöver du mata in användarens " +
                           "\nanvändarnamn och sedan mata in ditt lösenord för att åtgärden skall fastställas.\n";
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ascii.Header());
                PrintUsers();

                Console.WriteLine(heading);
                Console.Write("Användarnamn: ");
                username = Console.ReadLine();

                // Checks if the customer is registered in the system.
                if (UsernameExistsInList(username))
                {
                    break;
                }
                else
                {
                    Console.Write("\nFelaktig inmatning! Användaren finns inte i systemet.\n" +
                                    "Tryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                }
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine(ascii.Header());
                PrintUsers();

                Console.Write($"{heading}" +
                              $"\nAnvändarnamn: {username}" +
                              $"\nAnge ditt lösenord: ");
                string adminPassword = Console.ReadLine();

                // To execute the process of deleting a customer, the admin has to enter its password.
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
                        // The customer is located in the list through its username and is then deleted.
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

        // Method that allows admin to create a new customer by entering the
        // customers full name, username and assigning it a password. Both
        // the username and password are being verified before they are being 
        // assigned to the new customer. Also, the username has to be unique.        
        public void CreateNewUser()
        {
            Console.Clear();
            Console.WriteLine(ascii.Header());
            string heading = "Du kommer nu att få skapa en ny användare genom att fylla i kundens" +
                           "\nförnamn, efternamn samt ange ett användarnamn och ett lösenord.\n";
            string username;
            string firstName = VerifyFirstName(heading);
            string lastName = VerifyLastName(heading, firstName);

            while (true)
            {
                Console.Clear();
                Console.WriteLine(ascii.Header());
                Console.WriteLine($"{heading}" +
                                $"\nFörnamn: {firstName} " +
                                $"\nEfternamn: {lastName}");

                Console.Write("\nEtt nytt användarnamn skall nu anges. Användarnamnet får inte innehålla några" +
                      "\nspecialtecken (t.ex. %, &, !, @ etc.) och vara mellan 4-24 tecken långt.\n" +
                      "\nAnvändarnamn: ");
                username = Console.ReadLine();

                // Checks if a username already exists in the system.
                if (IsUsernameTaken(username))
                {
                    Console.Write($"\nEtt konto med användarnamnet {username} existerar redan. " +
                                    $"\nVar god välj ett nytt användarnamn.\n" +
                                    $"\nTryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                }
                else
                {
                    // Verifies the new username.
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
                Console.WriteLine(ascii.Header());
                Console.WriteLine($"{heading}" +
                       $"\nFörnamn: {firstName} " +
                       $"\nEfternamn: {lastName}" +
                       $"\nAnvändarnamn: {username}");

                Console.Write("\nNu skall ett nytt lösenord anges. Lösenordet måste innehålla" +
                              "\nminst ett specialtecken (t.ex. %, &, !, @ etc.), minst" +
                              "\nen versal och en siffra samt vara mellan 6-30 tecken långt.\n" +
                              "\nLösenord: ");
                string password = Console.ReadLine();

                // Verifies the new password.
                if (User.VerifyNewPassword(password))
                {
                    // The new customer is then added to the list.
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

        // Method that ensures that a last name is being entered by the admin and returns the verified name.
        public static string VerifyLastName(string heading, string firstName)
        {
            string verifiedLastName;
            AsciiArt ascii = new();

            while (true)
            {
                Console.Clear();
                Console.WriteLine(ascii.Header());
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

        // Method that ensures that a first name is being entered by the admin and returns the verified name.
        public static string VerifyFirstName(string heading)
        {
            string verifiedFirstName;
            AsciiArt ascii = new();

            while (true)
            {
                Console.Clear();
                Console.WriteLine(ascii.Header());
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

        // Method for adding a new customer the list.
        private void AddCustomer(Customer customer)
        {
            _customers.Add(customer);
        }

        // Method for removing a customer from the list.
        private void DeleteCustomer(Customer customer)
        {
            _customers.Remove(customer);
        }

        // Prints every customer in the system out to console.
        public void PrintUsers()
        {
            Console.Clear();
            Console.WriteLine(ascii.Header());

            // Checking if there are any customers registered.
            if (_customers.Count == 0)
            {
                Console.WriteLine("För tillfället finns inga användare i banken.\n");
            }
            else
            {
                Console.WriteLine($"ANVÄNDARE\n" +
                    $"\nNamn\t\t\t\tAnvändarnamn\t\tKontostatus" +
                    $"\n===================================================================");

                foreach (var user in _customers)
                {
                    // Checking if a user is of type customer.
                    if (user is Customer c)
                    {
                        // For a nicer formatting, customers whose name is as long or longer than
                        // 15 characters have a different format when printing to console.
                        if (c.FirstName.Length + c.LastName.Length >= 15)
                        {
                            // If a customer is blocked, its marked with the word "Spärrat" (= blocked).
                            if (c.IsBlocked)
                            {
                                Console.Write($"{c.FirstName} {c.LastName}\t\t{c.Username}\t\t\tSpärrat\n");
                            }
                            else
                            {
                                Console.Write($"{c.FirstName} {c.LastName}\t\t{c.Username}\n");
                            }
                        }
                        else
                        {
                            if (c.IsBlocked)
                            {
                                Console.Write($"{c.FirstName} {c.LastName}\t\t\t{c.Username}\t\t\tSpärrat\n");
                            }
                            else
                            {
                                Console.Write($"{c.FirstName} {c.LastName}\t\t\t{c.Username}\n");
                            }
                        }
                    }
                }
                Console.WriteLine("\n");
            }
        }

        // Method for allowing admin to unblock a customer, which means 
        // that the customer in question now is able to log in again.
        public void UnblockCustomer()
        {
            Console.Clear();
            Console.WriteLine(ascii.Header());

            bool isRunning = true;

            // Every blocked customer in the _customer-list is stored in a new list, "blockedCustomers".
            var blockedCustomers = _customers.FindAll(c => c.IsBlocked);

            // If blockedCustomers-list is not empty.
            if (blockedCustomers.Count > 0)
            {
                while (isRunning)
                {
                    Console.Clear();
                    Console.WriteLine(ascii.Header());

                    // Displays only the blocked customers.
                    PrintBlockedCustomers();

                    Console.Write("\nSkriv in användarnamnet på den användare du önskar återställa från listan ovan." +
                                "\nHar du ändrat dig? Tryck \"ENTER\" och avsluta sedan processen i menyn.\n" +
                                "\nAnvändarnamn: ");
                    string username = Console.ReadLine();

                    // Checks whether the entered username corresponds with a customer in the list.
                    if (UsernameExistsInList(username))
                    {
                        // The customer to be unblocked are stored in a temporary Customer-object.
                        var customerToUnblock = _customers.Find(c => c.Username == username);

                        // The blocked customer is then unblocked.
                        customerToUnblock.Unblock();

                        Console.WriteLine($"\nAnvändaren {customerToUnblock.Username} är nu återställd.");
                        Console.Write("\nTryck \"ENTER\" för att återgå till föregående meny.");
                        Console.ReadKey();
                        isRunning = false;
                    }
                    else
                    {
                        bool tryAgain = false;
                        while (!tryAgain)
                        {
                            Console.Clear();
                            Console.WriteLine(ascii.Header());

                            // Admin is prompted to enter a valid username or terminate the process.
                            Console.Write($"Användaren finns inte registrerad i banken." +
                                           "\nVill du försöka igen?" +
                                           "\n[1] Ja" +
                                           "\n[2] Nej" +
                                           "\n---" +
                                           "\nDitt val: ");

                            string answer = Console.ReadLine();

                            switch (answer)
                            {
                                case "1":
                                    tryAgain = true;
                                    break;
                                case "2":
                                    Console.Write("\nDu har valt att avsluta processen. " +
                                        "\nTryck \"ENTER\" för att återgå till föregående meny.");
                                    Console.ReadKey();
                                    return;
                                default:
                                    Console.Write("\nOgiltigt menyval! Tryck \"ENTER\" och försök igen.");
                                    Console.ReadKey();
                                    break;
                            }
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

        // Method for printing out all the blocked customers.
        public void PrintBlockedCustomers()
        {
            var blockedCustomers = _customers.FindAll(c => c.IsBlocked);

            Console.WriteLine($"SPÄRRADE ANVÄNDARE\n" +
                   $"\nNamn\t\t\t\tAnvändarnamn\t\tKontostatus" +
                   $"\n===================================================================");

            foreach (var user in blockedCustomers)
            {
                if (user is Customer c)
                {
                    // For a nicer formatting, customers whose name is as long or longer than
                    // 15 characters have a different format when printing to console.
                    if (c.FirstName.Length + c.LastName.Length >= 15)
                    {
                        Console.Write($"{c.FirstName} {c.LastName}\t\t{c.Username}\t\t\tSpärrat\n");
                    }
                    else
                    {
                        Console.Write($"{c.FirstName} {c.LastName}\t\t\t{c.Username}\t\t\tSpärrat\n");
                    }
                }
            }
        }

        // This method is being used to block a customer, if the customer 
        // exists in the list and fails to log in, in three attempts.
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

        // This method checks whether a username is unique upon a new customer is being created.        
        public bool IsUsernameTaken(string username)
        {
            return _customers.Exists(user => user.Username == username);
        }

        // Method that checks if the entered username och password 
        // corresponds with their respectively value stored in each property.
        public bool ConfirmUserLogin(string username, string password)
        {
            // First we check if admin is trying to log in.
            if (_admin.Username == username && _admin.CheckPassword(password))
            {
                // Resets admins log in attempts.
                _admin.LogInAttempts = 3;
                return true;
            }

            // Then we check if the customer is registered (i.e. existing in the list).
            if (_customers.Exists(customer => customer.Username == username && customer.CheckPassword(password)))
            {
                // Resets admins log in attempts.
                var loggedInCustomer = _customers.Find(c => c.Username == username);
                loggedInCustomer.LogInAttempts = 3;
                return true;
            }
            return false;
        }

        // This method returns a User-object based on the passing argument (username). 
        // If the username doesn't correspond with either admin or a customer in 
        // the list, it returns an "empty" user so that the program don't crash.        
        public User GetUserByUsername(string username)
        {
            if (_admin.Username == username)
            {
                return _admin;
            }

            if (!UsernameExistsInList(username))
            {
                return new User("n/a", "n/a");
            }

            return _customers.Find(user => user.Username == username);
        }

        // This method checks whether a customers username exists in the _customer-list.       
        public bool UsernameExistsInList(string username)
        {
            return _customers.Exists(user => user.Username == username);
        }

        // This method returns the Customer-object from the _customer-list 
        // that corresponds with the username being passed as an argument.
        public Customer GetCustomer(string username)
        {
            return _customers.FirstOrDefault(c => c.Username == username);
        }

        // When a user successfully logs in as admin, this 
        // method returns the instance _admin to that user.
        public Admin GetAdmin()
        {
            return _admin;
        }
    }
}
