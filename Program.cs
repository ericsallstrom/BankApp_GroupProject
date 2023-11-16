namespace BankApp_GroupProject
{
    internal class Program
    {

        static void Main(string[] args)
        {
            //Byta namn??
            Login();
        }

        // LOGIN METHOD
        static void Login()
        {
            LogInManager logInManager = new LogInManager();
            ConsoleIO io = new ConsoleIO();

            // Login counter
            int loginCounter = 0;

            while (loginCounter < 3)
            {
                // LÄGG TILL FELHANTERING!!!
                Console.Clear();
                Console.Write("Vänligen ange användarnamn: ");
                string username = Console.ReadLine();
                Console.Write("Vänligen ange lösenord: ");
                string password = Console.ReadLine();

                bool success = logInManager.Login(username, password);

                if (success)
                {
                    if (username == "admin")
                    {
                        // Go to admin menu method.
                        io.DisplayAdminMenu();
                        // LA TILL DENNA BREAK DÅ VI HAMNADE TILLBAKA I LOOPEN EFTER ATT VI GÅTT UR MENYERNA ANNARS. KOMMER SÄKERT ÄNDRAS SENARE
                        break;
                    }
                    else
                    {
                        // Go to customer menu method.
                        io.DisplayMenu();
                        // LA TILL DENNA BREAK DÅ VI HAMNADE TILLBAKA I LOOPEN EFTER ATT VI GÅTT UR MENYERNA ANNARS. KOMMER SÄKERT ÄNDRAS SENARE
                        break;
                    }
                }
                else
                {
                    loginCounter++;
                    if (loginCounter < 3)
                    {
                        Console.WriteLine("Fel användarnamn eller lösenord, försök igen...");
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
    }
}