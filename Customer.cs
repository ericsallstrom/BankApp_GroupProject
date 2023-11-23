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
        private Account _checkingAccount { get; set; }
        private Account _globalAccount { get; set; }
        private SavingsAccount _savingsAccount { get; set; }

        private static readonly LogInManager LIM = new();

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<Account> UserAccounts = new();

        public Customer(string username, string password, string firstname, string lastname)
            : base(username, password)
        {
            FirstName = firstname;
            LastName = lastname;
            IsAdmin = false;
            IsBlocked = false;
            _checkingAccount = new() { AccType = "Lönekonto" };
        }

        public Account GetCheckingAccount()
        {
            return _checkingAccount;
        }

        public void AddUserAccount(Account account)
        {
            UserAccounts.Add(account);
        }

        //Skapa Lönekontot
        public void NewCheckingAccount()
        {
            Console.Clear();

            if (!UserAccounts.Exists(a => a.AccType == "Lönekonto"))
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
            _savingsAccount = new SavingsAccount() { AccType = "Sparkonto" };

            if (!UserAccounts.Exists(a => a.AccType == "Sparkonto"))
            {
                int answer = ProceedCreatingAccount(_savingsAccount);

                if (answer == 1)
                {
                    UserAccounts.Add(_savingsAccount);
                    _savingsAccount.InterestChoice();

                    DoYouWantToDeposit(_savingsAccount);
                    PrintAccountSuccess(_savingsAccount);

                    _savingsAccount.IsSavingsAccount = true; //kollar om _savingsAccount            
                    _savingsAccount.CalcInterest();
                }
                else
                {
                    UserAccounts.Remove(_savingsAccount);
                    Console.Write("\nDu har valt att avbryta processen! Tryck \"ENTER\" för att återgå till föregående meny.");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.Write($"Du har redan ett {_savingsAccount.AccType.ToLower()}!\n" +
                              "\nTryck \"ENTER\" för att återgå till föregående meny.");
                Console.ReadKey();
            }
        }

        //Skapa Utlandskontot
        public void NewGlobalAccount()
        {
            bool currencySet = false;
            Console.Clear();
            _globalAccount = new Account() { AccType = "Utlandskonto" };

            if (!UserAccounts.Exists(a => a.AccType == "Utlandskonto"))
            {
                UserAccounts.Add(_globalAccount);
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
                    PrintAccountSuccess(_globalAccount);
                    DoYouWantToDeposit(_globalAccount);
                }
                else
                {
                    UserAccounts.Remove(_globalAccount);
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
                Console.Write("Du har för närvarande inga konton.\n" +
                    "\nTryck \"ENTER\" för att återgå till föregående meny.");
                Console.ReadKey();
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

        public bool CheckFunds(Account account)
        {
            if (account.GetBalance() > 0)
            {
                return true;
            }
            return false;
        }

        public void PrintEveryCustomersAccount(bool displayGoBackMessage = true)
        {
            Console.Clear();

            if (UserAccounts.Count == 0)
            {
                Console.WriteLine("För tillfället existerar inga kundkonton i banken.\n");
            }
            else
            {
                var customers = LIM.GetAllCustomers();

                Console.WriteLine("EXTERNA KUNDKONTON" +
                                "\n######");

                foreach (var customer in customers)
                {
                    foreach (var acc in UserAccounts)
                    {
                        Console.WriteLine($"Användarnamn: {customer.Username}\tKontonr: {acc.AccountNumber}\tKontotyp: {acc.AccType}\tAntal konton i UserAccounts: {UserAccounts.Count}\n");
                    }
                }
            }
        }
        //frågar om man vill göra en insättning till befintligt konto
        public void AccountDeposit(Account account)
        {
            while (true)
            {
                Console.Clear();
                //visar konton       
                PrintAccounts(false); //false för att inte skriva ut tillbaka

                Console.Write("Ange kontonummret för det konto du önskar sätta in pengar på." +
                            "\nKontonr: ");

                string accountNrToDeposit = Console.ReadLine().Trim();

                Account selectedAccount = UserAccounts.FirstOrDefault(account => account.AccountNumber.Trim() == accountNrToDeposit);

                if (selectedAccount != null)
                {
                    selectedAccount.MakeDeposit();
                    Console.Clear();
                    Console.WriteLine($"\n\nDin insättning till ditt {selectedAccount.AccType.ToLower()}: {selectedAccount.AccountNumber} är klar. {account.DateCreated}" +
                        $"\n\nDitt nya saldo är: {selectedAccount.Balance} {selectedAccount.Currency}");

                    Console.Write("\nTryck \"ENTER\" för att återgå till föregående meny.");
                    Console.ReadKey();

                }
                else
                {
                    Console.Write("\nOgiltigt kontonummer! Tryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                }
            }
        }

        private void TransferToExternalAccount(Account transferAccount)
        {
            decimal transferAmount = 0;

            while (true)
            {
                PrintAccounts();
                Console.Write($"Hur mycket pengar vill du föra över från ditt {transferAccount.AccType.ToLower()}?" +
                            $"\nBelopp: ");

                if (decimal.TryParse(Console.ReadLine(), out transferAmount))
                {
                    if (transferAmount <= transferAccount.GetBalance())
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

            bool transferComplete = false;

            while (!transferComplete)
            {
                PrintEveryCustomersAccount();

                Console.Write("Ange kontonummret för det konto du önskar föra över pengar till." +
                            "\nKontonr: ");

                string accountNrToDeposit = Console.ReadLine();

                if (UserAccounts.Exists(a => a.AccountNumber == accountNrToDeposit))
                {
                    var selectedAccount = UserAccounts.Find(a => a.AccountNumber == accountNrToDeposit);

                    selectedAccount.Deposit(transferAmount);
                    transferAccount.Withdraw(transferAmount);
                    Console.Write($"\nÖverföringen lyckades! Tryck \"ENTER\" för att återgå till föregående meny.");
                    Console.ReadKey();
                    transferComplete = true;
                }
                else
                {
                    Console.Write("\nOgiltigt kontonummer! Tryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                }
            }
        }

        private void TransferToInternalAccount(Account transferAccount)
        {
            decimal transferAmount = 0;

            while (true)
            {
                PrintAccounts();
                Console.Write($"Hur mycket vill du föra över från {transferAccount.AccType.ToLower()}t?" +
                              $"\nBelopp: ");

                if (decimal.TryParse(Console.ReadLine(), out transferAmount))
                {
                    if (transferAmount <= transferAccount.GetBalance())
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

            bool transferComplete = false;

            while (!transferComplete)
            {
                UserAccounts.Remove(transferAccount);
                PrintAccounts();
                int counter = 1;
                Console.WriteLine($"Välj det konto du vill föra över {transferAmount:c} till.\n");

                UserAccounts.ForEach(a => Console.Write($"[{counter++}] {a.AccType}\n"));
                Console.Write("---" +
                           "\nVälj konto: ");

                string accountChoice = Console.ReadLine();

                switch (accountChoice)
                {
                    case "1":
                        var accountFirstIndex = UserAccounts.ElementAtOrDefault(0);
                        transferAccount.Withdraw(transferAmount);
                        accountFirstIndex.Deposit(transferAmount);
                        Console.Clear();
                        Console.WriteLine($"Överföringen lyckades! Ditt nya saldo för {accountFirstIndex.AccType.ToLower()}t är {accountFirstIndex.GetBalance():c}.");
                        transferComplete = true;
                        Transaction f1 = new(transferAccount, transferSum, "Överföring", true);
                        Transaction t1 = new(accountFirstIndex, transferSum, "Överföring", false);
                        break;
                    case "2":
                        var accountSecondIndex = UserAccounts.ElementAtOrDefault(1);
                        transferAccount.Withdraw(transferAmount);
                        accountSecondIndex.Deposit(transferAmount);
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
            UserAccounts.Reverse();
            Console.Write("\nTryck \"ENTER\" för att återgå till föregående meny.");
            Console.ReadKey();
        }

        //Metod för att göra överföringar mellan egna konton
        public void InternalTransaction()
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
                    string accountChoice = ChooseAccountForWithdraw();

                    switch (accountChoice)
                    {
                        case "1":
                            var accountFirstIndex = UserAccounts.ElementAtOrDefault(0);
                            if (CheckFunds(accountFirstIndex))
                            {
                                TransferToInternalAccount(accountFirstIndex);
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
                            var accountSecondIndex = UserAccounts.ElementAtOrDefault(1);
                            if (CheckFunds(accountSecondIndex))
                            {
                                TransferToInternalAccount(accountSecondIndex);
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
                            var accountThirdIndex = UserAccounts.ElementAtOrDefault(2);
                            if (accountThirdIndex != null)
                            {
                                if (CheckFunds(accountThirdIndex))
                                {
                                    TransferToInternalAccount(accountThirdIndex);
                                    RunMenu = false;
                                }
                                else
                                {
                                    Console.Write($"\nKontot saknar täckning. Välj ett annat konto." +
                                        $"\nTryck\"ENTER\" och försök igen.");
                                    Console.ReadKey();
                                }
                            }
                            else
                            {
                                goto default;
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

        //Metod för att göra överföringar mellan kunders konton
        public void ExternalTransaction()
        {
            bool RunMenu = true;

            if (UserAccounts.Count == 0)
            {
                Console.Clear();
                Console.Write("Du behöver minst ett konto för att kunna göra en extern överföring.\n" +
                            "\nTryck \"ENTER\" för att återgå till föregående meny.");
                Console.ReadKey();
            }
            else
            {
                while (RunMenu)
                {
                    string accountChoice = ChooseAccountForWithdraw();

                    switch (accountChoice)
                    {
                        case "1":
                            var accountFirstIndex = UserAccounts.ElementAtOrDefault(0);
                            if (CheckFunds(accountFirstIndex))
                            {
                                TransferToExternalAccount(accountFirstIndex);
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
                            var accountSecondIndex = UserAccounts.ElementAtOrDefault(1);
                            if (accountSecondIndex != null)
                            {
                                if (CheckFunds(accountSecondIndex))
                                {
                                    TransferToExternalAccount(accountSecondIndex);
                                    RunMenu = false;
                                }
                                else
                                {
                                    Console.Write($"\nKontot saknar täckning. Välj ett annat konto." +
                                        $"\nTryck\"ENTER\" och försök igen.");
                                    Console.ReadKey();
                                }
                            }
                            else
                            {
                                goto default;
                            }
                            break;
                        case "3":
                            var accountThirdIndex = UserAccounts.ElementAtOrDefault(2);
                            if (accountThirdIndex != null)
                            {
                                if (CheckFunds(accountThirdIndex))
                                {
                                    TransferToExternalAccount(accountThirdIndex);
                                    RunMenu = false;
                                }
                                else
                                {
                                    Console.Write($"\nKontot saknar täckning. Välj ett annat konto." +
                                        $"\nTryck\"ENTER\" och försök igen.");
                                    Console.ReadKey();
                                }
                            }
                            else
                            {
                                goto default;
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

        private string ChooseAccountForWithdraw()
        {
            PrintAccounts();
            int counter = 1;
            Console.WriteLine("Välj det konto du vill föra över pengar från.\n");
            UserAccounts.ForEach(a => Console.Write($"[{counter++}] {a.AccType}\n"));
            Console.Write("---" +
                        "\n[0] Avsluta överföringen" +
                        "\n---" +
                        "\nVälj konto: ");

            string accountChoice = Console.ReadLine();
            return accountChoice;
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
            // Check to see if the customerAccounts have an checkingAccount.
            if (UserAccounts.Any() != true)
            {
                Console.WriteLine("Du har inga konton än.\n");
                Console.WriteLine("Tryck för att gå tillbaka");
                Console.ReadKey();
                return;
            }
            else
                // Print out all of customers accounts and balance         
         
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
                // Print out total balance and how much customerAccounts can loan (max 5 times the amount of total balance). 
                Console.WriteLine("\nDitt totala saldo är " + totalBalance + "kr" +
                "\nDu kan låna max " + totalBalance * 5 + "kr" +
                "\n\nHur mycket vill du låna?");
                // User input.
                loanMoney = int.Parse(Console.ReadLine());
                // See if customerAccounts can loan that amount of money or not. Also make sure it isnt possible to type in negative number.
                if (loanMoney <= totalBalance * 5 && loanMoney > 0)
                // Skriver ut totalbalans och hur mycket en kund kan låna pengar (max 5 gånger).
                Console.WriteLine("\nDitt totala saldo är " + totalBalance + " kr" +
                "\nDu kan låna max " + totalBalance * 5 + " kr" +
                "\n\nHur mycket vill du låna? (Skriv 0 för att inte låna några pengar och gå tillbaka)");
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
                else if ((double.TryParse(userInput, out loanMoney) && loanMoney == 0))
                {
                    Console.WriteLine("Du har valt att inte långa några pengar.");
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


            Console.Write("\nTryck \"ENTER\" för att återgå till föregående meny.");
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