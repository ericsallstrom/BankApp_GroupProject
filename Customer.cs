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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public static List<Customer> AllCustomers { get; } = new List<Customer>();
        public List<Account> CustomerAccounts { get; set; }

        AsciiArt ascii = new();

        public Customer(string username, string password, string firstname, string lastname)
            : base(username, password)
        {
            FirstName = firstname;
            LastName = lastname;
            IsAdmin = false;
            IsBlocked = false;
            CustomerAccounts = new List<Account>();
            AllCustomers.Add(this);
        }

        //Skapa Lönekontot
        public void NewCheckingAccount(Customer customer)
        {
            Console.Clear();
            Console.WriteLine(ascii.Header());

            Account checkingAccount = new(AccountType.Checking, customer);

            checkingAccount.SetCurency("SEK");

            if (!customer.CustomerAccounts.Any(a => a.Type == AccountType.Checking))
            {
                int answer = ProceedCreatingAccount(checkingAccount);

                if (answer == 1)
                {
                    customer.CustomerAccounts.Add(checkingAccount);
                    PrintAccountSuccess(checkingAccount);
                    DoYouWantToDeposit(checkingAccount);
                }
                else
                {
                    Console.Write("\nDu har valt att avbryta processen! Tryck \"ENTER\" för att återgå till föregående meny.");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.Write($"Du har redan ett {checkingAccount.GetAccountType(checkingAccount).ToLower()}!\n" +
                              "\nTryck \"ENTER\" för att återgå till föregående meny.");
                Console.ReadKey();
            }
        }

        //skapa nytt sparkonto
        public void NewSavingsAccount(Customer customer)
        {
            Console.Clear();
            Console.WriteLine(ascii.Header());          

            SavingsAccount savingsAccount = new(AccountType.Savings, customer);

            savingsAccount.SetCurency("SEK");

            if (!customer.CustomerAccounts.Any(a => a.Type == AccountType.Savings))
            {
                int answer = ProceedCreatingAccount(savingsAccount);

                if (answer == 1)
                {
                    customer.CustomerAccounts.Add(savingsAccount);
                    savingsAccount.InterestChoice();

                    DoYouWantToDeposit(savingsAccount);
                    PrintAccountSuccess(savingsAccount);

                    savingsAccount.CalcInterest();
                }
                else
                {
                    Console.Write("\nDu har valt att avbryta processen! Tryck \"ENTER\" för att återgå till föregående meny.");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.Write($"Du har redan ett {savingsAccount.GetAccountType(savingsAccount).ToLower()}!\n" +
                              "\nTryck \"ENTER\" för att återgå till föregående meny.");
                Console.ReadKey();
            }
        }

        //Skapa Utlandskontot
        public void NewGlobalAccount(Customer customer)
        {
            Console.Clear();
            Console.WriteLine(ascii.Header());            
            
            bool currencySet = false;
            Account globalAccount = new(AccountType.Global, customer);

            if (!customer.CustomerAccounts.Any(a => a.Type == AccountType.Global))
            {
                int answer = ProceedCreatingAccount(globalAccount);

                if (answer == 1)
                {
                    while (!currencySet)
                    {
                        Console.Clear();
                        Console.WriteLine(ascii.Header());
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
                                globalAccount.SetCurency("EUR");
                                currencySet = true;
                                break;
                            case "2":
                                globalAccount.SetCurency("USD");
                                currencySet = true;
                                break;
                            default:
                                Console.Write("\nOgiltigt val! Tryck \"ENTER\" och försök igen.");
                                Console.ReadKey();
                                break;
                        }
                    }
                    customer.CustomerAccounts.Add(globalAccount);
                    PrintAccountSuccess(globalAccount);
                    DoYouWantToDeposit(globalAccount);
                }
                else
                {
                    Console.Write("\nDu har valt att avbryta processen! Tryck \"ENTER\" för att återgå till föregående meny.");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.Write($"Du har redan ett {globalAccount.GetAccountType(globalAccount).ToLower()}!\n" +
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
                Console.WriteLine(ascii.Header());                

                Console.Write($"Vill du skapa ett nytt {account.GetAccountType(account).ToLower()}?" +
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
                account.MakeADeposit(account);
            }
        }

        //Skriver ut Grattis och kontoöversikt
        public void PrintAccountSuccess(Account account)
        {
            Console.Clear();
            Console.WriteLine(ascii.Header());                    

            Console.WriteLine($"Grattis! Du har skapat ett nytt {account.GetAccountType(account).ToLower()}.\n" +
                              $"\nKontonummer: {account.AccountNumber}" +
                              $"\nKontotyp:    {account.GetAccountType(account)}" +
                              $"\nSaldo:       {account.Balance}" +
                              $"\nValuta:      {account.Currency}" +
                              $"\nSkapat:      {account.DateCreated:g}");
        }

        //Skriver ut kundens alla konton
        public void PrintAccounts(bool displayGoBackMessage = true)
        {
            Console.Clear();
            Console.WriteLine(ascii.Header());

            if (CustomerAccounts.Any() != true)
            {
                Console.Write("Du har för närvarande inga konton.\n" +
                    "\nTryck \"ENTER\" för att återgå till föregående meny.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine($"{FirstName} {LastName} - Konton\n");
                CustomerAccounts.ForEach(account => Console.WriteLine($"Kontonummer: {account.AccountNumber}" +
                                                                    $"\nKontotyp:    {account.GetAccountType(account)}" +
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

        public void PrintEveryCustomersAccount(Customer inloggedCustomer)
        {
            Console.Clear();
            Console.WriteLine(ascii.Header());
            
            if (Account.AllCustomerAccounts.Count == 0)
            {
                Console.WriteLine("För tillfället existerar inga kundkonton i banken.\n");
            }
            else
            {
                Console.WriteLine($"EXTERNA KUNDKONTON\n" +
                    $"\nNamn\t\t\t\tKontonummer\t\tKontotyp" +
                    $"\n=================================================================");
                foreach (var account in Account.AllCustomerAccounts)
                {
                    if (account.CustomerName != inloggedCustomer.Username)
                    {
                        if (!inloggedCustomer.CustomerAccounts.Exists(a => a.AccountNumber == account.AccountNumber) && account.Type == AccountType.Checking)
                        {
                            if (account.CustomerName.Length >= 16)
                            {
                                Console.Write($"{account.CustomerName}\t\t{account.AccountNumber}\t\t{account.GetAccountType(account)}\n");
                            }
                            else
                            {
                                Console.Write($"{account.CustomerName}\t\t\t{account.AccountNumber}\t\t{account.GetAccountType(account)}\n");
                            }
                        }
                    }
                }
            }
        }
        //frågar om man vill göra en insättning till befintligt konto
        public void AccountDeposit()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ascii.Header());
                  
                PrintAccounts(false); //false för att inte skriva ut tillbaka

                Console.Write("Ange kontonummret för det konto du önskar sätta in pengar på. (Skriv 0 för att avbryta processen och gå tillbaka)." +
                            "\nKontonr: ");

                string accountNrToDeposit = Console.ReadLine().Trim();

                Account selectedAccount = CustomerAccounts.FirstOrDefault(account => account.AccountNumber.Trim() == accountNrToDeposit);

                if (selectedAccount != null)
                {
                    Console.Clear();                    
                    selectedAccount.MakeADeposit(selectedAccount);
                    Console.WriteLine(ascii.Header());
                    Console.WriteLine($"\n\nDin insättning till ditt {selectedAccount.GetAccountType(selectedAccount).ToLower()}: {selectedAccount.AccountNumber} har gått igenom." +
                                      $"\nDitt nya saldo är: {selectedAccount.Balance} {selectedAccount.Currency}.");
                    Console.Write("\nTryck \"ENTER\" för att återgå till föregående meny.");
                    Console.ReadKey();
                    break;
                }
                else if (accountNrToDeposit == "0")
                {
                    break;
                }
                else
                {
                    Console.Write("\nOgiltigt kontonummer! Tryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                }
            }
        }

        private void TransferToExternalAccount(Account transferAccount, Customer customer)
        {
            decimal transferAmount = 0;

            while (true)
            {
                PrintAccounts();
                Console.Write($"Hur mycket pengar vill du föra över från ditt {transferAccount.GetAccountType(transferAccount).ToLower()}?" +
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
                PrintEveryCustomersAccount(customer);

                Console.Write("\nAnge kontonummret för det konto du önskar föra över pengar till." +
                            "\nKontonr: ");

                string accountNrToDeposit = Console.ReadLine();

                var selectedAccount = Account.AllCustomerAccounts.Find(a => a.AccountNumber == accountNrToDeposit);

                if (selectedAccount != null)
                {
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
            decimal originalTransferAmount = 0;

            while (true)
            {
                PrintAccounts();
                Console.Write($"Hur mycket vill du föra över från {transferAccount.GetAccountType(transferAccount).ToLower()}t?" +
                              $"\nBelopp: ");

                if (decimal.TryParse(Console.ReadLine(), out originalTransferAmount))
                {
                    if (originalTransferAmount <= transferAccount.GetBalance())
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
                CustomerAccounts.Remove(transferAccount);
                PrintAccounts();
                int counter = 1;
                Console.WriteLine($"Välj det konto du vill föra över {originalTransferAmount:c} {transferAccount.Currency} till.\n");

                CustomerAccounts.ForEach(a => Console.Write($"[{counter++}] {a.GetAccountType(a)}\n"));
                Console.Write("---" +
                           "\nVälj konto: ");

                string accountChoice = Console.ReadLine();

                switch (accountChoice)
                {
                    case "1":
                        var accountFirstIndex = CustomerAccounts.ElementAt(0);

                        if (transferAccount.Currency != accountFirstIndex.Currency)
                        {
                            transferAmount = ExchangeManager.Exchange.CurrencyConverter(transferAccount.Currency, accountFirstIndex.Currency, originalTransferAmount);
                        }
                        else
                        {
                            transferAmount = originalTransferAmount;
                        }

                        transferAccount.Withdraw(originalTransferAmount);
                        accountFirstIndex.Deposit(transferAmount);

                        Console.Clear();
                        Console.WriteLine(ascii.Header());
                        Console.WriteLine($"Överföringen lyckades! Ditt nya saldo för {accountFirstIndex.GetAccountType(accountFirstIndex).ToLower()}t " +
                                          $"är {accountFirstIndex.GetBalance():c}.");
                        Transaction f1 = new(transferAccount, originalTransferAmount, "Överföring", true);
                        Transaction t1 = new(accountFirstIndex, originalTransferAmount, "Överföring", false);
                        transferComplete = true;
                        break;
                    case "2":
                        var accountSecondIndex = CustomerAccounts.ElementAt(1);

                        if (transferAccount.Currency != accountSecondIndex.Currency)
                        {
                            transferAmount = ExchangeManager.Exchange.CurrencyConverter(transferAccount.Currency, accountSecondIndex.Currency, originalTransferAmount);
                        }
                        else
                        {
                            transferAmount = originalTransferAmount;
                        }

                        transferAccount.Withdraw(originalTransferAmount);
                        accountSecondIndex.Deposit(transferAmount);

                        Console.Clear();
                        Console.WriteLine(ascii.Header());
                        Console.WriteLine($"Överföringen lyckades! Ditt nya saldo för {accountSecondIndex.GetAccountType(accountSecondIndex).ToLower()}t " +
                                          $"är {accountSecondIndex.GetBalance():c}.");
                        Transaction f2 = new(transferAccount, originalTransferAmount, "Överföring", true);
                        Transaction t2 = new(accountSecondIndex, originalTransferAmount, "Överföring", false);
                        transferComplete = true;
                        break;
                    case "3":
                        var accountThirdIndex = CustomerAccounts.ElementAt(2);

                        if (transferAccount.Currency != accountThirdIndex.Currency)
                        {
                            transferAmount = ExchangeManager.Exchange.CurrencyConverter(transferAccount.Currency, accountThirdIndex.Currency, originalTransferAmount);
                        }
                        else
                        {
                            transferAmount = originalTransferAmount;
                        }
                        transferAccount.Withdraw(originalTransferAmount);
                        accountThirdIndex.Deposit(transferAmount);

                        Console.Clear();
                        Console.WriteLine(ascii.Header());                        
                        Console.WriteLine($"Överföringen lyckades! Ditt nya saldo för {accountThirdIndex.GetAccountType(accountThirdIndex).ToLower()}t " +
                                          $"är {accountThirdIndex.GetBalance():c}.");
                        Transaction f3 = new(transferAccount, originalTransferAmount, "Överföring", true);
                        Transaction t3 = new(accountThirdIndex, originalTransferAmount, "Överföring", false);
                        transferComplete = true;
                        break;
                    default:
                        Console.Write("\nOgiltigt val! Tryck \"ENTER\" och försök igen.");
                        Console.ReadKey();
                        break;
                }
            }
            CustomerAccounts.Add(transferAccount);
            CustomerAccounts.Reverse();
            Console.Write("\nTryck \"ENTER\" för att återgå till föregående meny.");
            Console.ReadKey();
        }

        //Metod för att göra överföringar mellan egna konton
        public void InternalTransaction(Customer customer)
        {
            bool RunMenu = true;

            if (CustomerAccounts.Count <= 1)
            {
                Console.Clear();
                Console.WriteLine(ascii.Header());
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
                            var accountFirstIndex = CustomerAccounts.ElementAtOrDefault(0);
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
                            var accountSecondIndex = CustomerAccounts.ElementAtOrDefault(1);
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
                            var accountThirdIndex = CustomerAccounts.ElementAtOrDefault(2);
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
        public void ExternalTransaction(Customer customer)
        {
            bool RunMenu = true;

            if (CustomerAccounts.Count == 0)
            {
                Console.Clear();
                Console.WriteLine(ascii.Header());
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
                            var accountFirstIndex = CustomerAccounts.ElementAtOrDefault(0);
                            if (CheckFunds(accountFirstIndex))
                            {
                                TransferToExternalAccount(accountFirstIndex, customer);
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
                            var accountSecondIndex = CustomerAccounts.ElementAtOrDefault(1);
                            if (accountSecondIndex != null)
                            {
                                if (CheckFunds(accountSecondIndex))
                                {
                                    TransferToExternalAccount(accountSecondIndex, customer);
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
                            var accountThirdIndex = CustomerAccounts.ElementAtOrDefault(2);
                            if (accountThirdIndex != null)
                            {
                                if (CheckFunds(accountThirdIndex))
                                {
                                    TransferToExternalAccount(accountThirdIndex, customer);
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
            //CustomerAccounts.ForEach(a => Console.Write($"[{counter++}] {a.AccType}\n"));
            CustomerAccounts.ForEach(a => Console.Write($"[{counter++}] {a.GetAccountType(a)}\n"));
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
            Console.WriteLine(ascii.Header());
            Console.WriteLine("Låneavdelningen");
            Console.WriteLine("*****************************");
            Console.WriteLine("Välkommen till låneavdelningen. Vi erbjuder just nu annuitetslån till 8,48% ränta.\n");
            // Check to see if the customerAccounts have an checkingAccount.
            if (CustomerAccounts.Any() != true)
            {
                Console.WriteLine("Du har inga konton än.\n");
                Console.WriteLine("Tryck för att gå tillbaka");
                Console.ReadKey();
                return;
            }
            else
                // Print out all of customers accounts and balance         

                // skriver ut alla konton och hur mycket summa det finns på de.
                foreach (var item in CustomerAccounts)
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
            return CustomerAccounts;
        }

        public void PrintAllTransactions()
        {
            Console.Clear();
            Console.WriteLine(ascii.Header());
            Console.WriteLine($"{FirstName} {LastName} - Kontohistorik");

            foreach (var account in CustomerAccounts)
            {
                Console.WriteLine($"\nKonto\t\tKontonr.\tBelopp\t\tHändelse\tDatum");
                Console.WriteLine($"==========================================================================");
                account.PrintAccountHistory();
            }

            Console.Write("\nTryck \"ENTER\" för att återgå till föregående meny.");
            Console.ReadKey();
        }
    }
}