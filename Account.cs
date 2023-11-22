using System.Transactions;

namespace BankApp_GroupProject
{

    public class Account
    {
        //Lade till denna för att särskilja alla konton
        public string AccType { get; set; }
        public string AccountNumber { get; set; }
        public Decimal Balance { get; set; }
        public DateTime DateCreated { get; set; }
        public string Currency { get; set; }
        private List<Transaction> AccountHistory { get; set; }
        public decimal _Deposit { get; set; }
        public bool IsSavingsAccount { get; set; }

        public Account()
        {
            AccountNumber = GenerateAccountNumber();
            Balance = 0.0m;
            DateCreated = DateTime.Now;
            AccountHistory = new List<Transaction>();
            _Deposit = 0;
            Currency = "SEK";
        }
        protected string GenerateAccountNumber()
        {
            //Förslag på att implementera ett random kontonummer, kan uvecklas vidare
            //Tanken är att man behöver anropa metoden när man skapar ett nytt konto i en annan klass


            Random random = new();
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

        public decimal GetBalance()
        {            
            return Balance;
        }

        public void Deposit(decimal deposit)
        {
            Balance = deposit;
        }

        public void Withdraw(decimal sum)
        {
            Balance -= sum;
        }

        public virtual decimal MakeDeposit()
        {
            string accountType = IsSavingsAccount ? "sparkonto" : "bankkonto";
            decimal deposit;

            while (true)
            {
                Console.Clear();
                Console.Write("Hur mycket vill du sätta in? Ange värdet i siffror." +
                            "\nInsättning: ");
                string userInput = Console.ReadLine();
                Console.WriteLine();

                if (decimal.TryParse(userInput, out deposit) && deposit >= 1 && deposit <= 999999) //ändrat till decimal
                {
                    break;
                }
                else
                {
                    Console.Write("Ogiltig inmatning. Vänligen ange beloppet i siffror samt ett värde mellan 1-999999." +
                                "\nTryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                }
            }

            while (true)
            {
                Console.Write($"Accepterar du en insättning på {deposit:c} till ditt {accountType}?" +
                                "\n[1] JA" +
                                "\n[2] NEJ" +
                                "\n---" +
                                "\nDitt val: ");

                string userChoice = Console.ReadLine();

                if (int.TryParse(userChoice, out int choice) && (choice == 1 || choice == 2))
                {
                    if (choice == 1)
                    {
                        Balance += deposit;
                        _Deposit = deposit;
                        Console.Write($"\nInsättning av {deposit:c} accepterad." +
                                      $"\nTryck \"ENTER\" för att gå vidare.");
                        Console.ReadKey();
                        break;
                    }
                    else if (choice == 2)
                    {
                        Console.Write("\nInsättning avbruten." +
                                      "\nTryck \"ENTER\" för att återgå till föregående meny.");
                        Console.ReadKey();
                        return 0;
                    }
                }
                else
                {
                    Console.Write("\nOgiltigt val. Vänligen välj 1 för JA eller 2 för NEJ." +
                                  "\nTryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                    Console.Clear();
                }
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