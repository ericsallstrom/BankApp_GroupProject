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

        public void NewAccount()
        {
            Account account = new Account();
            Console.WriteLine("Skapa nytt konto");


            UserAccounts.Add(account);
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

        //Metod för att ta ett lån med banken
        public void TakeLoan()
        {

        }


    }
}