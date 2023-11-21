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

        public Customer(string username, string password, string firstname, string lastname)
            : base(username, password)
        {
            FirstName = firstname;
            LastName = lastname;
            IsAdmin = false;
            IsBlocked = false;
            UserAccounts = new List<Account>();
        }   

        //Skapa Lönekontot
        public void NewCheckingAccount()
        {
            Account account = new();
            Console.WriteLine("Skapa nytt konto");
            
            Account checkingAccount = new();
            checkingAccount.AccType = "Lönekonto";

            UserAccounts.Add(checkingAccount);
            PrintAccountSuccess(checkingAccount);
            DoYouWantToDeposit(checkingAccount);
        }

        //skapa nytt sparkonto
        public void NewSavingsAccount()
        {

            SavingsAccount savingsAccount = new();
            Console.WriteLine("Skapa nytt sparkonto");
            Console.Clear();
            savingsAccount.InterestChoice();
            savingsAccount.AccType = "Sparkonto";
            UserAccounts.Add(savingsAccount);
            
            DoYouWantToDeposit(savingsAccount);
            PrintAccountSuccess(savingsAccount);
            savingsAccount.IsSavingsAccount = true; //kollar om savingsAccount
            Console.WriteLine();
            savingsAccount.CalcInterest();
            Console.WriteLine("tryck på valfri tangent för att gå tillbaka");
        }

        //Skapa Utlandskontot
        public void NewGlobalAccount()
        {
            Account globalAccount = new();
            globalAccount.AccType = "Utlandskonto";

            Console.WriteLine("Skapa nytt utlandskonto");
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
                        globalAccount.SetCurency("SEK");
                        break;
                    }
                    if (result == 2)
                    {
                        globalAccount.SetCurency("EUR");
                        break;
                    }
                    if (result == 3)
                    {
                        globalAccount.SetCurency("USD");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Ogiltigt val. Prova igen");
                    }
                }
                else
                {
                    Console.WriteLine("Ogiltigt val. Prova igen");
                }
            }

            UserAccounts.Add(globalAccount);
            PrintAccountSuccess(globalAccount);
            DoYouWantToDeposit(globalAccount);
        }
        
        //Frågar om man vill göra en deposit
        public void DoYouWantToDeposit(Account account)
        {
            Console.WriteLine("\n\nTryck [j] göra en insättning på kontot.");
            Console.WriteLine("Annars tryck valfri tangent för att gå tillbaka");

            string x = Console.ReadLine();
            if (x.ToLower() == "j")
            {
                account.MakeDeposit();
            }
        }
        //frågar om man vill göra en insättning till befintligt konto
        public void AccountDeposit(Account account)
        {
            Console.Clear();
            //visar konton       
            PrintAccounts(false); //false för att inte skriva ut tillbaka
        
            Console.WriteLine();
            Console.Write("Ange kontonummer för det konto du vill sätta in pengar på: ");
            string accountNumberToDeposit = Console.ReadLine().Trim();

            Account selectdAccount = UserAccounts.FirstOrDefault(account => account.AccountNumber.Trim() == accountNumberToDeposit);

            if (selectdAccount != null)
            {
                selectdAccount.MakeDeposit();
                PrintAccountSuccess(selectdAccount);
                Console.WriteLine("Tryck på valrfri tanget för att gå tillbaka");
                Console.ReadKey();
                
            }                 
            else
            {
                Console.WriteLine("Ogiltigt kontonummer. Tryck på valfri tangent för att gå tillbaka");
                Console.ReadKey();
            }

        }

        //Skriver ut Grattis och kontoöversikt
        public void PrintAccountSuccess(Account account)
        {
            Console.Clear();
            Console.WriteLine($"\n\nGrattis! Ett nytt {account.AccType} är skapat:" +
                $"\n\nKonto nr\tKontotyp\tSaldo\tValuta\tSkapat" +
                "\n****************************************************" +
                $"\n{account.AccountNumber}\t{account.AccType}\t{account.Balance}\t{account.Currency}\t{account.DateCreated}");
        }

        //Skriver ut kundens alla konton
        public void PrintAccounts(bool displayGoBackMessage =true)
        {
            Console.Clear();
            if (UserAccounts.Any() != true)
            {
                Console.WriteLine("Du har inga konton än.\n");

            } 
            else
            {
                Console.WriteLine($"Konto nr\tKontotyp\tSaldo\tValuta\tSkapat");
                Console.WriteLine("**************************************************");

                foreach (var item in UserAccounts)
                {
                    Console.WriteLine($"{item.AccountNumber}\t{item.AccType}\t{item.Balance}\t{item.Currency}\t{item.DateCreated}");
                }
            }
            if (displayGoBackMessage)
            {
                Console.WriteLine("\nTryck för att gå tillbaka");
                Console.ReadKey();
            }       
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
            // Using double so Math.Pow will work.
            // double that saves the total balance of the accounts.
            double totalBalance = 0;
            double interest = 0.0848;
            // double that will be set in the while loop. Total loan.
            double loanMoney = 0;
            // double that will be set in the while loop. Total loan time.
            double loanTime = 0;
            // double that will save a formula for total debt and monthly debt.
            double totalDebt = 0;
            double monthlyDebt = 0;

            // Clear console for design purpose.
            Console.Clear();
            Console.WriteLine("Lånaeavdelningen");
            Console.WriteLine("*****************************");
            Console.WriteLine("Välkommen till låneavdelningen. Vi erbjuder just nu annuitetslån till 8,48% ränta.\n");
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
                    if (item.Currency == "USD")
                    {
                        // KOD FÖR ATT KONVERTERA USD - SEK
                    }
                    else if (item.Currency == "EUR")
                    {
                        // KOD FÖR ATT KONVERTERA EUR - SEK
                    }
                    // Add all accounts balance to the totalBalance decimal.
                    totalBalance = totalBalance + Convert.ToDouble(item.Balance);
                }

            // while loop for exception handling while input loan 
            while (true)
            {
                // Print out total balance and how much customer can loan (max 5 times the amount of total balance). 
                Console.WriteLine("\nDitt totala saldo är " + totalBalance + "kr" +
                "\nDu kan låna max " + totalBalance * 5 + "kr" +
                "\n\nHur mycket vill du låna?");
                // User input.
                loanMoney = int.Parse(Console.ReadLine());
                // See if customer can loan that amount of money or not. Also make sure it isnt possible to type in negative number.
                if (loanMoney <= totalBalance * 5 && loanMoney > 0)
                {
                    Console.WriteLine("Du har valt att låna " + loanMoney + "kr. till 8,48% ränta.");
                    // While loop for exception handling when input loan time.
                    while (true)
                    {
                        Console.WriteLine("Hur lång avbetalningstid? (1-10 år)");
                        loanTime = int.Parse(Console.ReadLine());
                        if (loanTime <= 10)
                        {
                            double totalPayments = loanTime * 12;
                            double interestRate = interest / 12;
                            monthlyDebt = (loanMoney * interestRate) / (1 - Math.Pow(1 + interestRate, totalPayments * -1));
                            totalDebt = monthlyDebt * totalPayments;
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
                else
                {
                    Console.WriteLine("Du kan inte låna så mycket pengar. Vänligen försök med en lägre summa.");
                }

            }

            // HÄR SKA KOD FÖR ATT LÄGGA TILL LÅNADE PENGAR PÅ LÖNEKONTO SKRIVAS

            Console.WriteLine("Tryck för att gå tillbaka");
            Console.ReadKey();

        }    
    }
}