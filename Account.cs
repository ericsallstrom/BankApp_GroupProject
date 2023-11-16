using System.Transactions;

namespace BankApp_GroupProject
{
    public class Account
    {
        //Ändrat Number till AccountNumber för att jag tycker att det passar sammanhanget bättre
        public string AccountNumber { get; set; }
        public Decimal Balance { get; set; }
        public DateTime DateCreated { get; set; }
        public string Currency { get; set; }
        public List<Transaction> AccountHistory { get; set; }
        //Dictionary som håller valutorna, även där vi sätter växelkurs
        public Dictionary<string, decimal> Currencies { get; set; }
        public int Deposit { get; set; }

        public Account()
        {
            AccountNumber = GenerateAccountNumber();
            Balance = 0.0m;
            DateCreated = DateTime.Now;
            AccountHistory = new List<Transaction>();
            Currencies = new Dictionary<string, decimal>();
            Deposit = 0;
        }
        protected string GenerateAccountNumber()
        {
            //Förslag på att implementera ett random kontonummer, kan uvecklas vidare
            //Tanken är att man behöver anropa metoden när man skapar ett nytt konto i en annan klass
           

            Random random = new Random();
            //use of const because its a set value and wont be changed in the future
            const string chars = "0123456789";
            //Generating random number based on chars of a maximum value of 10
            string generatedNumber = new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            //Adding a hypfen(-) after the forth number to simulate a realistic account number
            generatedNumber = generatedNumber.Insert(4, "-");

            return generatedNumber;
        }

        public void SetCurency(string currency)
        {
            Currency = currency;
        }

        //Lägger till valutorna med defaultvärden till dictionary.
        //OBS! Admin kanske vill ändra växelkurser innan ett konto är skapat?
        //Currencies kanske ska ha en egen klass som heter typ ExchangeManager?
        public void InitCurrencies()
        {
            Currencies.Add("SEK", 1.0m);
            Currencies.Add("EUR", 0.85m);
            Currencies.Add("USD", 0.75m);
        }

        //Här sätter admin nya värden för växelkursen.
        public void SetCurrencies()
        {
            //Skriv kod för admin för att sätta varje enskild växelkurs
            
            //Currencies["SEK"] = userInput;
            //Currencies["EUR"] = 0.85m;
            //Currencies["SEK"] = 0.75m;
        }


        public decimal GetBalance()
        {
            //Skriv kod för att hämta saldot
            return Balance;
          
        }
        public int MakeDeposit() //Metod för insättning av pengar

        {
            int deposit;
            
            Console.WriteLine("How much would you like to deposit?");
            string userInput = Console.ReadLine();

            if (int.TryParse(userInput, out deposit)&& deposit >=1 && deposit <= 999999)
             {
                Console.WriteLine($"Would you like to acceept your deposit of {deposit} to your bank account");
                Console.WriteLine("1:for Yes \n2: for No");

                string userChoiche = Console.ReadLine();

                if (int.TryParse(userChoiche, out int choice) && (choice == 1 || choice == 2))
                {
                    if (choice == 1)
                    {
                        Balance += deposit;
                        Deposit = deposit;
                        Console.WriteLine($"Deposit of {deposit} accepted. New balance: {Balance}");
                    }
                    else
                    {
                        Console.WriteLine("Deposit canceled");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter 1 for Yes or 2 for No");
                }                          
            }
            else
            {
                Console.WriteLine("Input not valid. Please enter a numbmer between 1-999999");
            }
            return deposit;
        }
        public List<Transaction> GetTransactions()
        {
            //Skriv kod för att hämta transaktionshistoriken
            return AccountHistory;
        }
    }
}