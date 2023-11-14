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
        public List<string> Currencies { get; set; }

        public Account()
        {
            AccountNumber = GenerateAccountNumber();
            Balance = 0.0m;
            DateCreated = DateTime.Now;
            AccountHistory = new List<Transaction>();
            Currencies = new List<String>();
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
        public void SetCurency(string currency, Admin admin)
        {
            //kod ska implementeras
        }
        public decimal GetBalance()
        {
            //Skriv kod för att hämta saldot
            return Balance;
          
        }
        public List<Transaction> GetTransactions()
        {
            //Skriv kod för att hämta transaktionshistoriken
            return AccountHistory;
        }
    }
}