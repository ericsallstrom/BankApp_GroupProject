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
