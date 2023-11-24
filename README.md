# BankApp_GroupProject

Account Class

The Account class forms the core structure for managing bank accounts. It introduces the concept of different account types through the AccountType enumeration, which includes values such as Checking, Savings, and Global. Each account instance created from this class is associated with a specific account type.
It encapsulates important properties like AccountNumber, deposit, Balance, Currency and others.
The class manages actions such as deposits, withdrawals, and provides functionality for managing transaction history.
In the Customer class, methods like NewCheckingAccount, NewSavingsAccount, and NewGlobalAccount use the Account class to create instances of different account types for customers. These methods in turn creates checking accounts, savings accounts, and global accounts.

Key Methods for the Account class:

GenerateAccountNumber(): Generates a random account number.
SetCurrency(string currency): Sets the currency for the account.
Deposit (decimal amount): Performs a deposit on the account.
Withdraw (decimal amount): Performs a withdrawal from the account.
MakeADeposit (Account account): Manages deposits with user interaction.

SavingsAccount Class: 

Inheriting from the Account class, the SavingsAccount specifically represents savings accounts. It introduces a new property for interest rates and methods to choose and calculate interest. This class provides functionality for users to manage their savings.

Key Methods for SavingsAccount:
InterestChoice(): Allows users to choose the interest rate.
CalcInterest(): Calculates and displays interest increases.

----------

Customer Class

The Customer class is inheriting from the base User class. The class encapsulates customer information, such as first and last name, and manages various banking functionalities. The purpose of this class is to facilitate customer interactions with the banking system, including creating accounts, managing transactions, and accessing account information.

Key Features and Methods:
Account Creation:
The class allows customers to create different types of accounts, such as checking, savings, and global accounts. Methods like NewCheckingAccount, NewSavingsAccount, and NewGlobalAccount guide customers through the account creation process, including setting currency and handling potential duplicate accounts.

Transaction Handling:
Customers can perform various transactions, including making deposits (DoYouWantToDeposit), viewing account information (PrintAccounts), and making internal transfers (InternalTransaction) between their own accounts.

The class supports external transactions (ExternalTransaction) for transferring funds from the customer's account to another customer's account. This involves checking account balances, validating user input, and executing the transfer.

Loan Management:
Customers can take loans from the bank using the TakeLoan method. This involves specifying the loan amount, choosing a repayment period, calculating interest, and updating account balances.

Account Information and History:
Methods like GetUserAccounts provide a list of all accounts associated with the customer, and PrintAllTransactions displays the transaction history for each account, offering a comprehensive overview of the customer's financial activities.

