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

        //
        public Transaction(Account sourceAccount, decimal transactionAmount,
            string transactionType, bool isDebet)
        {
            if (isDebet) { transactionAmount *= -1; }

            SourceAcc = sourceAccount.GetAccountType(sourceAccount);
            SourceAccNumber = sourceAccount.AccountNumber;
            TransactionAmount = transactionAmount;
            TransactionType = transactionType;
            TransactionDate = DateTime.Now.ToString("yyyy/mm/dd");

            // Loggar transaktionen till AccountHistory
            sourceAccount.GetAccountHistory().Add(this);
        }
    }
}
