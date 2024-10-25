using System;

namespace BankAccountApp.Models
{
    public class Transaction
    {
        public DateTime Date { get; }
        public string TransactionId { get; }
        public string Type { get; }
        public decimal Amount { get; }

        public Transaction(DateTime date, string transactionId, string type, decimal amount)
        {
            Date = date;
            TransactionId = transactionId;
            Type = type;
            Amount = amount;
        }
    }
}
