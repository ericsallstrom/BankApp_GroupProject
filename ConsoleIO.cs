namespace BankApp_GroupProject
{
    public class ConsoleIO
    {
        private readonly LogInManager LogInManager = new();
        private Customer inloggedCustomer;
        private Admin inloggedAdmin;
        private User user;

        AsciiArt ascii = new();
        // Main menu
        public void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine(ascii.Header());

            Console.WriteLine("=================" +
                            "\n[1] Logga in" +
                            "\n-----------------" +
                            "\n[0] Avsluta programmet" +
                            "\n=================");

            Console.Write("Ditt val: ");
        }

        public void LogIn()
        {
            int logginAttempts = 3;

            while (logginAttempts > 0)
            {
                Console.Clear();
                Console.WriteLine(ascii.Header());
                Console.WriteLine("Ange ditt användarnamn och lösenord för att logga in." +
                                "\nEfter tre misslyckade försök spärras ditt konto av säkerhetsskäl.\n");

                Console.Write("Användarnamn: ");
                string username = Console.ReadLine();

                Console.Write("Lösenord: ");
                string password = Console.ReadLine();

                user = LogInManager.GetUserByUsername(username);

                bool loginSuccess = LogInManager.ConfirmUserLogin(username, password);

                if (loginSuccess && !user.IsBlocked)
                {
                    if (user.IsAdmin)
                    {
                        inloggedAdmin = LogInManager.GetAdmin();                        
                        DisplayAdminMenu();
                        break;
                    }
                    else
                    {
                        inloggedCustomer = LogInManager.GetCustomer(username);
                        DisplayCustomerMenu();
                        break;
                    }
                }
                else
                {
                    if (user.IsBlocked)
                    {
                        Console.Write("\nKontot är spärrat. Kontakta vår administrativa avdelning för ytterligare information.\n" +
                                                 "\nTryck \"ENTER\" för att återgå till huvudmenyn.");
                        Console.ReadKey();
                        break;
                    }

                    logginAttempts--;

                    if (logginAttempts == 0)
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
                    else
                    {
                        Console.Write($"\nFel användarnamn eller lösenord! {logginAttempts} försök återstår." +
                                      "\nTryck \"ENTER\" och försök igen.");
                        Console.ReadKey();
                    }
                }
            }
        }

        // Customer menu
        public void DisplayCustomerMenu()
        {
            string menuChoice = "";

            while (menuChoice != "0")
            {
                Console.Clear();
                Console.WriteLine(ascii.Header());

                Console.WriteLine($"\nVälkommen {inloggedCustomer.FirstName} {inloggedCustomer.LastName}" +
                                "\n=================" +
                                "\n[1] Visa konton" +
                                "\n[2] Öppna nytt bankkonto" +
                                "\n[3] Föra över pengar" +
                                "\n[4] Insättning av pengar" + 
                                "\n[5] Låna pengar av banken" +
                                "\n[6] Se tidigare transaktioner" +
                                "\n-----------------" +
                                "\n[9] Ändra lösenord" +
                                "\n[0] Logga ut" +
                                "\n=================");

                Console.Write("Ditt val: ");
                menuChoice = Console.ReadLine();

                switch (menuChoice)
                {
                    case "1":
                        // Anropa metod för att visa kontosaldo!
                        // Anropa metod för att visa kundens konton!
                        inloggedCustomer.PrintAccounts();
                        Console.Write("Tryck \"ENTER\" för att återgå till föregående meny.");
                        Console.ReadKey();
                        break;
                    case "2":
                        // Går till en till meny där användaren kan välja att öppna sparkonto, skapa nytt bankkonto med eller utan en annan valuta.
                        DisplayAccountMenu();
                        break;
                    case "3":
                        DisplayTransactionsMenu();
                        // Anropa metod för att föra över pengar!                   
                        break;
                    case "4":
                        // Anropa metod för att sätta in pengar på ett konto!                    
                        // Skicka med ett konto från inloggad kundens konton
                        if (inloggedCustomer.UserAccounts.Any()) //kollar om det finns konton
                        {
                            Account selectedAccount = inloggedCustomer.UserAccounts[0]; // Du kan välja det första kontot som ett exempel
                            inloggedCustomer.AccountDeposit(selectedAccount);
                        }
                        else
                        {
                            Console.WriteLine("Du har inga konton att göra insättning på.");
                        }                        
                        break;

                    case "5":
                        // Anropa metod för att sätta in pengar på ett konto!                    
                        // Skicka med ett konto från inloggad kundens konton
                        if (inloggedCustomer.UserAccounts.Any()) //kollar om det finns konton
                        {
                            Account selectedAccount = inloggedCustomer.UserAccounts[0]; // Du kan välja det första kontot som ett exempel
                            inloggedCustomer.TakeLoan(selectedAccount);
                        }
                        else
                        {
                            Console.WriteLine("Du har inga konton att göra insättning på.");
                        }                        
                        break;
                    case "6":
                        // Anropa metod för att se tidigare transaktioner!
                        inloggedCustomer.PrintAllTransactions();
                        break;
                    case "7":
                        // Anropa metod för att visa kundens konton!
                        inloggedCustomer.PrintAccounts();
                        break;
                    case "9":                       
                        inloggedCustomer.ChangePassword();
                        break;
                    case "0":
                        Console.Write("\nDu loggas nu ut. Ha en trevlig dag!\n" +
                                      "\nTryck \"ENTER\" för att återgå till huvudmenyn.");
                        Console.ReadKey();
                        DisplayMainMenu();
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

        public void DisplayTransactionsMenu()
        {
            string menuChoice = "";

            while (menuChoice != "0")
            {
                Console.Clear();
                Console.WriteLine(ascii.Header());

                Console.WriteLine("===========================" +
                                "\n[1] Överföra pengar mellan dina konton" +
                                "\n[2] Överföra pengar till ett annat konto" +
                                "\n-----------------" +
                                "\n[0] Återgå till kontoöversikten" +
                                "\n=================");

                Console.Write("Ditt val: ");
                menuChoice = Console.ReadLine();

                switch (menuChoice)
                {
                    case "1":                        
                        inloggedCustomer.InternalTransaction();
                        break;
                    case "2":
                        // Anropa metod för att föra över pengar till ett annat, externt konto
                        inloggedCustomer.ExternalTransaction();
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

        public void DisplayAccountMenu()
        {
            string menuChoice = "";

            while (menuChoice != "0")
            {
                Console.Clear();
                Console.WriteLine(ascii.Header());

                Console.WriteLine("===========================" +
                                "\n[1] Skapa nytt lönekonto" +
                                "\n[2] Skapa nytt sparkonto" +
                                "\n[3] Skapa nytt konto med en annan valuta" +
                                "\n-----------------" +
                                "\n[0] Återgå till kontoöversikten" +
                                "\n=================");

                Console.Write("Ditt val: ");
                menuChoice = Console.ReadLine();

                switch (menuChoice)
                {
                    case "1":
                        // Anropa metod för att skapa nytt konto!
                        inloggedCustomer.NewCheckingAccount();                        
                        break;
                    case "2":
                        // Anropa metod för att öppna ett nytt sparkonto!
                        inloggedCustomer.NewSavingsAccount();                                           
                        break;
                    case "3":
                        // Anropa metod för att öppna ett nytt konto med en annan valuta!
                        inloggedCustomer.NewGlobalAccount();                        
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

        public void DisplayAdminMenu()
        {
            string menuChoice = "";

            while (menuChoice != "0")
            {
                Console.Clear();
                Console.WriteLine(ascii.Header());

                Console.WriteLine("Du är inloggad som admin" +
                                "\n===========================" +
                                "\n[1] Lägg till ny användare" +
                                "\n[2] Visa alla användare" +
                                "\n[3] Radera en användare" +
                                "\n[4] Återställ spärrade användare" +
                                "\n[5] Sätt växelkurs" +
                                "\n-----------------" +
                                "\n[9] Ändra lösenord" +
                                "\n[0] Logga ut" +
                                "\n=================");

                Console.Write("Ditt val: ");
                menuChoice = Console.ReadLine();

                switch (menuChoice)
                {
                    case "1":
                        LogInManager.CreateNewUser();
                        break;
                    case "2":
                        LogInManager.PrintUsers();
                        Console.Write("Tryck \"ENTER\" för att återgå till föregående meny.");
                        Console.ReadKey();
                        break;
                    case "3":
                        LogInManager.DeleteExistingCustomer();
                        break;
                    case "4":                  
                        LogInManager.UnblockCustomer();
                        break;
                    case "5":
                        // Sätt växelkurs
                        ExchangeManager.Exchange.SetCurrencies();
                        break;
                    case "9":
                        inloggedAdmin.ChangePassword();
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