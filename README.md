# Logic Lions Bank: Project overview

### ConsoleIO Class

The ConsoleIO class serves as the user interface. It displays all menus, keeps track of which user in logged in and calls all menu methods that interact with the application.

**Key Methods**

*DisplayMainMenu():* Displays the login screen. Acts as the entry point for users.

*LogIn():* Handles user login, including three attempts and account blocking for security. Distinguishes between customer and admin logins, directing to respective menus. 
Integrates with *LogInManager*, that handles user authentication, login attempts, and user-related operations.

*DisplayCustomerMenu():* Displays the customer menu with various banking options.
Allows customers to view accounts, perform transactions, open new accounts, and change password.

*DisplayTransactionsMenu():* Displays menu for internal and external transactions.

*DisplayAccountMenu():* Displays menu for opening checking, savings, or global accounts.

*DisplayAdminMenu():* Displays admin menu. Admins can add users, view and delete users, unblock accounts, set exchange rates, and change passwords.

*inloggedCustomer:* An instance to keep track which user is logged in.

*inloggedAdmin:* Gives acces to admin related stuff.

----------

### Account Class

The Account class forms the core structure for managing bank accounts. It introduces the concept of different account types through the *AccountType enumeration*, which includes values such as *Checking*, *Savings*, and *Global*. Each account instance created from this class is associated with a specific account type.
It encapsulates important properties like *AccountNumber, deposit, Balance, Currency* and others.
The class manages actions such as deposits, withdrawals, and provides functionality for managing transaction history.
In the Customer class, methods like *NewCheckingAccount*, *NewSavingsAccount*, and *NewGlobalAccount* use the Account class to create instances of different account types for customers. These methods in turn creates checking accounts, savings accounts, and global accounts.

**Key Methods**

*GenerateAccountNumber():* Generates a random account number.
*SetCurrency(string currency):* Sets the currency for the account.
*Deposit (decimal amount):* Performs a deposit on the account.
*Withdraw (decimal amount):* Performs a withdrawal from the account.
*MakeADeposit (Account account):* Manages deposits with user interaction.

**SavingsAccount Class**

Inheriting from the Account class, the SavingsAccount specifically represents savings accounts. It introduces a new property for interest rates and methods to choose and calculate interest. This class provides functionality for users to manage their savings.

**Key Methods**

*InterestChoice():* Allows users to choose the interest rate.
*CalcInterest():* Calculates and displays interest increases.

----------

### User Class

The User class encompassing essential features related to account access and security. The class facilitates user authentication, password management, and security-related functionalities within the application.

**Key Methods**

*GetUsername():* Returns the username of the user.

*Block()/Unblock():* Sets the IsBlocked property to true, blocking the user, or false allowing the user to access the system.

*VerifyNewUsername(string username):* Verifies if a new username meets specific criteria, such as length and absence of certain characters.

*ChangePassword():* Used to change the user password.

*VerifyNewPassword(string password):* Verifies if a new password meets specific criteria, such as length, uppercase letters, digits, and special characters.

*CheckPassword(string password):* Checks if the provided password matches the user's current password.

*CheckValidChar(string input):* Checks if a given input contains specific special characters.

----------

### Admin Class

Admin is inheriting from the base User class. It simply holds a constructor that sets *IsAdmin* to true.
It's used inside ConsoleIO for gaining access to all admin related stuff.

----------

### Customer Class

The Customer class is inheriting from the base User class. The class encapsulates customer information, such as first and last name, and manages various banking functionalities. The purpose of this class is to facilitate customer interactions with the banking system, including creating accounts, managing transactions, and accessing account information.

**Key Features and Methods**

Account Creation:
The class allows customers to create different types of accounts, such as checking, savings, and global accounts. Methods like *NewCheckingAccount*, *NewSavingsAccount*, and *NewGlobalAccount* guide customers through the account creation process, including setting currency and handling potential duplicate accounts.

Transaction Handling:
Customers can perform various transactions, including making deposits (*DoYouWantToDeposit*), viewing account information (*PrintAccounts*), and making internal transfers *(InternalTransaction)* between their own accounts.

The class supports external transactions *(ExternalTransaction)* for transferring funds from the customer's account to another customer's account. This involves checking account balances, validating user input, and executing the transfer.

Loan Management:
Customers can take loans from the bank using the *TakeLoan()* method. This involves specifying the loan amount, choosing a repayment period, calculating interest, and updating account balances.

Account Information and History:
Methods like *GetUserAccounts* provide a list of all accounts associated with the customer, and *PrintAllTransactions* displays the transaction history for each account, offering a comprehensive overview of the customer's financial activities.

----------

### ExchangeManager Class

The ExchangeManager class is used for managing currency exchange rates and handles currency conversion. It maintains a dictionary of currencies and their corresponding exchange rates, allowing the admin to set and update these rates. The class implements a singleton pattern to ensure a single instance throughout the application.

Key Methods:

*ExchangeManager():* The constructor sets the default exchange rates for EUR, USD, and SEK, when the application starts.

*Exchange (property):* Singleton property providing access to the ExchangeManager instance, creating it if it doesn't exist.

*SetCurrencies():* Allows admin to update the exchange rates for EUR and USD.

*CurrencyConverter():* Converts a specified amount from a source currency to a target currency based on stored exchange rates.

*CurrencyConvertSummary():* Converts and summarizes the conversion of an amount between source and target currencies, displaying exchange rates and the result.

*GetUserDecimalInput():* Prompts users for valid decimal input within a specified range, ensuring accurate exchange rates are provided for currency conversion.

----------

### Transaction Class

The Transaction class is responsible for keeping a records of all transactions for every corresponding account, and saves them in a unique list. It encapsulates details such as *transaction type, amount, date, balance, currency.* The *IsDebet* property is used to indicate whether the transaction is a debit (true) or credit (false).

Whenever a transaction is happening in the application, the constructor is run and takes *source account, transaction amount* and *IsDebet* as argument. Once instantiated, the transaction(object) gets added to the AccountHistory list.

----------

### AsciiArt Class

Holds the header/logo

----------

### Program Class

The Program class initialises the app and loads the login method.
Also makes an instance of ConsoleIO

### UML

Link to the UML project 
https://www.figma.com/file/eK8xe3MsPgWl2qQbtigFJe/Whiteboard?type=whiteboard&node-id=0%3A1&t=CrDfGxKSBu8QMMPC-1



