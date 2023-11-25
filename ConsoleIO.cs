namespace BankApp_GroupProject
{
    public class ConsoleIO
    {
        // Through the LogInManager-object the admin
        // is giving permission to create, remove 
        // and modify different customers.
        private readonly LogInManager LogInManager = new();

        // The user-object is storing the username
        // and password. Based on the username,
        // a specific user-object is chosen.
        private User user;

        // Based on which user that's logged in, 
        // one of the objects of Customer 
        // respectively Admin is activated.
        private Customer loggedInCustomer;
        private Admin loggedInAdmin;

        // With the ascii-object we can print out our logo.
        readonly AsciiArt ascii = new();

        // Main menu.
        public void DisplayMainMenu()
        {
            Console.Clear();

            // Prints out the bank-logo.
            Console.WriteLine(ascii.Header());

            Console.WriteLine("==================" +
                            "\n[1] Logga in" +
                            "\n------------------" +
                            "\n[0] Avsluta programmet" +
                            "\n==================");

            Console.Write("Ditt val: ");
        }

        // Method that's handling the logic when a user is trying to log in.
        public void LogIn()
        {
            int loginAttempts = 3;

            while (loginAttempts > 0)
            {
                Console.Clear();
                Console.WriteLine(ascii.Header());

                Console.WriteLine("Ange ditt användarnamn och lösenord för att logga in." +
                                "\nEfter tre misslyckade försök spärras ditt konto av säkerhetsskäl.\n");

                Console.Write("Användarnamn: ");
                string username = Console.ReadLine();

                Console.Write("Lösenord: ");
                string password = Console.ReadLine();

                // user is assigned either as admin och customer based on the entered username.
                // If the username is unknown to the system an empty user is being returned.
                user = LogInManager.GetUserByUsername(username);

                // Checks if the username and password is correct and returns a bool.
                bool loginSuccess = LogInManager.ConfirmUserLogin(username, password);

                // Making sure that both username and password is entered correctly 
                // and so that the user attempting to log in, is not already blocked.
                if (loginSuccess && !user.IsBlocked)
                {
                    if (user.IsAdmin)
                    {
                        // If the user logged in as admin, a specific admin-menu is presented.
                        loggedInAdmin = LogInManager.GetAdmin();
                        DisplayAdminMenu();
                        break;
                    }
                    else
                    {
                        // If the user logged in as a customer, a specific customer-menu is presented.
                        loggedInCustomer = LogInManager.GetCustomer(username);
                        DisplayCustomerMenu();
                        break;
                    }
                }
                // Checks whether a username is known to the system or not.
                else if (!LogInManager.UsernameExistsInList(username))
                {
                    Console.Write($"\nAnvändaren {username} är okänd. Kontrollera stavning." +
                                  $"\nKontakta vår administrativa avdelning om problemet kvarstår.\n" +
                                  $"\nTryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                }
                else
                {
                    // If the user is blocked the following message gets printed.
                    if (user.IsBlocked)
                    {
                        Console.Write("\nKontot är spärrat. Kontakta vår administrativa avdelning för ytterligare information.\n" +
                                                 "\nTryck \"ENTER\" för att återgå till huvudmenyn.");
                        Console.ReadKey();
                        break;
                    }

                    // Only if a user existing in the system fails 
                    // to log in, the loginAttempts is decremented.
                    loginAttempts = user.LogInAttempts--;

                    // After three failed login attempts the system checks if the user 
                    // is not a admin, then the user gets blocked. Otherwise the admin 
                    // is prompted to contact the banks administrative department. 
                    if (user.LogInAttempts == 0)
                    {
                        if (user.Username != "admin")
                        {
                            LogInManager.BlockCustomer(user);
                        }
                        else
                        {
                            Console.Write("\nFör många felaktiga försök har genomförts." +
                                          "\nDu saknar åtkomst för att utföra administrativa uppgifter.\n" +
                                          "\nTryck \"ENTER\" för att återgå till huvudmenyn.");
                            Console.ReadKey();
                        }
                    }
                    // For every failed attempt (up til three attempts) to log 
                    // in the following message is prompted to the known user.
                    else
                    {
                        Console.Write($"\nFel lösenord! {user.LogInAttempts} försök återstår." +
                                      "\nTryck \"ENTER\" och försök igen.");
                        Console.ReadKey();
                    }
                }
            }
        }

        // Customer menu, only visible for the customer.
        public void DisplayCustomerMenu()
        {
            string menuChoice = "";

            while (menuChoice != "0")
            {
                Console.Clear();
                Console.WriteLine(ascii.Header());

                Console.WriteLine($"\nVälkommen {loggedInCustomer.FirstName} {loggedInCustomer.LastName}" +
                                "\n==================" +
                                "\n[1] Visa konton" +
                                "\n[2] Öppna nytt bankkonto" +
                                "\n[3] Föra över pengar" +
                                "\n[4] Insättning av pengar" +
                                "\n[5] Låna pengar av banken" +
                                "\n[6] Se tidigare transaktioner" +
                                "\n------------------" +
                                "\n[9] Ändra lösenord" +
                                "\n[0] Logga ut" +
                                "\n==================");

                Console.Write("Ditt val: ");
                menuChoice = Console.ReadLine();

                switch (menuChoice)
                {
                    case "1":
                        // Calls the method for displaying the customers different accounts.
                        loggedInCustomer.PrintAccounts();
                        Console.Write("\nTryck \"ENTER\" för att återgå till föregående meny.");
                        Console.ReadKey();
                        break;
                    case "2":
                        // The customer is redirected to a menu for choosing what type of account to create.
                        DisplayAccountMenu();
                        break;
                    case "3":
                        // The customer is redirected to a menu where they can chose 
                        // if they want to make an internal or external transfer.
                        DisplayTransactionsMenu();
                        break;
                    case "4":
                        // Checks if the customer have any accounts. If so, the first 
                        // account of the customer is selected and the customer can continue 
                        // deposit money. Otherwise a error message is being displayed.
                        if (loggedInCustomer.CustomerAccounts.Any())
                        {
                            Account selectedAccount = loggedInCustomer.CustomerAccounts[0];
                            loggedInCustomer.AccountDeposit();
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine(ascii.Header());
                            Console.Write("Du har för närvarande inga konton att göra en insättning på." +
                                "\nTryck \"ENTER\" för att återgå till föregående meny.");
                            Console.ReadKey();
                        }
                        break;

                    case "5":
                        // Checks if the customer have any accounts. If so, the first account 
                        // of the customer is selected and the customer can continue asking 
                        // for a loan. Otherwise a error message is being displayed.
                        if (loggedInCustomer.CustomerAccounts.Any())
                        {
                            Account selectedAccount = loggedInCustomer.CustomerAccounts[0];
                            loggedInCustomer.TakeLoan(selectedAccount);
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine(ascii.Header());
                            Console.Write("Du har för närvarande inga konton att göra en insättning på." +
                                "\nTryck \"ENTER\" för att återgå till föregående meny.");
                            Console.ReadKey();
                        }
                        break;
                    case "6":
                        // Displays every transaction been made to and from the customer different accounts.
                        loggedInCustomer.PrintAllTransactions();
                        break;
                    case "9":
                        // Calls a method that allows the customer to change their password.
                        loggedInCustomer.ChangePassword();
                        break;
                    case "0":
                        // The customer is being logged out.
                        Console.Write("\nDu loggas nu ut. Ha en trevlig dag!\n" +
                                      "\nTryck \"ENTER\" för att återgå till huvudmenyn.");
                        Console.ReadKey();
                        DisplayMainMenu();
                        break;
                    default:
                        // Following message is being displayed if the user enters an invalid choice.
                        // This is a recurring event that happens for every menu that's being displayed.
                        Console.Write("\nOgiltigt menyval! Var god välj ett alternativ från menyn." +
                                        "\nTryck \"ENTER\" och försök igen.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }
        }

        // Menu for making different transactions.
        public void DisplayTransactionsMenu()
        {
            string menuChoice = "";

            while (menuChoice != "0")
            {
                Console.Clear();
                Console.WriteLine(ascii.Header());

                Console.WriteLine("==================" +
                                "\n[1] Överföra pengar mellan dina konton" +
                                "\n[2] Överföra pengar till ett annat konto" +
                                "\n------------------" +
                                "\n[0] Återgå till kontoöversikten" +
                                "\n==================");

                Console.Write("Ditt val: ");
                menuChoice = Console.ReadLine();

                switch (menuChoice)
                {
                    case "1":
                        // Calls a method that allows the customer to make an internal transaction.
                        loggedInCustomer.InternalTransaction();
                        break;
                    case "2":
                        // Calls a method that allows the customer to make an external transaction.
                        loggedInCustomer.ExternalTransaction(loggedInCustomer);
                        break;
                    case "0":
                        // Returns the customer to the their start menu
                        Console.Write("\nTryck \"ENTER\" för att återgå till kontoöversikten.");
                        Console.ReadKey();
                        break;
                    default:
                        Console.Write("\nOgiltigt menyval! Var god välj ett alternativ från menyn." +
                                        "\nTryck \"ENTER\" och försök igen.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }
        }

        // Menu for making a new account.
        public void DisplayAccountMenu()
        {
            string menuChoice = "";

            while (menuChoice != "0")
            {
                Console.Clear();
                Console.WriteLine(ascii.Header());

                Console.WriteLine("==================" +
                                "\n[1] Skapa nytt lönekonto" +
                                "\n[2] Skapa nytt sparkonto" +
                                "\n[3] Skapa nytt konto med en annan valuta" +
                                "\n------------------" +
                                "\n[0] Återgå till kontoöversikten" +
                                "\n==================");

                Console.Write("Ditt val: ");
                menuChoice = Console.ReadLine();

                switch (menuChoice)
                {
                    case "1":
                        // Calls a method for creating a new checking account.
                        loggedInCustomer.NewCheckingAccount(loggedInCustomer);
                        break;
                    case "2":
                        // Calls a method for creating a new savings account.
                        loggedInCustomer.NewSavingsAccount(loggedInCustomer);
                        break;
                    case "3":
                        // Calls a method for creating a new global account.
                        loggedInCustomer.NewGlobalAccount(loggedInCustomer);
                        break;
                    case "0":
                        Console.Write("\nTryck \"ENTER\" för att återgå till kontoöversikten.");
                        Console.ReadKey();
                        break;
                    default:
                        Console.Write("\nOgiltigt menyval! Var god välj ett alternativ från menyn." +
                                        "\nTryck \"ENTER\" och försök igen.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }
        }

        // Admin menu, only visible for the admin.
        public void DisplayAdminMenu()
        {
            string menuChoice = "";

            while (menuChoice != "0")
            {
                Console.Clear();
                Console.WriteLine(ascii.Header());

                Console.WriteLine("Du är inloggad som admin" +
                                "\n==================" +
                                "\n[1] Lägg till ny användare" +
                                "\n[2] Visa alla användare" +
                                "\n[3] Radera en användare" +
                                "\n[4] Återställ spärrade användare" +
                                "\n[5] Sätt växelkurs" +
                                "\n------------------" +
                                "\n[9] Ändra lösenord" +
                                "\n[0] Logga ut" +
                                "\n==================");

                Console.Write("Ditt val: ");
                menuChoice = Console.ReadLine();

                switch (menuChoice)
                {
                    case "1":
                        // Method for creating a new user.
                        LogInManager.CreateNewUser();
                        break;
                    case "2":
                        // Method that displays every user, even the ones that is blocked.
                        LogInManager.PrintUsers();
                        Console.Write("Tryck \"ENTER\" för att återgå till föregående meny.");
                        Console.ReadKey();
                        break;
                    case "3":
                        // Method for deleting a user. 
                        LogInManager.DeleteExistingCustomer();
                        break;
                    case "4":
                        // Method for remove the block for a specific customer.
                        LogInManager.UnblockCustomer();
                        break;
                    case "5":
                        // Method that allows the admin to set the different currencies.
                        ExchangeManager.Exchange.SetCurrencies();
                        break;
                    case "9":
                        // Method allowing admin to change its password.
                        loggedInAdmin.ChangePassword();
                        break;
                    case "0":
                        Console.Write("\nDu loggas nu ut. Tryck \"ENTER\" för att återgå till huvudmenyn.");
                        Console.ReadKey();
                        break;
                    default:
                        Console.Write("\nOgiltigt menyval! Var god välj ett alternativ från menyn." +
                                        "\nTryck \"ENTER\" och försök igen.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }
        }
    }
}