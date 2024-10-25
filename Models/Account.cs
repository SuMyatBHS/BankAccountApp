using System;
using System.Collections.Generic;
using System.Linq;

namespace BankAccountApp.Models
{
    public class Account
    {
        public string AccountId { get; }
        public List<Transaction> transactions = new List<Transaction>();
        private decimal balance = 0;

        public Account(string accountId)
        {
            AccountId = accountId;
        }

        public void AddTransaction(DateTime date, string type, decimal amount)
        {
            if (type == "W" && balance < amount)
                throw new InvalidOperationException("Insufficient balance for withdrawal.");

            string transactionId = GenerateTransactionId(date);
            transactions.Add(new Transaction(date, transactionId, type, amount));

            if (type == "D")
                balance += amount;
            else
                balance -= amount;
        }

        public void PrintTransactions()
        {
            Console.WriteLine($"Account: {AccountId}");
            Console.WriteLine("| Date     | Txn Id      | Type | Amount |");
            foreach (var txn in transactions)
            {
                Console.WriteLine($"| {txn.Date:yyyyMMdd} | {txn.TransactionId} | {txn.Type} | {txn.Amount:F2} |");
            }
        }

        private string GenerateTransactionId(DateTime date)
        {
            int count = transactions.Count(t => t.Date == date) + 1;
            return $"{date:yyyyMMdd}-{count:D2}";
        }
        
        public List<Transaction> GetTransactionsForMonth(int year, int month)
        {
            return transactions
                .Where(txn => txn.Date.Year == year && txn.Date.Month == month)
                .OrderBy(txn => txn.Date)
                .ToList();
        }
    }
}
