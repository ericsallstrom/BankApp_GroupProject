using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Threading.Channels;
using System.Transactions;

namespace BankApp_GroupProject
{
    // The User-class is inherited by the Customer-class.
    public class Customer : User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Public static list of customers that automatically stores every customer upon creation.
        public static List<Customer> AllCustomers { get; } = new List<Customer>();

        // Public list that stores the accounts of a customer.
        public List<Account> CustomerAccounts { get; set; }

        readonly AsciiArt ascii = new();

        // Constructor for the Customer-class. Other than the two inherited variables from
        // the User-class, it takes two other arguments: firstName and lastName. Also,
        // inside the constructor the IsAdmin and IsBlocked is false upon execution.  
        public Customer(string username, string password, string firstName, string lastName)
            : base(username, password)
        {
            FirstName = firstName;
            LastName = lastName;
            IsAdmin = false;
            IsBlocked = false;

            // A list of the accounts is instantiated when a new customer is created.
            CustomerAccounts = new List<Account>();

            // AllCustomers-list adds the current instance of the Customer-class to its list. 
            AllCustomers.Add(this);
        }

        // Method for creating a new checking account.
        public void NewCheckingAccount(Customer customer)
        {
            Console.Clear();
            Console.WriteLine(ascii.Header());

            // A new account is instantiated with the account-type of 
            // Checking and the customer that's creating the account.
            Account checkingAccount = new(AccountType.Checking, customer);

            // The account's currency is automatically set to SEK.
            checkingAccount.SetCurrency("SEK");

            // A customer can only have one checking account. If the 
            // list of accounts doesn't contain a checking account of this
            // specific customer, they can create a new checking account.
            if (!customer.CustomerAccounts.Any(a => a.Type == AccountType.Checking))
            {
                // Asking the customer if they still wants to open a new checking account.
                int answer = ProceedCreatingAccount(checkingAccount);

                // If they do...
                if (answer == 1)
                {
                    // ... the account is then added to the customers list of accounts.
                    customer.CustomerAccounts.Add(checkingAccount);

                    // Praises the customer upon opening a new account.
                    PrintAccountSuccess(checkingAccount);

                    // Asks the customer if they want to make a deposit to the new account.
                    DoYouWantToDeposit(checkingAccount);
                }
                else
                {
                    // ... else the process is terminated. 
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

        // Method for creating a new savings account.
        public void NewSavingsAccount(Customer customer)
        {
            Console.Clear();
            Console.WriteLine(ascii.Header());

            // A new account is instantiated with the account-type of 
            // Savings and the customer that's creating the account.
            SavingsAccount savingsAccount = new(AccountType.Savings, customer);

            // The account's currency is automatically set to SEK.
            savingsAccount.SetCurrency("SEK");

            // A customer can only have one savings account. If the 
            // list of accounts doesn't contain a savings account of 
            // this specific customer, they can open a new account.
            if (!customer.CustomerAccounts.Any(a => a.Type == AccountType.Savings))
            {
                // Asking the customer if they still wants to open a new account.
                int answer = ProceedCreatingAccount(savingsAccount);

                // If they do...
                if (answer == 1)
                {
                    // ... the account is then added to the customers list of accounts.
                    customer.CustomerAccounts.Add(savingsAccount);

                    // Here the customer can chose between two types of interest.
                    savingsAccount.InterestChoice();

                    // Praises the customer upon opening a new account.
                    PrintAccountSuccess(savingsAccount);

                    // Asks the customer if they want to make a deposit to the new account.
                    DoYouWantToDeposit(savingsAccount);

                    // Calculating and presenting how much money the customer will
                    // earn in a year based on the deposit to the savings account.
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

        // Method for creating a new global account.
        public void NewGlobalAccount(Customer customer)
        {
            Console.Clear();
            Console.WriteLine(ascii.Header());

            // currencySet is false until the customer decides which
            // currency they want to store in their new global account.
            bool currencySet = false;

            // A new account is instantiated with the account-type of 
            // Global and the customer that's creating the account.
            Account globalAccount = new(AccountType.Global, customer);

            // A customer can only have one savings account. If the 
            // list of accounts doesn't contain a account of the type
            // Global of this specific customer, they can open a new account.
            if (!customer.CustomerAccounts.Any(a => a.Type == AccountType.Global))
            {
                // Asking the customer if they still wants to open a new account.
                int answer = ProceedCreatingAccount(globalAccount);

                // If they do...
                if (answer == 1)
                {
                    while (!currencySet)
                    {
                        Console.Clear();
                        Console.WriteLine(ascii.Header());

                        // The user is asked to chose which currency the new account should store.
                        Console.Write("För att skapa ett utlandskonto måste du välja vilken typ av valuta kontots värde skall stå i.\n" +
                                    "Vänligen välj valuta:" +
                                    "\n[1] EUR" +
                                    "\n[2] USD" +
                                    "\n---" +
                                    "\nDitt val: ");

                        string menuChoice = Console.ReadLine();

                        switch (menuChoice)
                        {
                            case "1":
                                // The currency of the global account is set to euro.
                                globalAccount.SetCurrency("EUR");
                                currencySet = true;
                                break;
                            case "2":
                                // The currency of the global account is set to dollar.
                                globalAccount.SetCurrency("USD");
                                currencySet = true;
                                break;
                            default:
                                Console.Write("\nOgiltigt val! Tryck \"ENTER\" och försök igen.");
                                Console.ReadKey();
                                break;
                        }
                    }
                    // The account is added to the customers list of accounts.
                    customer.CustomerAccounts.Add(globalAccount);

                    // Praises the customer upon opening a new account.
                    PrintAccountSuccess(globalAccount);

                    // Asks the customer if they want to make a deposit to the new account.
                    DoYouWantToDeposit(globalAccount);
                }
                // ... else the process is terminated.
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

        // Method that asks the customer if they want to proceed creating a new account.
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

        // Method that asks the customer if they want to make an deposit.
        public void DoYouWantToDeposit(Account account)
        {
            Console.Write("\nTryck [j] för att göra en insättning på kontot." +
                          "\nAnnars tryck valfri tangent för att återgå till föregående meny." +
                          "\n---" +
                          "\nDitt val: ");

            string answer = Console.ReadLine();
            if (answer.ToLower() == "j")
            {
                // If yes, the deposit is stored in the balance of the specific account.
                account.MakeADeposit(account);
            }
        }

        // Gives praise to the customer for opening a new account and displays details of the account.
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

        // Method that displays every account that a customer have. 
        public void PrintAccounts(bool displayGoBackMessage = true)
        {
            Console.Clear();
            Console.WriteLine(ascii.Header());

            // Checks if the customer have any accounts.
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

        // Method that checks if a certain account have sufficient funds.
        public bool CheckFunds(Account account)
        {
            if (account.GetBalance() > 0)
            {
                return true;
            }
            return false;
        }

        // Method that prints the username, account number and account type of every registered customer in the bank.
        public void PrintEveryCustomersAccount(Customer loggedInCustomer)
        {
            Console.Clear();
            Console.WriteLine(ascii.Header());

            // Checks whether the list is empty or not.
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
                    // Making sure the customer trying to make an external 
                    // transactions doesn't make one to its own account.
                    if (account.CustomerName != loggedInCustomer.Username)
                    {
                        // Only the account number of the customers checking account should be visible.
                        if (!loggedInCustomer.CustomerAccounts.Exists(a => a.AccountNumber == account.AccountNumber) && account.Type == AccountType.Checking)
                        {
                            // For formatting reasons, a name longer than 16 characters are treated differently when printed to console.
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

        // Method that allows the customer to make a deposit to one of the accounts they have created.
        public void AccountDeposit()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ascii.Header());

                PrintAccounts(false);

                Console.Write("Ange kontonumret för det konto du önskar sätta in pengar på. " +
                            "\n(Skriv 0 för att avbryta processen och återgå till föregående meny)." +
                            "\nKontonummer: ");

                string accountNrToDeposit = Console.ReadLine().Trim();

                // A new temporary account is instantiated based on the entered account number.
                Account selectedAccount = CustomerAccounts.FirstOrDefault(account => account.AccountNumber.Trim() == accountNrToDeposit);

                // If the new, temporary account is not null (i.e. the entered account
                // number matched the account number of an actually account)...
                if (selectedAccount != null)
                {
                    Console.Clear();
                    Console.WriteLine(ascii.Header());

                    // The customer is allowed to make a deposit to the selected account.
                    selectedAccount.MakeADeposit(selectedAccount);

                    Console.WriteLine($"\n\nDin insättning till ditt {selectedAccount.GetAccountType(selectedAccount).ToLower()}: {selectedAccount.AccountNumber} har gått igenom." +
                                      $"\nDitt nya saldo är: {selectedAccount.Balance} {selectedAccount.Currency}.");
                    Console.Write("\nTryck \"ENTER\" för att återgå till föregående meny.");
                    Console.ReadKey();
                    break;
                }
                else if (accountNrToDeposit == "0")
                {
                    Console.WriteLine("\nDu har valt att avslutat processen att sätta in pengar." +
                                      "\nTryck \"ENTER\" för att återgå till föregående meny.");
                    Console.ReadKey();
                    break;
                }
                else
                {
                    Console.Write("\nOgiltigt kontonummer! Tryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                }
            }
        }

        // Method in which the customer decides which external account number to make an external transfer to.
        private void TransferToExternalAccount(Account transferAccount, Customer customer)
        {
            decimal transferAmount = 0;

            while (true)
            {
                // Displays the customers' own accounts.
                PrintAccounts();

                // Asking what amount the customer would like to transfer from their chosen account.
                Console.Write($"Hur mycket pengar vill du föra över från ditt {transferAccount.GetAccountType(transferAccount).ToLower()}?" +
                            $"\nBelopp: ");

                if (decimal.TryParse(Console.ReadLine(), out transferAmount))
                {
                    // Checks whether the account has sufficient funds or not.
                    if (transferAmount <= transferAccount.GetBalance())
                    {
                        // If there's enough funds we break out of 
                        // the while loop and go into the next one.
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
                // Displays the account number, and more, for every checking account from all available customers.
                PrintEveryCustomersAccount(customer);

                // The customer is prompted to enter the account number for the
                // desired account to which they wishes to transfer the money.
                Console.Write("\nAnge kontonumret för det konto du önskar föra över pengar till." +
                            "\nKontonr: ");

                string accountNrToDeposit = Console.ReadLine();

                // New temporary account is created based on the entered account number.
                var selectedAccount = Account.AllCustomerAccounts.Find(a => a.AccountNumber == accountNrToDeposit);

                // If the new, temporary account is not null (i.e. the entered
                // account number matched the account number of an actually account)...
                if (selectedAccount != null)
                {

                    // A withdraw is made to the customers own account.
                    transferAccount.Withdraw(transferAmount);

                    // The selected account receives the deposit.
                    selectedAccount.Deposit(transferAmount);

                    // Both transactions (debit & credit) are logged to the list of transactions.
                    Transaction f1 = new(transferAccount, transferAmount, "Överföring", true); // Debit
                    Transaction t2 = new(selectedAccount, transferAmount, "Överföring", false); // Credit

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

        // Method in which the customer choses which of their account the money should be transferred to.
        private void TransferToInternalAccount(Account transferAccount)
        {
            decimal transferAmount = 0;
            decimal originalTransferAmount = 0;

            while (true)
            {
                // Displaying the customers' own accounts.
                PrintAccounts();

                // Asking what amount the customer would like to transfer from their chosen account.
                Console.Write($"Hur mycket vill du föra över från {transferAccount.GetAccountType(transferAccount).ToLower()}t?" +
                              $"\nBelopp: ");

                if (decimal.TryParse(Console.ReadLine(), out originalTransferAmount))
                {
                    // Checks whether the account has sufficient funds or not.
                    if (originalTransferAmount <= transferAccount.GetBalance())
                    {
                        // If there's enough funds we break out of 
                        // the while loop and go into the next one.
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
                // Removes the account from which the customer wants to transfer from.
                // Just so it won't show up when the next accounts are displayed.                
                CustomerAccounts.Remove(transferAccount);

                // Displays the customers own accounts, except the one they want to  transfer from.
                // That's because they shouldn't be able to transfer back the money by mistake.                
                PrintAccounts();
                int counter = 1;
                // The customer is prompted to chose which account they want to transfer the money to.
                Console.WriteLine($"Välj det konto du vill föra över {originalTransferAmount} {transferAccount.Currency} till.\n");

                // A menu is presented from which the customer choses the account to transfer to.
                CustomerAccounts.ForEach(a => Console.Write($"[{counter++}] {a.GetAccountType(a)}\n"));
                Console.Write("---" +
                           "\nVälj konto: ");

                string accountChoice = Console.ReadLine();

                switch (accountChoice)
                {
                    case "1":
                        // Selects the first account.
                        var accountFirstIndex = CustomerAccounts.ElementAt(0);

                        // If the account's currency, from which the transfer is being sent from, doesn't match the
                        // target account's currency, the transfer amount will be converted into the right currency.
                        if (transferAccount.Currency != accountFirstIndex.Currency)
                        {
                            transferAmount = ExchangeManager.Exchange.CurrencyConverter(transferAccount.Currency, accountFirstIndex.Currency, originalTransferAmount);
                        }
                        else
                        {
                            transferAmount = originalTransferAmount;
                        }

                        // A withdraw is made to the customers own account.
                        transferAccount.Withdraw(originalTransferAmount);

                        // The selected account receives the deposit.
                        accountFirstIndex.Deposit(transferAmount);

                        Console.Clear();
                        Console.WriteLine(ascii.Header());

                        // A message is displayed telling the customer the transfer have gone through. 
                        Console.WriteLine($"Överföringen lyckades! Ditt nya saldo för {accountFirstIndex.GetAccountType(accountFirstIndex).ToLower()}t " +
                                          $"är {accountFirstIndex.GetBalance()} {accountFirstIndex.Currency}.");

                        // Both transactions (debit & credit) are logged to the list of transactions.
                        Transaction f1 = new(transferAccount, originalTransferAmount, "Överföring", true); // Debit
                        Transaction t1 = new(accountFirstIndex, originalTransferAmount, "Överföring", false); // Credit

                        transferComplete = true;
                        break;
                    case "2":
                        // Selects the second account, if it exists. 
                        var accountSecondIndex = CustomerAccounts.ElementAt(1);

                        // If the account's currency, from which the transfer is being sent from, doesn't match the
                        // target account's currency, the transfer amount will be converted into the right currency.
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

                        // A message is displayed telling the customer the transfer have gone through. 
                        Console.WriteLine($"Överföringen lyckades! Ditt nya saldo för {accountSecondIndex.GetAccountType(accountSecondIndex).ToLower()}t " +
                                          $"är {accountSecondIndex.GetBalance()} {accountSecondIndex.Currency}.");

                        // Both transactions (debit & credit) are logged to the list of transactions.
                        Transaction f2 = new(transferAccount, originalTransferAmount, "Överföring", true); // Debit
                        Transaction t2 = new(accountSecondIndex, originalTransferAmount, "Överföring", false); // Credit

                        transferComplete = true;
                        break;
                    default:
                        Console.Write("\nOgiltigt val! Tryck \"ENTER\" och försök igen.");
                        Console.ReadKey();
                        break;
                }
            }
            // The account from which the transfer was made is added back to the list.
            CustomerAccounts.Add(transferAccount);

            // The list is then reversed so that the order in which the
            // different accounts where added to the list is restored.
            CustomerAccounts.Reverse();

            Console.Write("\nTryck \"ENTER\" för att återgå till föregående meny.");
            Console.ReadKey();
        }

        // Method for the customer to make a internal transaction between their own accounts.
        public void InternalTransaction()
        {
            bool RunMenu = true;

            // Checks whether the customer have at least two different accounts.
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
                    // The customer is prompted to chose which account to make the transfer from.
                    string accountChoice = ChooseAccountForWithdraw();

                    switch (accountChoice)
                    {
                        // Based on the menu in the ChooseAccountForWithdraw()-method, the 
                        // input from the customer determents which account they have chosen. 
                        case "1":
                            // Selects the first account.
                            var accountFirstIndex = CustomerAccounts.ElementAtOrDefault(0);

                            // Making sure there are enough funds.
                            if (CheckFunds(accountFirstIndex))
                            {
                                // The method below is called, in which the customer
                                // choses the account to transfer their money.
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
                            // Selects the second account.
                            var accountSecondIndex = CustomerAccounts.ElementAtOrDefault(1);

                            // Making sure there are enough funds.
                            if (CheckFunds(accountSecondIndex))
                            {
                                // The method below is called, in which the customer
                                // choses the account to transfer their money.
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
                            // Selects the third account, if it exists.
                            var accountThirdIndex = CustomerAccounts.ElementAtOrDefault(2);

                            // If the account exists...
                            if (accountThirdIndex != null)
                            {
                                // ... we're making sure there are enough funds.
                                if (CheckFunds(accountThirdIndex))
                                {
                                    // The method below is called, in which the customer
                                    // choses the account to transfer their money.
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
                                // If the account doesn't exist the default-case is called.
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

        // Method for the customer to make a external transaction to another customer.
        public void ExternalTransaction(Customer customer)
        {
            bool RunMenu = true;

            // Checks whether the customer have at least one account to make the transfer.
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
                    // The customer is prompted to chose which account to make the transfer from.
                    string accountChoice = ChooseAccountForWithdraw();

                    switch (accountChoice)
                    {
                        // Based on the menu in the ChooseAccountForWithdraw()-method, the 
                        // input from the customer determents which account they have chosen. 
                        case "1":
                            // Selects the first account.
                            var accountFirstIndex = CustomerAccounts.ElementAtOrDefault(0);

                            // Making sure there are enough funds.
                            if (CheckFunds(accountFirstIndex))
                            {
                                // The method below is called, in which the customer
                                // choses the account to transfer their money.
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
                            // Selects the second account, if it exists.
                            var accountSecondIndex = CustomerAccounts.ElementAtOrDefault(1);

                            // If the account exists...
                            if (accountSecondIndex != null)
                            {
                                // ... we're making sure there are enough funds.
                                if (CheckFunds(accountSecondIndex))
                                {
                                    // The method below is called, in which the customer
                                    // choses the account to transfer their money.
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
                                // If the account doesn't exist the default-case is called.
                                goto default;
                            }
                            break;
                        case "3":
                            // Selects the third account, if it exists.
                            var accountThirdIndex = CustomerAccounts.ElementAtOrDefault(2);

                            // If the account exists...
                            if (accountThirdIndex != null)
                            {
                                // ... we're making sure there are enough funds.
                                if (CheckFunds(accountThirdIndex))
                                {
                                    // The method below is called, in which the customer
                                    // choses the account to transfer their money.
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
                                // If the account doesn't exist the default-case is called.
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

        // Method that prompts the user to chose which account to transfer 
        // money from. Returns a string carrying the customers answer.
        private string ChooseAccountForWithdraw()
        {
            PrintAccounts();
            int counter = 1;
            Console.WriteLine("Välj det konto du vill föra över pengar från.\n");            
            CustomerAccounts.ForEach(a => Console.Write($"[{counter++}] {a.GetAccountType(a)}\n"));
            Console.Write("---" +
                        "\n[0] Avsluta överföringen" +
                        "\n---" +
                        "\nVälj konto: ");

            string accountChoice = Console.ReadLine();
            return accountChoice;
        }

        // Method that allows the customer to take a loan from the bank.
        public void TakeLoan(Account account)
        {            
            double totalBalance = 0;
            double interest = 0.0848;
            double loanAmount = 0;            
            double loanTime = 0;
            decimal exchangeCurrency = 0;          
            double totalDebt = 0;
            double monthlyDebt = 0;
            bool loanCompleted = false;

            ExchangeManager exchange = new();
            
            Console.Clear();
            Console.WriteLine(ascii.Header());
            Console.WriteLine("Låneavdelningen");
            Console.WriteLine("===============");
            Console.Write($"\nVälkommen {FirstName} {LastName}! Vi erbjuder just nu annuitetslån till 8,48% ränta.\n" +
                          $"\nTryck \"ENTER\" för att förhandla om ett lån med oss.");
            Console.ReadKey();

            // Checks whether the customer have checking account.
            if (!CustomerAccounts.Any(a => a.Type == AccountType.Checking))
            {                
                Console.Write("Du har för närvarande inget aktivt lönekonto. Du kan öppna ett nytt konto i din startmeny.\n" +
                            "\nTryck \"ENTER\" för att återgå till föregående meny.");
                Console.ReadKey();
                return;
            }
            else                
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine(ascii.Header());

                    Console.WriteLine("Kontonr.\tSaldo\t\tValuta\tLåneskuld" +
                        "\n=================================================");

                    // Here are the customers accounts being displayed along with details for the 
                    // account number, balance, currency and if they already have a debt to the bank.
                    foreach (var item in CustomerAccounts)
                    {
                        totalBalance = 0;

                        if (item.Debt == 0)
                        {
                            Console.Write($"{item.AccountNumber}\t{item.Balance}\t\t{item.Currency}\t{item.Debt:-}");
                        }
                        else
                        {
                            if (item.Type == AccountType.Checking)
                            {
                                Console.Write($"{item.AccountNumber}\t{item.Balance}\t\t{item.Currency}\t{item.Debt}\n");
                            }
                            else
                            {
                                Console.Write($"{item.AccountNumber}\t{item.Balance}\t\t{item.Currency}\t-");
                            }
                        }
                        Console.WriteLine();

                        // Checks whether a account have a different currency and if so, 
                        // the balance of that account is then converted into SEK.
                        if (item.Currency == "USD")
                        {
                            // Converts dollar to SEK.
                            exchangeCurrency = exchange.CurrencyConverter("USD", "SEK", item.Balance);

                            // Every balance from each account is added together and assigned to totalBalance.
                            totalBalance = totalBalance + Convert.ToDouble(exchangeCurrency) - Convert.ToDouble(item.Debt);
                        }
                        else if (item.Currency == "EUR")
                        {
                            // Converts euro to SEK.
                            exchangeCurrency = exchange.CurrencyConverter("EUR", "SEK", item.Balance);

                            // Every balance from each account is added together and assigned to totalBalance.
                            totalBalance = totalBalance + Convert.ToDouble(exchangeCurrency) - Convert.ToDouble(item.Debt);
                        }
                        else
                        {
                            exchangeCurrency = 0;

                            // Every balance from each account is added together and assigned to totalBalance.
                            totalBalance = totalBalance + Convert.ToDouble(item.Balance) - Convert.ToDouble(item.Debt);
                        }
                    }

                    Console.WriteLine("=================================================");

                    // The customers total balance is printed out along with how much money 
                    // the customer can loan (max five times the amount of total balance). 
                    Console.Write($"\nDitt totala saldo exklusive lån är {totalBalance} kr." +
                                  $"\nDetta betyder att du max kan låna {totalBalance * 5} kr." +
                                  "\n\nHur mycket vill du låna? Skriv in 0 för att avbryta processen." +
                                  "\nDitt belopp: ");
                    
                    string userInput = Console.ReadLine();

                    if (userInput == "0")
                    {
                        Console.Write("\nDu har valt att avsluta låneprocessen. " +
                            "\nTryck \"ENTER\" för att återgå till föregående meny.");
                        Console.ReadKey();
                        return;
                    }
                    else if (double.TryParse(userInput, out loanAmount))
                    {
                        // Checks whether the amount don't exceed the allowed loan limit.
                        if (loanAmount <= totalBalance * 5 && loanAmount != 0)
                        {
                            while (!loanCompleted)
                            {
                                Console.Clear();
                                Console.WriteLine(ascii.Header());
                                Console.WriteLine($"Du har valt att låna {loanAmount:c} till 8,48% ränta.");

                                // The customer is prompted to decide how long the 
                                // time for installment should be (max 10 years).
                                Console.Write("\nHur lång avbetalningstid vill du ha? (1-10 år)." +
                                              "\nAntal år: ");
                                userInput = Console.ReadLine();

                                // Checks whether the customer entered the right value. If so the loan have been completed.
                                if (double.TryParse(userInput, out loanTime) && loanTime <= 10)
                                {
                                    double totalPayments = loanTime * 12;
                                    double interestRate = interest / 12;

                                    // Installment per month.
                                    monthlyDebt = (loanAmount * interestRate) / (1 - Math.Pow(1 + interestRate, totalPayments * -1));

                                    // Total debt.
                                    totalDebt = monthlyDebt * totalPayments;

                                    loanCompleted = true;
                                }
                                else
                                {
                                    Console.Write("\nOgiltig inmatning! Du har angett felaktigt antal år." +
                                        "\nTryck \"ENTER\" och försök igen.");
                                    Console.ReadKey();
                                }
                            }

                            Console.Clear();
                            Console.WriteLine(ascii.Header());

                            // The loan have been cleared and info regarding the payment plan is displayed.
                            Console.WriteLine("Grattis! Ditt lån har blivit godkänt och pengarna har förts över till ditt lönekonto.\n" +
                                "\nDin totala skuld är " + Math.Round(totalDebt, 2) + " kr." +
                                "\nDin totala månadskostnad kommer att vara " + Math.Round(monthlyDebt, 2) + " kr/månad i " + loanTime * 12 + " månader.");
                           
                            decimal loan = Convert.ToDecimal(loanAmount);

                            // The transactions for the loan is being logged.
                            Transaction t1 = new(account, loan, "Lån", false);
                           
                            // The loan is stored in the property Debt.
                            account.Debt += Convert.ToDecimal(loanAmount);

                            // The total amount the bank is loaning the customer 
                            // is being deposit in the customers bank account.
                            account.Deposit(loan);

                            // This message shows the customer the total sum of their account on which the loan is being deposit.
                            Console.WriteLine($"Totalsumman på ditt lönekonto: {account.Balance} kr.");

                            Console.Write("\nTryck \"ENTER\" för att återgå till föregående meny.");
                            Console.ReadKey();
                            break;
                        }
                        else
                        {
                            Console.Write("\nDu kan inte låna så mycket pengar. Vänligen försök med en lägre summa." +
                                "\nTryck \"ENTER\" och försök igen.");
                            Console.ReadKey();
                            Console.Clear();
                        }
                    }
                    else
                    {
                        Console.Write("\nOgiltig input! Tryck \"ENTER\" och försök igen.");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
        }

        // Method that allows the customer to see every transaction made in their accounts.
        public void PrintAllTransactions()
        {
            Console.Clear();
            Console.WriteLine(ascii.Header());
            Console.WriteLine($"{FirstName} {LastName} - Kontohistorik");

            foreach (var account in CustomerAccounts)
            {
                Console.WriteLine($"\nKonto\t\tValuta\tBelopp\t\tHändelse\tSaldo\t\tDatum");
                Console.WriteLine($"===================================================================================");
                account.PrintAccountHistory();
            }

            Console.Write("\nTryck \"ENTER\" för att återgå till föregående meny.");
            Console.ReadKey();
        }
    }
}