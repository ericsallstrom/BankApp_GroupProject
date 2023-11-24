
namespace BankApp_GroupProject
{
    internal class ExchangeManager
    {
        // Static field, so that its method is reachable from
        // everywhere without the need to make new instances. 
        // That prohibits the default values from being altered.
        public static ExchangeManager exchange;

        // A Dictionary in charge of keeping the currencies, even when their being set by admin.        
        public Dictionary<string, decimal> Currencies { get; set; }

        readonly AsciiArt ascii = new();

        public ExchangeManager()
        {
            // When the constructor is called, a new Dictionary for the currencies is created.            
            Currencies = new Dictionary<string, decimal>()
            {
                // Default value for every currency.
                {"SEK", 1.0m },
                {"EUR", 11.45m },
                {"USD", 10.46m }
            };
        }

        // A property that checks if the object is empty. In 
        // that case a new instance of the class is created. 
        public static ExchangeManager Exchange
        {
            get
            {
                if (exchange == null)
                {
                    exchange = new ExchangeManager();
                }
                return exchange;
            }
        }

        // Allows the admin to enter new values for each foreign currency.
        public void SetCurrencies()
        {
            Console.Clear();
            Console.WriteLine(ascii.Header());
            string currentCurrencies = "Nuvarande växlingskurser" +
                                     "\n========================" +
                                    $"\nEUR:\t {Currencies["EUR"]}" +
                                    $"\nUSD:\t {Currencies["USD"]}";

            string setCurrencyEur = "\nSkriv in nytt värde för EUR: ";

            Currencies["EUR"] = GetUserDecimalInput(setCurrencyEur, currentCurrencies);

            string updatedCurrencies = "Växelkurser uppdaterade" +
                                "\n=======================" +
                               $"\nEUR:\t {Currencies["EUR"]}" +
                               $"\nUSD:\t {Currencies["USD"]}";

            string setCurrencyUsd = "\nSkriv in nytt värde för USD: ";

            Currencies["USD"] = GetUserDecimalInput(setCurrencyUsd, updatedCurrencies);

            Console.Clear();
            Console.WriteLine(ascii.Header());
            Console.WriteLine(updatedCurrencies);
            Console.Write("\nTryck \"ENTER\" för att återgå till föregående meny.");
            Console.ReadKey();
        }

        // Converts the value from the source currency to a the new value 
        // in the desired currency and returns the result as a decimal.
        public decimal CurrencyConverter(string sourceCurrency, string targetCurrency, decimal userAmount)
        {
            decimal sourceRate = Currencies[sourceCurrency];
            decimal targetRate = Currencies[targetCurrency];

            decimal result = Math.Round(userAmount * (sourceRate / targetRate), 2);

            return result;
        }

        // Converts the value from the source currency to a the new value 
        // in the desired currency and displays the summary for the user.
        public void CurrencyConvertSummary(string sourceCurrency, string targetCurrency, decimal userAmount)
        {
            decimal sourceRate = Currencies[sourceCurrency];
            decimal targetRate = Currencies[targetCurrency];

            decimal result = Math.Round(userAmount * (sourceRate / targetRate), 2);

            Console.WriteLine($"Konverterar {userAmount} {sourceCurrency} till {targetCurrency}.");
            Console.WriteLine($"Växlingskurs {sourceCurrency}: {sourceRate}");
            Console.WriteLine($"Resultat: {result}");
        }

        // Method that prompts the admin to enter a correct vale
        // while setting new values for the different currencies.
        public decimal GetUserDecimalInput(string question, string currentCurrencies)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ascii.Header());
                Console.WriteLine(currentCurrencies);
                Console.Write(question);
                string userInput = Console.ReadLine();

                // The user can only enter a new currency rate between 1-20.
                if (decimal.TryParse(userInput, out decimal newRate) && newRate >= 1 && newRate < 21)
                {
                    Console.Write($"\nVäxelkursen har ändrats. Tryck \"ENTER\" för att gå vidare.");
                    Console.ReadKey();
                    return newRate;
                }
                else
                {
                    Console.Write("\nOgiltig växelkurs! Växelkursen bör anges i siffror och " +
                                  "\nkan ej erhålla ett värde mindre än 1 och större än 20.\n" +
                                  "\nTryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                }
            }
        }
    }
}
