namespace BankApp_GroupProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Run();
        }

        // Method to run the program.
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
                            Console.Write("\nDu har valt att avsluta...");
                            Thread.Sleep(2000);
                            Console.Write("\nVälkommen åter!");
                            Thread.Sleep(1500);
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
