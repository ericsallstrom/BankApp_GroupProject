namespace BankApp_GroupProject
{
    public class ConsoleIO
    {
        int menuChoice; 

        public void DisplayMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(
                    "Customer Meny\r" +
                    "\n=================\r" +
                    "\n1. Skapa nytt konto\r" +
                    "\n2. Visa konton\r" +
                    "\n3. Låna\r" +
                    "\n4. Avsluta");

                menuChoice = GetMenuChoice("admin");
                Console.ReadKey();


                if (menuChoice == 1)
                {
                    //CreateAccount();
                }

                if (menuChoice == 2)
                {
                    //PrintAccounts();
                }

                if (menuChoice == 3)
                {
                    //TakeLoan();
                }

                if (menuChoice == 4)
                {
                    break;
                }
            }
        }

        public void DisplayAdminMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(
                    "Admin Meny\r" +
                    "\n=================\r" +
                    "\n1. Skapa användare\r" +
                    "\n2. Visa användare\r" +
                    "\n3. Ta bort användare\r" +
                    "\n4. Sätt växelkurs\r" +
                    "\n5. Avsluta");

                menuChoice = GetMenuChoice("admin");
                Console.ReadKey();


                if (menuChoice == 1)
                {
                    //CreateAccount();
                }

                if (menuChoice == 2)
                {
                    //PrintAccounts();
                }

                if (menuChoice == 3)
                {
                    //TakeLoan();
                }

                if (menuChoice == 4)
                {
                    //SetCurrency();
                }

                if (menuChoice == 5)
                {
                    break;
                }

            }
        }

        //Method for getting user menu choice that cathes wrong input.
        public int GetMenuChoice(string userType)
        {
            while (true)
            {

                Console.Write("\nDitt val:");

                if (int.TryParse(Console.ReadLine(), out menuChoice))
                {
                    //If Admin menu is called
                    if (userType == "admin")
                    {
                        if (menuChoice >= 1 && menuChoice <= 5)
                        {
                            return menuChoice;
                        }
                    }

                    //If Customer menu is called
                    if (userType == "customer")
                    {
                        if (menuChoice >= 1 && menuChoice <= 4)
                        {
                            return menuChoice;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Skriv in ett giltigt val.");
                    Console.ReadKey();
                }
            }
        }
    }
}