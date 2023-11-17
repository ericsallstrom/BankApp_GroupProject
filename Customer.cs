using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Transactions;

namespace BankApp_GroupProject
{
    public class Customer : User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<Account> UserAccounts { get; set; }

        public Customer()
        {
            FirstName = "";
            LastName = "";
        }

        public Customer(string username, string password, string firstname, string lastname)
            : base(username, password)
        {
            FirstName = firstname;
            LastName = lastname;

            UserAccounts = new List<Account>();
        }

        public void NewAccount()
        {
            Account account = new Account();
            Console.WriteLine("Skapa nytt konto");
            Console.WriteLine("Vänligen välj valuta:" +
                "\n[1] SEK" +
                "\n[2] EUR" +
                "\n[3] USD");


            while (true)
            {
                string userInput = Console.ReadLine();

                if (int.TryParse(userInput, out int result))
                {
                    if (result == 1)
                    {
                        account.SetCurency("SEK");
                        break;
                    }
                    if (result == 2)
                    {
                        account.SetCurency("EUR");
                        break;
                    }
                    if (result == 3)
                    {
                        account.SetCurency("USD");
                        break;
                    }
                    if (result == 0 || result > 3)
                    {
                        Console.WriteLine("Ogiltigt val. Prova igen");
                    }
                } else
                {
                    Console.WriteLine("Ogiltigt val. Prova igen");
                }
            }

            UserAccounts.Add(account);

            Console.Clear();
            Console.WriteLine($"\n\nGrattis! Ett nytt lönekonto är skapat:" +
                $"\n\nKonto nr\tSaldo\tValuta\tSkapat" +
                "\n****************************************************" +
                $"\n{account.AccountNumber}\t{account.Balance}\t{account.Currency}\t{account.DateCreated}" +
                "\n\nTryck för att gå tillbaka");
            Console.ReadKey();
        }

        //Skriver ut kundens alla konton
        public void PrintAccounts()
        {
            if (UserAccounts.Any() != true)
            {
                Console.WriteLine("Du har inga konton än.\n");

            } else 

            foreach (var item in UserAccounts)
            {
                Console.WriteLine($"Konto nr\tSaldo\tValuta\tSkapat");
                Console.WriteLine("**************************************************");
                Console.WriteLine($"{item.AccountNumber}\t{item.Balance}\t{item.Currency}\t{item.DateCreated}");
            }

            Console.WriteLine("Tryck för att gå tillbaka");
            Console.ReadKey();
        }

        //Metod för att göra överföringar mellan egna konton
        public void InternalTransaction()
        {

        }

        //Metod för att göra överföringar mellan kunders konton
        public void ExternalTransaction()
        {

        }

        // Method to loan money from the bank.
        public void TakeLoan()
        {
            // Decimal that saves the total balance of the accounts.
            decimal totalBalance = 0;
            decimal interest = 0.0848M;
            // Clear console for design purpose.
            Console.Clear();
            Console.WriteLine("Lånaeavdelningen");
            Console.WriteLine("*****************************");
            Console.WriteLine("Välkommen till låneavdelningen. Vi erbjuder just nu lån till 8,48% ränta.\n");
            // Check to see if the customer have an account.
            if (UserAccounts.Any() != true)
            {
                Console.WriteLine("Du har inga konton än.\n");
                Console.WriteLine("Tryck för att gå tillbaka");
                Console.ReadKey();
                return;
            }
            else
                // Print out all of customers accounts and balance
                foreach (var item in UserAccounts)
                {
                    Console.WriteLine($"Konto nr\tSaldo\tValuta\tSkapat");
                    Console.WriteLine("**************************************************");
                    Console.WriteLine($"{item.AccountNumber}\t{item.Balance}\t{item.Currency}");
                    // See if currency is foreign, and if so convert to SEK.
                    if(item.Currency == "USD")
                    {
                        // KOD FÖR ATT KONVERTERA USD - SEK
                    }
                    else if (item.Currency == "EUR")
                    {
                        // KOD FÖR ATT KONVERTERA EUR - SEK
                    }
                    // Add all accounts balance to the totalBalance decimal.
                    totalBalance = item.Balance++;
                }

            // decimal that will be set in the while loop.
            decimal loanMoney = 0;
            // while loop for exception handling while input loan 
            while (true)
            {
                // Print out total balance and how much customer can loan (max 5 times the amount of total balance). 
                Console.WriteLine("\nDitt totala saldo är " + totalBalance + "kr" +
                "\nDu kan låna max " + totalBalance * 5 + "kr" +
                "\nHur mycket vill du låna?");
                // User input.
                loanMoney = int.Parse(Console.ReadLine());
                int loanTime = 0;
                // Calculate total debt and monthly debt.
                decimal totalDebt = (loanMoney * interest * loanTime) + loanMoney;
                decimal monthlyDebt = (loanMoney * interest / 12) + (loanMoney / loanTime * 12);
                // See if customer can loan that amount of money or not.
                if (loanMoney > totalBalance * 5)
                {
                    Console.WriteLine("Du kan inte låna så mycket pengar. Vänligen försök med en lägre summa.");
                }
                else
                {
                    Console.WriteLine("Du har valt att låna " + loanMoney + "kr. till 8,48% ränta.");
                    // While loop for exception handling when input loan time.
                    while (true)
                    {
                        Console.WriteLine("Hur lång avbetalningstid? (1-10 år)");
                        loanTime = int.Parse(Console.ReadLine());
                        if (loanTime <= 10)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Du har angett ett felaktigt antal år, prova igen...");
                        }
                    }
                    Console.WriteLine("Din totala skuld är " + Math.Round(totalDebt, 2) + "kr och din månadskostnad kommer bli " + Math.Round(monthlyDebt, 2) + "kr.");
                    break;
                }

            }

            // HÄR SKA KOD FÖR ATT LÄGGA TILL LÅNADE PENGAR PÅ LÖNEKONTO SKRIVAS

            Console.WriteLine("Tryck för att gå tillbaka");
            Console.ReadKey();
            
        }
    }
}