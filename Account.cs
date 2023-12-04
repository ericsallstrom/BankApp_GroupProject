using System;
using System.Transactions;

namespace BankApp_GroupProject
{
    // Public enum, that holds the types of the three 
    // different account types a customer can have.
    public enum AccountType
    {
        Checking,
        Savings,
        Global
    }

    public class Account
    {
        public string AccountNumber { get; private set; }
        public decimal Balance { get; private set; }
        public AccountType Type { get; }
        public Customer Customer { get; set; }
        public DateTime DateCreated { get; set; }
        public string Currency { get; set; }
        private List<Transaction> AccountHistory { get; set; }

        // Public static list of accounts that automatically stores every customers accounts upon creation.
        public static List<Account> AllCustomerAccounts { get; } = new List<Account>();
        public string CustomerName { get; set; }
        public decimal Debt { get; set; }

        protected decimal _deposit;

        readonly AsciiArt ascii = new();

        // Constructor for the Account-class. Takes a enum AccountType and a Customer as in-parameters.
        public Account(AccountType type, Customer customer)
        {
            // Every unique account is assigned a random generated account number.
            AccountNumber = GenerateAccountNumber();
            Balance = 0.0m;
            DateCreated = DateTime.Now;
            Currency = "SEK";
            _deposit = 0.0m;
            Type = type;
            Customer = customer;
            CustomerName = customer.FirstName + " " + customer.LastName;

            // A list of the accounts transactions is instantiated when a new account is created.
            AccountHistory = new List<Transaction>();

            // AllCustomerAccounts.list adds the current instance of the Account-class to its list. 
            AllCustomerAccounts.Add(this);            
        }

        // Private method for generating and returning a random account number to every account. 
        private static string GenerateAccountNumber()
        {
            Random random = new();

            // The keyword const is applied to the variabel chars, because
            // it is a set value and shouldn't be changed in the future.
            const string chars = "0123456789";

            // Generating a random number based on the variabel chars, of a maximum value of 10 characters.
            string generatedNumber = new(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            // Adding a hyphen (-) after the forth number to simulate a realistic account number.
            generatedNumber = generatedNumber.Insert(4, "-");

            return generatedNumber;
        }

        // Method for assigning a type of currency to the property Currency.
        public void SetCurrency(string currency)
        {
            Currency = currency;
        }

        // Method that returns the decimal value of a accounts balance.
        public decimal GetBalance()
        {
            return Balance;
        }

        // Method for making a deposit to a account.
        public void Deposit(decimal amount)
        {

            Balance += amount;
        }

        // Method to withdraw a certain amount from a account.
        public void Withdraw(decimal amount)
        {
            Balance -= amount;
        }

        // Method that checks if a account is of a certain type. 
        // Returns a string-value based on which type of account it is.
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

        // Method that allows a customer to make a deposit.
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

                // If the userInput is a correct value, and not smaller than 1 and larger than 9999999...
                if (decimal.TryParse(userInput, out deposit) && deposit >= 1 && deposit <= 999999)
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
                Console.WriteLine(ascii.Header());
              
                 // Asking the customer if they accept a deposit of a certain amount to be placed in a specific account.
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
                        // Adds the amount to the account balance.
                        Deposit(deposit);

                        // Assigns the amount to _deposit for further calculations.
                        _deposit = deposit;
                        Console.Write($"\nInsättning av {deposit} {Currency} accepterad.\n" +
                                      $"\nTryck \"ENTER\" för att gå vidare.");

                        // The transaction is then stored in the object t1.
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

        // Returns the list of transactions for a specific account.
        public List<Transaction> GetAccountHistory()
        {
            return AccountHistory;
        }

        // Method that prints out every transaction made in every account for a customer, if there are any.
        public void PrintAccountHistory()
        {
            if (AccountHistory.Any() != true)
            {
                Console.WriteLine($"\nInga kontohändelser finns på {GetAccountType(this)}t.\n");
            }
            else
            {
                foreach (var item in AccountHistory)
                {
                    string s = item.TransactionType;

                    Console.WriteLine($"{item.SourceAcc}\t" +
                        $"{item.SourceCurrency}\t" +
                        $"{ExchangeManager.Exchange.ConvertAmount(item.TransactionAmount)}\t\t" +
                        $"{item.TransactionType}{StringCheck(s)}" +
                        $"{ExchangeManager.Exchange.ConvertAmount(item.SourceAccBalance)}\t\t" +
                        $"{item.TransactionDate}");
                }
            }
        }

        //Method just for formatting the tabs in the printouts.
        //If the string is to short it will mess with the columns,
        //For example "Lån" in TransactionType.
        public string StringCheck(string s) 
        {
            if (s.Length < 9) { return "\t\t"; }
            else { return "\t";}
        }
    }
}