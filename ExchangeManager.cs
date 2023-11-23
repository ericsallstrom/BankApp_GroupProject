
namespace BankApp_GroupProject
{
    internal class ExchangeManager
    {
        //Statisk så vi kommer åt metoderna överallt utan att behöva göra nya instanser.
        //Det i sin tur skyddar default värdena från att bli överskrivna.
        public static ExchangeManager _exchange;

        //Dictionary som håller valutorna, även där vi sätter växelkurs.
        public Dictionary<string, decimal> Currencies { get; set; }

        //Constructorn som bara körs en gång.
        public ExchangeManager()
        {
            //Skapar en ny dictionary av valutor med defaultvärden för växelkurserna.
            Currencies = new Dictionary<string, decimal>()
            {
                {"SEK", 1.0m },
                {"EUR", 11.45m },
                {"USD", 10.46m }
            };
        }

        //Property som kollar ifall objektet är tomt och isåfall skapar en instans.
        //Fast denna är främst för att slippa göra nya instanser av ExchangeManager.
        public static ExchangeManager Exchange
        {
            get
            {
                if (_exchange == null)
                {
                    _exchange = new ExchangeManager();
                }
                return _exchange;
            }
        }

        public void SetCurrencies()
        {
            Console.Clear();
            string currentCurrencies = "Nuvarande växlingskurser" +
                                     "\n=========================" +
                                    $"\nEUR:\t {Currencies["EUR"]}" +
                                    $"\nUSD:\t {Currencies["USD"]}";
            string setCurrencyEur = "\nSkriv in nytt värde för EUR: ";
            string setCurrencyUsd = "\nSkriv in nytt värde för USD: ";

            Currencies["EUR"] = GetUserDecimalInput(setCurrencyEur, currentCurrencies);

            Currencies["USD"] = GetUserDecimalInput(setCurrencyUsd, currentCurrencies);

            Console.Clear();
            Console.WriteLine("Växelkurser uppdaterade" +
                            "\n=========================" +
                           $"\nEUR:\t {Currencies["EUR"]}" +
                           $"\nUSD:\t {Currencies["USD"]}");

            Console.Write("\nTryck \"ENTER\" för att återgå till föregående meny.");
            Console.ReadKey();
        }

        //Valutakonvertering med return 
        public decimal CurrencyConverter(string sourceCurrency, string targetCurrency, decimal userAmount)
        {
            decimal sourceRate = Currencies[sourceCurrency];
            decimal targetRate = Currencies[targetCurrency];

            //Uträkningen avrundad till 2 decimaler
            decimal result = Math.Round(userAmount * (sourceRate / targetRate), 2);

            return result;
        }

        //Valutakonvertering med sumering
        public void CurrencyConvertSummary(string sourceCurrency, string targetCurrency, decimal userAmount)
        {
            decimal sourceRate = Currencies[sourceCurrency];
            decimal targetRate = Currencies[targetCurrency];

            //Uträkningen avrundad till 2 decimaler
            decimal result = Math.Round(userAmount * (sourceRate / targetRate), 2);

            //Skiver ut en sammanfattning av konverteringen
            Console.WriteLine($"Konverterar {userAmount} {sourceCurrency} till {targetCurrency}.");
            Console.WriteLine($"Växlingskurs {sourceCurrency}: {sourceRate}");
            Console.WriteLine($"Resultat: {result}");
        }

        public decimal GetUserDecimalInput(string question, string currentCurrencies)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(currentCurrencies);
                Console.Write(question);
                string userInput = Console.ReadLine();

                if (decimal.TryParse(userInput, out decimal newRate) && newRate > 1.0m && newRate < 20.0m)
                {
                    return newRate;
                }
                else
                {
                    Console.Write("\nOgiltig växelkurs! Ange växelkursen endast i siffror." +
                                  "\nTryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                }
            }
        }
    }
}
