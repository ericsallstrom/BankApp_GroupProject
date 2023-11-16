namespace BankApp_GroupProject
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Run();
        }

        // LOGIN METHOD
        static void Run()
        {
            bool runApp = true;
            ConsoleIO io = new();

            while (runApp)
            {                
                string userChoice = "";

                while (userChoice != "0")
                {
                    io.DisplayMainMenu();

                    userChoice = Console.ReadLine();

                    switch (userChoice)
                    {
                        case "1":
                            io.LogIn();
                            break;
                        case "0":
                            Console.Clear();
                            Console.WriteLine("\nDu har valt att avsluta...\n" +
                                    "\nVälkommen åter!");
                            runApp = false;
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
}
