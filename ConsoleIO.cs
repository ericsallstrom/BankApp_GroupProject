namespace BankApp_GroupProject
{
    public class ConsoleIO
    {
        private readonly LogInManager LogInManager = new();

        //Håller koll på vem som är inloggad
        private Customer customer;

        // Main menu
        public void DisplayMainMenu()
        {
            customer = new Customer();

            Console.Clear();
            Console.WriteLine("Välkommen till NAMN PÅ BANK!" +
                            "\n=================" +
                            "\n[1] Logga in" +
                            "\n-----------------" +
                            "\n[0] Avsluta programmet" +
                            "\n=================");

            Console.Write("Ditt val: ");
        }

        public void LogIn()
        {
            Console.Clear();

            // Run counter
            int loginCounter = 3;

            while (loginCounter > 0)
            {
                Console.WriteLine("Ange ditt användarnamn och lösenord för att logga in." +
                                "\nEfter tre misslyckade försök spärras ditt konto av säkerhetsskäl.\n");

                Console.Write("Användarnamn: ");
                string username = Console.ReadLine();

                if (LogInManager.IsBlocked(customer))
                {
                    Console.Write("\nKontot är spärrat! Kontakta vår administrativa avdelning för ytterligare information.\n" +
                                      "\nTryck \"ENTER\" för att återgå till huvudmenyn.");
                    Console.ReadKey();
                    break;
                }
                else
                {
                    Console.Write("Lösenord: ");
                    string password = Console.ReadLine();

                    bool loginSuccess = LogInManager.ConfirmUser(username, password);

                    loginCounter--;

                    if (loginSuccess)
                    {
                        if (username == "admin")
                        {
                            DisplayAdminMenu();
                            break;
                        }
                        else
                        {
                            //Kallar metod för att hämta användarnamn
                            customer = LogInManager.GetCustomerByUsername(username);
                            DisplayCustomerMenu();
                            break;
                        }
                    }
                    else
                    {
                        if (loginCounter == 0 && username != "admin")
                        {

                            Console.Write("\nFör många felaktiga försök har genomförts" +
                                          "\noch kontot kommer nu att spärras.\n" +
                                          "\nTryck \"ENTER\" för att återgå till huvudmenyn.");
                            Console.ReadKey();
                            LogInManager.BlockCustomer(customer);
                            break;
                        }
                        else if (loginCounter == 0 && username == "admin" )
                        {
                            Console.Write("\nFör många felaktiga försök har genomförts." +
                                          "\nDu saknar åtkomst för att utföra administrativa uppgifter.\n" +
                                          "\nTryck \"ENTER\" för att återgå till huvudmenyn.");
                            Console.ReadKey();
                            break;
                        }
                        else
                        {
                            Console.Write($"\nFel användarnamn eller lösenord! {loginCounter} försök återstår." +
                                     "\nTryck \"ENTER\" och försök igen.");
                            Console.ReadKey();
                            Console.Clear();
                        }
                    }
                }
            }
        }

        // Customer menu
        public void DisplayCustomerMenu()
        {
            Console.Clear();

            Console.WriteLine("NAMN PÅ BANK\n" +
                           $"\nVälkommen {customer.FirstName} {customer.LastName}" +
                            "\n=================" +
                            "\n[1] Visa konton" +
                            "\n[2] Öppna nytt bankkonto" +
                            "\n[3] Föra över pengar" +
                            "\n[4] Ta ut pengar" +
                            "\n[5] Låna pengar av banken" +
                            "\n[6] Se tidigare transaktioner" +
                            "\n-----------------" +
                            "\n[9] Ändra lösenord" +
                            "\n[0] Logga ut" +
                            "\n=================");

            Console.Write("Ditt val: ");
            string menuChoice = Console.ReadLine();

            switch (menuChoice)
            {
                case "1":
                    // Anropa metod för att visa kontosaldo!
                    // Anropa metod för att visa kundens konton!
                    customer.PrintAccounts();
                    DisplayCustomerMenu();
                    break;
                case "2":
                    // Går till en till meny där användaren kan välja att öppna sparkonto, skapa nytt bankkonto med eller utan en annan valuta.
                    DisplayAccountMenu();
                    break;
                case "3":
                    // Anropa metod för att föra över pengar!
                    break;
                case "4":
                    // Anropa metod för att ta ut pengar!
                    break;
                case "5":
                    // Anropa metod för att låna pengar!
                    break;
                case "6":
                    // Anropa metod för att se tidigare transaktioner!
                    break;
                case "7":
                    // Anropa metod för att visa kundens konton!
                    customer.PrintAccounts();
                    break;
                case "9":
                    // Anropa metod för att ändra lösenord!
                    break;
                case "0":
                    Console.Write("\nDu loggas nu ut och återgår snart till huvudmenyn." +
                                  "\nHa en trevlig dag!");
                    Thread.Sleep(4000);
                    break;
                default:
                    Console.WriteLine("Ogiltigt menyval! Var god välj ett alternativ från menyn." +
                                    "\nTryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                    Console.Clear();
                    break;
            }
        }

        public void DisplayAccountMenu()
        {
            Console.Clear();

            Console.WriteLine("NAMN PÅ BANK\n" +
                            "\n=================" +
                            "\n[1] Skapa nytt lönekonto" +
                            "\n[2] Skapa nytt sparkonto" +
                            "\n[3] Skapa nytt konto med en annan valuta" +
                            "\n-----------------" +
                            "\n[0] Återgå till kontoöversikten" +
                            "\n=================");

            Console.Write("Ditt val: ");
            string menuChoice = Console.ReadLine();

            switch (menuChoice)
            {
                case "1":
                    // Anropa metod för att skapa nytt konto!
                    customer.NewAccount();
                    break;
                case "2":
                    // Anropa metod för att öppna ett nytt sparkonto!
                    break;
                case "3":
                    // Anropa metod för att öppna ett nytt konto med en annan valuta!
                    break;
                case "0":
                    Console.Write("\nDu återgår nu till kontoöversikten.");
                    Thread.Sleep(4000);
                    DisplayCustomerMenu();
                    break;
                default:
                    Console.WriteLine("Ogiltigt menyval! Var god välj ett alternativ från menyn." +
                                    "\nTryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                    Console.Clear();
                    break;
            }
        }

        public void DisplayAdminMenu()
        {
            Console.Clear();

            Console.WriteLine("NAMN PÅ BANK\n" +
                            "\nDu är inloggad som admin" +
                            "\n=================" +
                            "\n[1] Lägg till ny användare" +
                            "\n[2] Visa alla användare" +
                            "\n[3] Radera användare" +
                            "\n[4] Återställ spärrade användare" +
                            "\n[5] Sätt växelkurs" +
                            "\n-----------------" +
                            "\n[9] Ändra lösenord" +
                            "\n[0] Logga ut" +
                            "\n=================");

            Console.Write("Ditt val: ");
            string menuChoice = Console.ReadLine();

            switch (menuChoice)
            {
                case "1":
                    //LogInManager.AddUser();
                    break;
                case "2":
                    //LogInManager.PrintUsers();
                    break;
                case "3":
                    //LogInManager.DeleteUser();
                    break;
                case "4":
                    // Återställ spärrad användare                    
                    LogInManager.UnblockCustomer();
                    DisplayAdminMenu();
                    break;
                case "5":
                    // Sätt växelkurs
                    break;
                case "9":
                    break;
                case "0":
                    Console.Write("\nDu loggas nu ut och återgår snart till huvudmenyn.");
                    Thread.Sleep(4000);
                    break;
                default:
                    Console.WriteLine("Ogiltigt menyval! Var god välj ett alternativ från menyn." +
                                    "\nTryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                    Console.Clear();
                    break;
            }
        }
    }
}