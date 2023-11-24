using System.Transactions;

namespace BankApp_GroupProject
{
    public enum AccountType
    {
        Checking,
        Savings,
        Global
    }

    public class Account
    {        
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public AccountType Type { get; set; }
        public Customer Customer { get; set; }
        public DateTime DateCreated { get; set; }
        public string Currency { get; set; }
        private List<Transaction> AccountHistory { get; set; }
        public static List<Account> AllCustomerAccounts { get; } = new List<Account>();
        public string CustomerName { get; set; }
        protected decimal _deposit;
        public decimal Debt { get; set; }
      
        AsciiArt ascii = new();

        public Account(AccountType type, Customer customer)
        {
            AccountNumber = GenerateAccountNumber();
            Balance = 0.0m;
            DateCreated = DateTime.Now;
            Currency = "SEK";
            _deposit = 0.0m;
            Type = type;
            Customer = customer;
            AccountHistory = new List<Transaction>();
            AllCustomerAccounts.Add(this);
            CustomerName = customer.FirstName + " " + customer.LastName;
        }

        protected static string GenerateAccountNumber()
        //Förslag på att implementera ett random kontonummer, kan uvecklas vidare
        //Tanken är att man behöver anropa metoden när man skapar ett nytt konto i en annan klass
        {
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

        public void Deposit(decimal amount)
        {

            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            Balance -= amount;
        }

        public string GetAccountType(Account account)
        {
            if (account.Type == AccountType.Checking)
            {
                return "Lönekonto";
            }
            else if (account.Type == AccountType.Savings)
            {
                return "Sparkonto";
            }
            else
            {
                return "Utlandskonto";
            }
        }

        public decimal MakeADeposit(Account account)
        {
            decimal deposit;

            while (true)
            {
                Console.Clear();
                Console.WriteLine(ascii.Header());
                Console.Write("Hur mycket vill du sätta in? Ange värdet i siffror." +
                            "\nInsättning: ");
                string userInput = Console.ReadLine();

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
                Console.Clear();
                Console.Write($"Accepterar du en insättning på {deposit} {Currency} till ditt {GetAccountType(account).ToLower()}?" +
                                "\n[1] JA" +
                                "\n[2] NEJ" +
                                "\n---" +
                                "\nDitt val: ");

                string userChoice = Console.ReadLine();

                if (int.TryParse(userChoice, out int choice) && (choice == 1 || choice == 2))
                {
                    if (choice == 1)
                    {
                        Deposit(deposit);
                        _deposit = deposit;
                        Console.Write($"\nInsättning av {deposit} {Currency} accepterad.\n" +
                                      $"\nTryck \"ENTER\" för att gå vidare.");
                        Transaction t1 = new(this, deposit, "Insättning", false);
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

        //Hämtar listan med transaktionshistoriken
        public List<Transaction> GetAccountHistory()
        {
            return AccountHistory;
        }

        public void PrintAccountHistory()
        {
            if (AccountHistory.Any() != true)
            {
                Console.WriteLine("\nDu har för närvarande inga genomförda kontohändelser.\n");
            }
            else
            {
                foreach (var item in AccountHistory)
                {
                    Console.WriteLine($"{item.SourceAcc}\t{item.SourceAccNumber}\t{item.TransactionAmount}" +
                        $"\t\t{item.TransactionType}\t{item.TransactionDate}");
                }
            }
        }
    }
}