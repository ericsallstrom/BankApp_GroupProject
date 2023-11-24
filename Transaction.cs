using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp_GroupProject
{
    public class Transaction
    {
        public string SourceAccNumber { get; set; }
        public string SourceAcc { get; set; }
        public AccountType Type { get; set; }
        public decimal TransactionAmount { get; set; }
        public string TransactionType { get; set; }
        public string TransactionDate { get; set; }
        public bool IsDebet { get; set; }
        public decimal SourceAccBalance { get; set; }
        public string SourceCurrency { get; set; }

        // Constructor for the Transaction-class 
        public Transaction(Account sourceAccount, decimal transactionAmount,
                           string transactionType, bool isDebet)
        {
            // When a new transaction is being logged from a account, 
            // the debit shows the amount of money with a minus sign.
            if (isDebet) { transactionAmount *= -1; }

            SourceAcc = sourceAccount.GetAccountType(sourceAccount);
            SourceAccNumber = sourceAccount.AccountNumber;
            TransactionAmount = transactionAmount;
            TransactionType = transactionType;
            TransactionDate = DateTime.Now.ToShortDateString().ToString();
            SourceAccBalance = sourceAccount.Balance;
            SourceCurrency = sourceAccount.Currency;

            // Logs the transaction to AccountHistory.
            sourceAccount.GetAccountHistory().Add(this);
        }
    }
}
