namespace BankApp_GroupProject
{
    public class ConsoleIO
    {
        //int menuChoice;
        //private bool isRunning;
        private readonly LogInManager LogInManager = new();

        // Main menu
        public static void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine("Välkommen till NAMN PÅ BANK!" +
                            "\n=================" +
                            "\n[1] Logga in" +
                            "\n[2] Skapa ny användare" +
                            "\n-----------------" +
                            "\n[0] Avsluta programmet" +
                            "\n=================");

            Console.Write("Ditt val: ");
        }

        public void LogIn()
        {
            Console.Clear();

            // Run counter
            int loginCounter = 0;

            while (loginCounter < 3)
            {
                Console.WriteLine("Ange ditt användarnamn och lösenord för att logga in." +
                                "\nEfter tre misslyckade försök återgår du till huvudmenyn.\n");

                Console.Write("Användarnamn: ");
                string username = Console.ReadLine();

                Console.Write("Lösenord: ");
                string password = Console.ReadLine();

                bool loginSuccess = LogInManager.ConfirmUser(username, password);

                loginCounter++;

                if (loginSuccess)
                {
                    if (username == "admin")
                    {
                        break;
                    }
                    else
                    {
                        DisplayCustomerMenu();
                        break;
                    }
                }
                else
                {
                    if (loginCounter == 3)
                    {
                        Console.Write("\nFör många felaktiga försök har genomförts och" +
                                      "\ndu kommer snart att återgå till huvudmenyn.");
                        Thread.Sleep(4000);
                        break;
                    }
                    else
                    {
                        Console.Write("\nFel användarnamn eller lösenord! Försök igen" +
                                 "\nTryck \"ENTER\" och försök igen.");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }
        }

        // Customer menu
        public static void DisplayCustomerMenu()
        {
            Console.Clear();
            Console.WriteLine(
                  "NAMN PÅ BANK!" +
                "\n=================" +
                "\n[1] Föra över pengar" +
                "\n[2] Ta ut pengar" +
                "\n[3] Låna pengar av banken" +
                "\n[4] Öppna sparkonto" +
                "\n[5] Se tidigare transaktioner" +
                "\n[6] Visa konton" +
                "\n-----------------" +
                "\n[9] Konfigurera konto" +
                "\n[0] Logga ut" +
                "\n=================");

            Console.Write("Ditt val: ");
            string menuChoice = Console.ReadLine();

            switch (menuChoice)
            {
                case "1":
                    // Anropa metod för att föra över pengar!
                    break;
                case "2":
                    // Anropa metod för att ta ut pengar!
                    break;
                case "3":
                    // Anropa metod för att låna pengar!
                    break;
                case "4":
                    // Anropa metod för att öppna ett sparkonto!
                    break;
                case "5":
                    // Anropa metod för att se tidigare transaktioner!
                    break;
                case "6":
                    // Anropa metod för att visa kundens konton!
                    break;
                case "9":
                    // EN MENY DÄR ANVÄNDAREN KAN ÄNDRA LÖSENORD, KANSKE TA BORT SITT KONTO ETC.
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

        //public void DisplayAdminMenu()
        //{
        //    while (true)
        //    {
        //        Console.Clear();
        //        Console.WriteLine(
        //            "Admin Meny\r" +
        //            "\n=================\r" +
        //            "\n1. Skapa användare\r" +
        //            "\n2. Visa användare\r" +
        //            "\n3. Ta bort användare\r" +
        //            "\n4. Sätt växelkurs\r" +
        //            "\n5. Avsluta");

        //        menuChoice = GetMenuChoice("admin");
        //        Console.ReadKey();


        //        if (menuChoice == 1)
        //        {
        //            //CreateAccount();
        //        }

        //        if (menuChoice == 2)
        //        {
        //            //PrintAccounts();
        //        }

        //        if (menuChoice == 3)
        //        {
        //            //TakeLoan();
        //        }

        //        if (menuChoice == 4)
        //        {
        //            //SetCurrency();
        //        }

        //        if (menuChoice == 5)
        //        {
        //            break;
        //        }

        //    }
    }

    //Method for getting user menu choice that cathes wrong input.
    //public int GetMenuChoice(string userType)
    //{
    //    while (true)
    //    {

    //        Console.Write("\nDitt val:");

    //        if (int.TryParse(Console.ReadLine(), out menuChoice))
    //        {
    //            //If Admin menu is called
    //            if (userType == "admin")
    //            {
    //                if (menuChoice >= 1 && menuChoice <= 5)
    //                {
    //                    return menuChoice;
    //                }
    //            }

    //            //If Customer menu is called
    //            if (userType == "customer")
    //            {
    //                if (menuChoice >= 1 && menuChoice <= 4)
    //                {
    //                    return menuChoice;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            Console.WriteLine("Skriv in ett giltigt val.");
    //            Console.ReadKey();
    //        }
    //    }
}