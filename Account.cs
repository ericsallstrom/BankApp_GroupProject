using System.Transactions;

namespace BankApp_GroupProject
{
    public class Account
    {
        //Lade till denna för att särskilja alla konton
        public string AccType { get; set; }
        //Ändrat Number till AccountNumber för att jag tycker att det passar sammanhanget bättre
        public string AccountNumber { get; set; }
        public Decimal Balance { get; set; }
        public DateTime DateCreated { get; set; }
        public string Currency { get; set; }
        public List<Transaction> AccountHistory { get; set; }
        //Dictionary som håller valutorna, även där vi sätter växelkurs
        public Dictionary<string, decimal> Currencies { get; set; }
        public decimal Deposit { get; set; }

        public Account()
        {
            AccountNumber = GenerateAccountNumber();
            Balance = 0.0m;
            DateCreated = DateTime.Now;
            AccountHistory = new List<Transaction>();
            Currencies = new Dictionary<string, decimal>();
            Deposit = 0;
            Currency = "SEK";
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
        public bool IsSavingsAccount { get; set; }
        public virtual decimal MakeDeposit()

        {
            string accountType = IsSavingsAccount ? "sparkonto" : "bankkonto";
            decimal deposit;

            Console.Clear();
            Console.WriteLine("Hur mycket vill du sätta in?");
            string userInput = Console.ReadLine();

            if (decimal.TryParse(userInput, out deposit) && deposit >= 1 && deposit <= 999999) //ändrat till decimal
            {
                Console.WriteLine($"Accepterar du en insättning på {deposit} till ditt {accountType}");
                Console.WriteLine("1: för JA\n2: för NEJ");

                string userChoice = Console.ReadLine();

                if (int.TryParse(userChoice, out int choice) && (choice == 1 || choice == 2))
                {
                    if (choice == 1)
                    {
                        Balance += deposit;
                        Deposit = deposit;
                        Console.WriteLine($"Insättning av {deposit} accepterad. Nytt saldo: {Balance}");
                    }
                    else
                    {
                        Console.WriteLine("Insättning avbruten");
                        return 0;
                    }
                }
                else
                {
                    Console.WriteLine("Ogiltigt val. Vänligen välj 1 för JA eller 2 för NEJ");
                }
            }
            else
            {
                Console.WriteLine("Ogiltig inmatning. Vänligen ange ett nummer mellan 1-999999");
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