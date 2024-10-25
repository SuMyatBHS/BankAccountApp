using System;
using BankAccountApp.Models;

namespace BankAccountApp
{
    class Program
    {
        static Bank bank = new Bank();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to AwesomeGIC Bank! What would you like to do?");

            while (true)
            {
                Console.WriteLine("[T] Input transactions");
                Console.WriteLine("[I] Define interest rules");
                Console.WriteLine("[P] Print statement");
                Console.WriteLine("[Q] Quit");
                Console.Write("> ");

                string action = Console.ReadLine().ToUpper();

                switch (action)
                {
                    case "T":
                        InputTransaction();
                        break;
                    case "I":
                        DefineInterestRule();
                        break;
                    case "P":
                        PrintStatement();
                        break;
                    case "Q":
                         Console.WriteLine("Thank you for banking with AwesomeGIC Bank.\nHave a nice day!");
                        return;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
                Console.WriteLine("Is there anything else you'd like to do?");
            }
        }
 
        static void InputTransaction()
        {
            Console.WriteLine("Please enter transaction details in <Date> <Account> <Type> <Amount> format");
            Console.WriteLine("(or enter blank to go back to main menu):");
            var input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
                return;

            var parts = input.Split(' ');
            if (parts.Length != 4)
            {
                Console.WriteLine("Invalid input format. Please try again.");
                return;
            }

            DateTime date;
            if (!DateTime.TryParseExact(parts[0], "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out date))
            {
                Console.WriteLine("Invalid date format. Please use YYYYMMDD.");
                return;
            }

            string accountId = parts[1];
            string type = parts[2].ToUpper();
            decimal amount;
            if (!decimal.TryParse(parts[3], out amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount.");
                return;
            }

            try
            {
                bank.AddTransaction(date, accountId, type, amount);
                bank.PrintAccountTransactions(accountId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void DefineInterestRule()
        {
            Console.WriteLine("Please enter interest rules details in <Date> <RuleId> <Rate in %> format");
            Console.WriteLine("(or enter blank to go back to main menu):");
            var input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
                return;

            var parts = input.Split(' ');
            if (parts.Length != 3)
            {
                Console.WriteLine("Invalid input format. Please try again.");
                return;
            }

            DateTime date;
            if (!DateTime.TryParseExact(parts[0], "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out date))
            {
                Console.WriteLine("Invalid date format. Please use YYYYMMDD.");
                return;
            }

            string ruleId = parts[1];
            decimal rate;
            if (!decimal.TryParse(parts[2], out rate) || rate <= 0 || rate >= 100)
            {
                Console.WriteLine("Invalid rate.");
                return;
            }

            bank.AddInterestRule(date, ruleId, rate);
            bank.PrintInterestRules();
        }

        static void PrintStatement()
        {
            Console.WriteLine("Please enter account and month to generate the statement <Account> <Year><Month>");
            Console.WriteLine("(or enter blank to go back to main menu):");
            var input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
                return;

            var parts = input.Split(' ');
            if (parts.Length != 2)
            {
                Console.WriteLine("Invalid input format. Please try again.");
                return;
            }

            string accountId = parts[0];
            if (!DateTime.TryParseExact(parts[1] + "01", "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime date))
            {
                Console.WriteLine("Invalid year/month format. Please use YYYYMM.");
                return;
            }

            bank.PrintAccountStatement(accountId, date);
        }
    }
}


