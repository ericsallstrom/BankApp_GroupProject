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
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool IsDebet { get; set; }

        public Transaction(Account sourceAccount, decimal transactionAmount, bool isDebet)
        {
            if (isDebet) { transactionAmount *= -1; }

            SourceAcc = sourceAccount?.AccType ?? "Unknown";
            SourceAccNumber = sourceAccount.AccountNumber;
            TransactionAmount = transactionAmount;
            TransactionDate = DateTime.Now;

            // Loggar transaktionen till AccountHistory
            sourceAccount.GetAccountHistory().Add(this);
        }

        public void LogTransaction()
        {
        }
    }
}
