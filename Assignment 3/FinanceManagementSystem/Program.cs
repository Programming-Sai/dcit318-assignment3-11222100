using System;
using System.Collections.Generic;

namespace FinanceManager
{
    // 🌟 1. Immutable Transaction Model using 'record'
    public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

    // 💸 2. Transaction Processor Interface
    public interface ITransactionProcessor
    {
        void Process(Transaction transaction);
    }

    // 🏦 3a. Bank Transfer Processor
    public class BankTransferProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[BankTransfer] Processing ${transaction.Amount} for '{transaction.Category}' on {transaction.Date.ToShortDateString()}");
            Console.ResetColor();
        }
    }

    // 📱 3b. Mobile Money Processor
    public class MobileMoneyProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[MobileMoney] Paid ${transaction.Amount} - Category: {transaction.Category}");
            Console.ResetColor();
        }
    }

    // 🪙 3c. Crypto Wallet Processor
    public class CryptoWalletProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"[CryptoWallet] - Sent {transaction.Amount} in crypto for '{transaction.Category}' 🪙");
            Console.ResetColor();
        }
    }

    // 🏦 4. Base Account Class
    public class Account
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; protected set; }

        public Account(string accountNumber, decimal startingBalance)
        {
            AccountNumber = accountNumber;
            Balance = startingBalance;
        }

        public virtual void ApplyTransaction(Transaction transaction)
        {
            Balance -= transaction.Amount;
        }

        public void ShowBalance()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n💰 Current Balance: ${Balance}");
            Console.ResetColor();
        }
    }

    // 🔒 5. Sealed SavingsAccount Class
    public sealed class SavingsAccount : Account
    {
        public SavingsAccount(string accountNumber, decimal startingBalance)
            : base(accountNumber, startingBalance) { }

        public override void ApplyTransaction(Transaction transaction)
        {
            if (Balance >= transaction.Amount)
            {
                Balance -= transaction.Amount;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"✔ Transaction of ${transaction.Amount} applied. New balance: ${Balance}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"✘ Transaction failed: Insufficient funds for ${transaction.Amount}");
            }
            Console.ResetColor();
        }
    }

    // 🚀 6. Main App Simulation
    public class FinanceApp
    {
        private List<Transaction> transactions = new List<Transaction>();

        public void Run()
        {
            PrintBanner();
            var account = new SavingsAccount("ACC001", 1000m);

            var t1 = new Transaction(1, DateTime.Now, 150m, "Groceries");
            var t2 = new Transaction(2, DateTime.Now, 500m, "Rent");
            var t3 = new Transaction(3, DateTime.Now, 400m, "Online Courses");

            var processors = new List<ITransactionProcessor>
            {
                new BankTransferProcessor(),
                new MobileMoneyProcessor(),
                new CryptoWalletProcessor()
            };

            var txList = new List<Transaction> { t1, t2, t3 };

            for (int i = 0; i < txList.Count; i++)
            {
                Console.WriteLine($"\n→ Processing Transaction #{txList[i].Id}: {txList[i].Category}");
                processors[i].Process(txList[i]);
                account.ApplyTransaction(txList[i]);
                transactions.Add(txList[i]);
            }

            account.ShowBalance();
            PrintTransactionHistory();
        }

        private void PrintBanner()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(@"
╔══════════════════════════════╗
║    💼 FINANCE SIMULATOR 💼   ║
╚══════════════════════════════╝
            ");
            Console.ResetColor();
        }

        private void PrintTransactionHistory()
        {
            Console.WriteLine("\n📜 Transaction History:");
            foreach (var tx in transactions)
            {
                Console.WriteLine($" - [{tx.Date.ToShortDateString()}] ${tx.Amount} for {tx.Category}");
            }
        }
    }

    // 🎯 Entry Point
    class Program
    {
        static void Main(string[] args)
        {
            var app = new FinanceApp();
            app.Run();
        }
    }
}
