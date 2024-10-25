namespace BankAccountApp.Models
{
    public class Bank
    {
        private List<Account> accounts = new List<Account>();
        private List<InterestRule> interestRules = new List<InterestRule>();

        public void AddTransaction(DateTime date, string accountId, string type, decimal amount)
        {
            var account = accounts.FirstOrDefault(a => a.AccountId == accountId);
            if (account == null)
            {
                account = new Account(accountId);
                accounts.Add(account);
            }

            account.AddTransaction(date, type, amount);
        }

        public void AddInterestRule(DateTime date, string ruleId, decimal rate)
        {
            interestRules.RemoveAll(rule => rule.Date == date);
            interestRules.Add(new InterestRule(date, ruleId, rate));
        }

        public void PrintAccountTransactions(string accountId)
        {
            var account = accounts.FirstOrDefault(a => a.AccountId == accountId);            
            account.PrintTransactions();
        }

        public void PrintInterestRules()
        {
            Console.WriteLine("Interest rules:");
            Console.WriteLine("| Date     | RuleId | Rate (%) |");
            foreach (var rule in interestRules.OrderBy(r => r.Date))
            {
                Console.WriteLine($"| {rule.Date:yyyyMMdd} | {rule.RuleId} | {rule.Rate:F2} |");
            }
        }
             
        public void PrintAccountStatement(string accountId, DateTime date)
        {
              // Check if account exists
            var account = accounts.FirstOrDefault(a => a.AccountId == accountId);
            if (account == null)
            {
                Console.WriteLine($"Account {accountId} not found.");
                return;
            }

            // Get all transactions for the specified month and year
            var monthTransactions = account.GetTransactionsForMonth(date.Year, date.Month);

            if (monthTransactions.Count == 0)
            {
                Console.WriteLine($"No transactions for account {accountId} in {date:yyyyMM}.");
                return;
            }

            // Print the header
            Console.WriteLine($"Account: {accountId}");
            Console.WriteLine("| Date     | Txn Id      | Type | Amount | Balance |");

            // Calculate daily balances and apply interest
            decimal currentBalance = 0;
            decimal totalInterest = 0;
         
            int currentYear = date.Year;
            int currentMonth = date.Month;

            // Loop through the days of the month
            DateTime startDate = new DateTime(currentYear, currentMonth, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            for (DateTime day = startDate; day <= endDate; day = day.AddDays(1))
            {
                // Check for transactions on this day
                var dayTransactions = monthTransactions
                    .Where(txn => txn.Date.Date == day.Date)
                    .ToList();

                // Process each transaction
                foreach (var txn in dayTransactions)
                {
                    // Update balance based on transaction type
                    if (txn.Type == "D")
                    {
                        currentBalance += txn.Amount;
                    }
                    else if (txn.Type == "W")
                    {
                        currentBalance -= txn.Amount;
                    }

                    // Print transaction
                    Console.WriteLine($"| {txn.Date:yyyyMMdd} | {txn.TransactionId} | {txn.Type}     |  {txn.Amount:F2} | {currentBalance:F2} |");
                }

                // Get the applicable interest rules
                var applicableRules = interestRules
                    .Where(rule => rule.Date <= day)
                    .OrderBy(rule => rule.Date)
                    .ToList();

                var currentRule = applicableRules.LastOrDefault();

                // Calculate interest at the end of each day (using the current rule)
                if (currentRule != null)
                {
                    decimal dailyInterest = currentBalance * (currentRule.Rate / 100) / 365;
                    
                    totalInterest += dailyInterest;
                }
            }

            // Apply interest on the last day of the month
            if (totalInterest > 0)
            {
                Console.WriteLine($"| {endDate:yyyyMMdd} |             | I    | {totalInterest:F2} | {currentBalance + totalInterest:F2} |");
            }
        }


    }
}
