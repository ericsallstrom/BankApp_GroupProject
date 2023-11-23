using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Threading.Channels;
using System.Transactions;

namespace BankApp_GroupProject
{
    public class Customer : User
    {
        private readonly Account _checkingAccount = new() { AccType = "Lönekonto" };
        private readonly Account _globalAccount = new() { AccType = "Utlandskonto" };
        private readonly SavingsAccount _savingsAccount = new() { AccType = "Sparkonto" };

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

        public Account GetCheckingAccount()
        {
            return _checkingAccount;
        }

        //Skapa Lönekontot
        public void NewCheckingAccount()
        {
            Console.Clear();

            if (!UserAccounts.Contains(_checkingAccount))
            {
                int answer = ProceedCreatingAccount(_checkingAccount);

                if (answer == 1)
                {
                    UserAccounts.Add(_checkingAccount);
                    PrintAccountSuccess(_checkingAccount);
                    DoYouWantToDeposit(_checkingAccount);
                }
                else
                {
                    Console.Write("\nDu har valt att avbryta processen! Tryck \"ENTER\" för att återgå till föregående meny.");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.Write($"Du har redan ett {_checkingAccount.AccType.ToLower()}!\n" +
                              "\nTryck \"ENTER\" för att återgå till föregående meny.");
                Console.ReadKey();
            }
        }

        //skapa nytt sparkonto
        public void NewSavingsAccount()
        {
            Console.Clear();

            if (!UserAccounts.Contains(_savingsAccount))
            {
                int answer = ProceedCreatingAccount(_savingsAccount);

                if (answer == 1)
                {
                    _savingsAccount.InterestChoice();
                    UserAccounts.Add(_savingsAccount);

                    DoYouWantToDeposit(_savingsAccount);
                    PrintAccountSuccess(_savingsAccount);

                    _savingsAccount.IsSavingsAccount = true; //kollar om _savingsAccount            
                    _savingsAccount.CalcInterest();
                }
                else
                {
                    Console.Write("\nDu har valt att avbryta processen! Tryck \"ENTER\" för att återgå till föregående meny.");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.Write($"Du har redan ett {_checkingAccount.AccType.ToLower()}!\n" +
                              "\nTryck \"ENTER\" för att återgå till föregående meny.");
                Console.ReadKey();
            }
        }

        //Skapa Utlandskontot
        public void NewGlobalAccount()
        {
            bool currencySet = false;
            Console.Clear();

            if (!UserAccounts.Contains(_globalAccount))
            {
                int answer = ProceedCreatingAccount(_globalAccount);

                if (answer == 1)
                {
                    while (!currencySet)
                    {
                        Console.Clear();
                        Console.Write("För att skapa ett utlandskonto måste du välja vilken valuta kontots värde skall stå i.\n" +
                                    "Vänligen välj valuta:" +
                                    "\n[1] EUR" +
                                    "\n[2] USD" +
                                    "\n---" +
                                    "\nDitt val: ");

                        string menuChoice = Console.ReadLine();

                        switch (menuChoice)
                        {
                            case "1":
                                _globalAccount.SetCurency("EUR");
                                currencySet = true;
                                break;
                            case "2":
                                _globalAccount.SetCurency("USD");
                                currencySet = true;
                                break;
                            default:
                                Console.Write("\nOgiltigt val! Tryck \"ENTER\" och försök igen.");
                                Console.ReadKey();
                                break;
                        }
                    }
                    UserAccounts.Add(_globalAccount);
                    PrintAccountSuccess(_globalAccount);
                    DoYouWantToDeposit(_globalAccount);
                }
                else
                {
                    Console.Write("\nDu har valt att avbryta processen! Tryck \"ENTER\" för att återgå till föregående meny.");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.Write($"Du har redan ett {_checkingAccount.AccType.ToLower()}!\n" +
                              "\nTryck \"ENTER\" för att återgå till föregående meny.");
                Console.ReadKey();
            }
        }

        private int ProceedCreatingAccount(Account account)
        {
            string menuChoice = "";

            while (menuChoice != "2")
            {
                Console.Clear();
                Console.Write($"Vill du skapa ett nytt {account.AccType.ToLower()}?" +
                                $"\n[1] JA" +
                                $"\n[2] NEJ" +
                                $"\n---" +
                                $"\nDitt val: ");

                menuChoice = Console.ReadLine();

                switch (menuChoice)
                {
                    case "1":
                        return 1;
                    case "2":
                        break;
                    default:
                        Console.Write("\nOgiltigt val! Tryck \"ENTER\" och försök igen.");
                        Console.ReadKey();
                        break;
                }
            }
            return 0;
        }

        //Frågar om man vill göra en deposit
        public void DoYouWantToDeposit(Account account)
        {
            Console.Write("\nTryck [j] för att göra en insättning på kontot." +
                          "\nAnnars tryck valfri tangent för att återgå till föregående meny." +
                          "\n---" +
                          "\nDitt val: ");

            string answer = Console.ReadLine();
            if (answer.ToLower() == "j")
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
                Console.Clear();
                Console.WriteLine($"\n\nDin insättning till ditt {selectdAccount.AccType}: {selectdAccount.AccountNumber} är klar. {account.DateCreated}" +
                    $"\n\nDItt nya saldo är: {selectdAccount.Balance} {selectdAccount.Currency}");

                Console.WriteLine();
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
            Console.WriteLine($"Grattis! Du har skapat ett nytt {account.AccType.ToLower()}." +
                              $"\n######" +
                              $"\nKontonummer: {account.AccountNumber}" +
                              $"\nKontotyp:    {account.AccType}" +
                              $"\nSaldo:       {account.Balance}" +
                              $"\nValuta:      {account.Currency}" +
                              $"\nSkapat:      {account.DateCreated:g}");
        }

        //Skriver ut kundens alla konton
        public void PrintAccounts(bool displayGoBackMessage = true)
        {
            Console.Clear();
            if (UserAccounts.Any() != true)
            {
                Console.WriteLine("Du har för närvarande inga konton.\n");
            }
            else
            {
                Console.WriteLine($"KONTON" +
                                $"\n{FirstName} {LastName}" +
                                 "\n######");
                UserAccounts.ForEach(account => Console.WriteLine($"Kontonummer: {account.AccountNumber}" +
                                                                $"\nKontotyp:    {account.AccType}" +
                                                                $"\nSaldo:       {account.Balance}" +
                                                                $"\nValuta:      {account.Currency}" +
                                                                $"\nSkapat:      {account.DateCreated:g}\n"));
            }
        }

        private void TransferFromAccount()
        {
            bool RunMenu = true;

            if (UserAccounts.Count <= 1)
            {
                Console.Clear();
                Console.Write("Du behöver minst två konton för att kunna göra en intern överföring.\n" +
                            "\nTryck \"ENTER\" för att återgå till föregående meny.");
                Console.ReadKey();
            }
            else
            {
                while (RunMenu)
                {
                    PrintAccounts();
                    int counter = 1;
                    Console.WriteLine("Välj ett av dina konton du vill föra över pengar från:");
                    UserAccounts.ForEach(a => Console.Write($"[{counter++}] {a.AccType}\n"));
                    Console.Write("---" +
                                "\n[0] Avsluta överföringen" +
                                "\n---" +
                                "\nVälj konto: ");

                    string accountChoice = Console.ReadLine();

                    switch (accountChoice)
                    {
                        case "1":
                            var accountFirstIndex = UserAccounts.ElementAt(0);
                            if (CheckFunds(accountFirstIndex))
                            {
                                TransferToAccount(accountFirstIndex);
                                RunMenu = false;
                            }
                            else
                            {
                                Console.Write($"\nKontot saknar täckning. Välj ett annat konto." +
                                              $"\nTryck \"ENTER\" och försök igen.");
                                Console.ReadKey();
                            }
                            break;
                        case "2":
                            var accountSecondIndex = UserAccounts.ElementAt(1);
                            if (CheckFunds(accountSecondIndex))
                            {
                                TransferToAccount(accountSecondIndex);
                                RunMenu = false;
                            }
                            else
                            {
                                Console.Write($"\nKontot saknar täckning. Välj ett annat konto." +
                                    $"\nTryck\"ENTER\" och försök igen.");
                                Console.ReadKey();
                            }
                            break;
                        case "3":
                            var accountThirdIndex = UserAccounts.ElementAt(2);
                            if (CheckFunds(accountThirdIndex))
                            {
                                TransferToAccount(accountThirdIndex);
                                RunMenu = false;
                            }
                            else
                            {
                                Console.Write($"\nKontot saknar täckning. Välj ett annat konto." +
                                    $"\nTryck\"ENTER\" och försök igen.");
                                Console.ReadKey();
                            }
                            break;
                        case "0":
                            Console.Write("\nDu har valt att avbryta processen! Tryck \"ENTER\" för att återgå till föregående meny.");
                            Console.ReadKey();
                            RunMenu = false;
                            break;
                        default:
                            Console.Write("\nOgiltigt val! Tryck \"ENTER\" och försök igen.");
                            Console.ReadKey();
                            break;
                    }
                }
            }
        }

        public bool CheckFunds(Account account)
        {
            if (account.GetBalance() > 0)
            {
                return true;
            }
            return false;
        }

        private void TransferToAccount(Account transferAccount)
        {
            bool transferComplete = false;
            decimal transferSum = 0;

            while (true)
            {
                PrintAccounts();
                Console.Write($"Hur mycket vill du föra över från {transferAccount.AccType.ToLower()}t?" +
                              $"\nBelopp: ");

                if (decimal.TryParse(Console.ReadLine(), out transferSum))
                {
                    if (transferSum <= transferAccount.GetBalance())
                    {
                        break;
                    }
                    else
                    {
                        Console.Write($"\nDet finns inte tillräckligt med pengar på kontot!" +
                                        $"\nTryck \"ENTER\" och försök igen.");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.Write("\nFelaktig inmatning! Ange beloppet endast i siffror." +
                                    "\nTryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                }
            }

            while (!transferComplete)
            {
                UserAccounts.Remove(transferAccount);
                PrintAccounts();
                int counter = 1;
                Console.WriteLine($"Välj vilket konto du vill föra över {transferSum:c} till:");
                UserAccounts.ForEach(a => Console.Write($"[{counter++}] {a.AccType}\n"));
                Console.Write("---" +
                           "\nVälj konto: ");

                string accountChoice = Console.ReadLine();

                switch (accountChoice)
                {
                    case "1":
                        var accountFirstIndex = UserAccounts.ElementAt(0);
                        transferAccount.Withdraw(transferSum);
                        accountFirstIndex.Deposit(transferSum);
                        Console.Clear();
                        Console.WriteLine($"Överföringen lyckades! Ditt nya saldo för {accountFirstIndex.AccType.ToLower()}t är {accountFirstIndex.GetBalance():c}.");
                        transferComplete = true;
                        Transaction f1 = new(transferAccount, transferSum, "Överföring", true);
                        Transaction t1 = new(accountFirstIndex, transferSum, "Överföring", false);
                        break;
                    case "2":
                        var accountSecondIndex = UserAccounts.ElementAt(1);
                        transferAccount.Withdraw(transferSum);
                        accountSecondIndex.Deposit(transferSum);
                        Console.Clear();
                        Console.WriteLine($"Överföringen lyckades! Ditt nya saldo för {accountSecondIndex.AccType.ToLower()}t är {accountSecondIndex.GetBalance():c}.");
                        transferComplete = true;
                        Transaction f2 = new(transferAccount, transferSum, "Överföring", true);
                        Transaction t2 = new(accountSecondIndex, transferSum, "Överföring", false);
                        break;
                    default:
                        Console.Write("\nOgiltigt val! Tryck \"ENTER\" och försök igen.");
                        Console.ReadKey();
                        break;
                }
            }
            UserAccounts.Add(transferAccount);
            Console.Write("\nTryck \"ENTER\" för att återgå till föregående meny.");
            Console.ReadKey();
        }

        //Metod för att göra överföringar mellan egna konton
        public void InternalTransaction()
        {
            TransferFromAccount();
        }

        //Metod för att göra överföringar mellan kunders konton
        public void ExternalTransaction()
        {

        }

        // Metod för att låna pengar från banken.
        public void TakeLoan(Account account)
        {
            // Använder double så Math.Pow ska funka.          
            double totalBalance = 0;
            double interest = 0.0848;
            // double som ska in i whileloop. Total loan.
            double loanMoney = 0;
            // double som ska vara i while loop. Total loan time.
            double loanTime = 0;
            // double som sparar hur mycket blir total lånet samt månad betaldvis.
            double totalDebt = 0;
            double monthlyDebt = 0;


            // Clear console så att designen blir finare.
            Console.Clear();
            Console.WriteLine("Lånaeavdelningen");
            Console.WriteLine("*****************************");
            Console.WriteLine("Välkommen till låneavdelningen. Vi erbjuder just nu annuitetslån till 8,48% ränta.\n");
           
         
                // skriver ut alla konton och hur mycket summa det finns på de.
                foreach (var item in UserAccounts)
                {
                    Console.WriteLine($"Konto nr\tSaldo\tValuta\tSkapat");
                    Console.WriteLine("****************************************************");
                    Console.WriteLine($"{item.AccountNumber}\t{item.Balance}\t{item.Currency}");
                   
                    // adderar alla konton balans tillsammas med totalt balance.
                    totalBalance = totalBalance + Convert.ToDouble(item.Balance);
                    
                }

            // while loop när man hanterar input från användaren i loop. 
            while (true)
            {
                // Skriver ut totalbalans och hur mycket en kund kan låna pengar (max 5 gånger).
                Console.WriteLine("\nDitt totala saldo är " + totalBalance + " kr" +
                "\nDu kan låna max " + totalBalance * 5 + " kr" +
                "\n\nHur mycket vill du låna?");
                // användare input
                string userInput = Console.ReadLine();
                Console.WriteLine();

                // hanterar felhantering, ifall användaren vill låna mer eller mer än 5 gånger.
                if (double.TryParse(userInput, out loanMoney) && loanMoney <= totalBalance * 5 && loanMoney > 0)
                {
                    Console.WriteLine("Du har valt att låna " + loanMoney + " kr. till 8,48% ränta.");
                    while (true)
                    {
                        Console.WriteLine("Hur lång avbetalningstid? (1-10 år)");
                        userInput = Console.ReadLine();
                        if (double.TryParse(userInput, out loanTime) && loanTime <= 10)
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
                    Console.WriteLine("Din totala skuld är " + Math.Round(totalDebt, 2) + " kr och din månadskostnad kommer bli " + Math.Round(monthlyDebt, 2) + " kr/månad.");
                    break;
                }
                else
                {
                    Console.WriteLine("Du kan inte låna så mycket pengar. Vänligen försök med en lägre summa.");
                }

            }

            //konvertera double till decimal
            decimal loan = Convert.ToDecimal(loanMoney);

            // Logg för kontohistorik
            Transaction t1 = new(account, loan, "Lån", false);

            // Här plusas summan på lånade pengar tillsammans med pengarna som fanns redan i lönekontot
            account.Balance += Convert.ToDecimal(loan);
            Console.WriteLine($"totalsumman på ditt lönekonto är {account.Balance} kr.");


            Console.Write("Tryck \"ENTER\" för att återgå till föregående meny.");
            Console.ReadKey();
        }

        public List<Account> GetUserAccounts()
        {
            return UserAccounts;
        }

        public void PrintAllTransactions()
        {
            Console.Clear();
            Console.WriteLine($"{FirstName} {LastName}: Kontohistorik" +
                                                "\n=================");
            
            foreach (var account in UserAccounts)
            {
                //Console.WriteLine($"\n{account.AccType}");
                Console.WriteLine($"\nKonto\t\tKontonr.\tBelopp\t\tTyp\t\tDatum");
                Console.WriteLine($"==========================================================================");
                account.PrintAccountHistory();
            }

            Console.Write("\nTryck \"ENTER\" för att återgå till föregående meny.");
            Console.ReadKey();            
        }
    }
}