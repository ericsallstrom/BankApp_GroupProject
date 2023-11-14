namespace BankApp_GroupProject
{
    internal class Program
    {

        static void Main(string[] args)
        {
            //Byta namn??
            Login();
        }

        static void Login()
        {
            ConsoleIO io = new ConsoleIO();

            // LOGIN METHOD
            LogInManager logInManager = new LogInManager();
            // Login counter
            int loginCounter = 0;
            
            while (loginCounter <3)
            {
                // LÄGG TILL FELHANTERING!!!
                Console.Clear();
                Console.WriteLine("Vänligen ange användarnamn:");
                string username = Console.ReadLine();
                Console.WriteLine("Vänligen ange lösenord");
                string password = Console.ReadLine();

                bool success = logInManager.Login(username, password);

                if (success)
                {
                    // Go to menu method.
                    io.DisplayMenu();
                    // LA TILL DENNA BREAK DÅ VI HAMNADE TILLBAKA I LOOPEN EFTER ATT VI GÅTT UR Menu() ANNARS. KOMMER SÄKERT ÄNDRAS SENARE
                    break;
                }
                else
                {
                    loginCounter++;
                    if (loginCounter < 3)
                    {
                        Console.WriteLine("Fel användarnamn eller lösernord, försök igen...");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("Du har gjort för många felaktiga försök. Programmet avslutas...");
                        Console.ReadLine();
                    }
                }
            }
            
        }
        // LÄGG TILL MENYALTERNATIV
        static void Menu()
        {
            Console.WriteLine("Vänligen gör ett val");
            Console.WriteLine("1. ");
            Console.WriteLine("2. ");
            Console.WriteLine("3. ");

            Console.ReadLine();
        }
    }
}