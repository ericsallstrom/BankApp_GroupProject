using System.Security.Cryptography.X509Certificates;
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

        //Skapa Lönekontot
        public void NewCheckingAccount()
        {
            Account checkingAccount = new Account();
            checkingAccount.AccType = "Lönekonto";

            UserAccounts.Add(checkingAccount);
            PrintAccountSuccess(checkingAccount);
            DoYouWantToDeposit(checkingAccount);
        }

        //Skapa Utlandskontot
        public void NewGlobalAccount()
        {
            Account globalAccount = new Account();
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
                    if (result == 0 || result > 3)
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
            Console.WriteLine("\nTryck [j] göra en insättning på kontot.");
            Console.WriteLine("Annars tryck valfri tangent för att gå tillbaka");

            string x = Console.ReadLine();
            if (x.ToLower() == "j")
            {
                account.MakeDeposit();
            }
        }

        //Skriver ut Grattis och kontoöversikt
        public void PrintAccountSuccess(Account account)
        {
            Console.Clear();
            Console.WriteLine($"\n\nGrattis! Ett nytt {account.AccType} är skapat:" +
                $"\n\nKonto nr\tSaldo\tValuta\tSkapat" +
                "\n****************************************************" +
                $"\n{account.AccountNumber}\t{account.Balance}\t{account.Currency}\t{account.DateCreated}");
        }

        //Skriver ut kundens alla konton
        public void PrintAccounts()
        {
            if (UserAccounts.Any() != true)
            {
                Console.WriteLine("Du har inga konton än.\n");

            } else

            Console.WriteLine($"Konto nr\tSaldo\tValuta\tSkapat");
            Console.WriteLine("**************************************************");

            foreach (var item in UserAccounts)
            {
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

        //Metod för att ta ett lån med banken
        public void TakeLoan()
        {

        }


    }
}