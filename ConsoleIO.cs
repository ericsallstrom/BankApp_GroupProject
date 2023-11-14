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
                    "Costumer Menu\r" +
                    "\n=================\r" +
                    "\n1. Skapa nytt konto\r" +
                    "\n2. Visa konton\r" +
                    "\n3. Låna\r" +
                    "\n4. Avsluta");

                menuChoice = GetMenuChoice("costumer");
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
                    "Admin Menu\r" +
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

        public int GetMenuChoice(string UserType)
        {
            while (true)
            {

                Console.Write("\nEnter choice:");

                if (int.TryParse(Console.ReadLine(), out menuChoice))
                {
                    if (menuChoice >= 1 && menuChoice <= 4)
                    {
                        return menuChoice;
                    }
                }
                else
                {
                    Console.WriteLine("Enter a valid choice.");
                    Console.ReadKey();
                }
            }
        }
    }
}