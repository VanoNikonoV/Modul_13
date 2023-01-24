using System;
using System.Collections.Generic;

namespace Modul_13.Models
{
    //https://learn.microsoft.com/ru-ru/dotnet/csharp/fundamentals/tutorials/oop
    public class BankAccount
    {
        /// <summary>
        /// Номер счета
        /// </summary>
        public string Number { get; }
        /// <summary>
        /// Cведения о владельце счета
        /// </summary>
        public Client Owner { get; set; }
        /// <summary>
        /// Вычесляет сальдо на основании журнала транзакций 
        /// </summary>
        public decimal Balance
        {
            get 
            {
                decimal balance = 0;
                
                foreach (var item in _allTransactions) 
                { 
                    balance+= item.Amount;
                }
                return balance; 
            }      
        }

        private static int accountNumberSeed = 0;

        private readonly decimal _minimumBalance;

        public BankAccount(Client owner, decimal initialBalance) : this(owner, initialBalance, 0)
        {

        }

        public BankAccount(Client owner, decimal initialBalance, decimal minimumBalance)
        {
            this.Owner = owner;

            this.Number = accountNumberSeed.ToString();
            accountNumberSeed++;

            _minimumBalance = minimumBalance;
            if (initialBalance > 0)
                MakeDeposit(initialBalance, DateTime.Now, "Начальный баланс");
        }

        /// <summary>
        /// Журнал для каждой транзакции по счету для аудита всех транзакций и управления ежедневным сальдо
        /// </summary>
        private List<Transaction> _allTransactions = new List<Transaction>();
        /// <summary>
        /// Открытие нового счета
        /// </summary>
        /// <param name="amount">Начальный баланс</param>
        /// <param name="date">Дата и время создания</param>
        /// <param name="note">Примечание</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void MakeDeposit(decimal amount, DateTime date, string note)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Сумма депозита должна быть положительной");
            }

            var deposit = new Transaction(amount, date, note);

            _allTransactions.Add(deposit);
        }

        //public void MakeWithdrawal(decimal amount, DateTime date, string note)
        //{
        //    if (amount <= 0)
        //    {
        //        throw new ArgumentOutOfRangeException(nameof(amount), "Amount of withdrawal must be positive");
        //    }

        //    if (Balance - amount < _minimumBalance)
        //    {
        //        throw new InvalidOperationException("Недостаточно средств для этого вывода");
        //    }

        //    var withdrawal = new Transaction(-amount, date, note);

        //    allTransactions.Add(withdrawal);
        //}

        public void MakeWithdrawal(decimal amount, DateTime date, string note)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Сумма вывода должна быть положительной");
            }
            Transaction overdraftTransaction = CheckWithdrawalLimit(Balance - amount < _minimumBalance);
            Transaction withdrawal = new Transaction(-amount, date, note);
            _allTransactions.Add(withdrawal);
            if (overdraftTransaction != null)
                _allTransactions.Add(overdraftTransaction);
        }

        protected virtual Transaction CheckWithdrawalLimit(bool isOverdrawn)
        {
            if (isOverdrawn)
            {
                throw new InvalidOperationException("Недостаточно средств для этого вывода");
            }
            else
            {
                return default;
            }
        }
        public string GetAccountHistory()
        {
            var report = new System.Text.StringBuilder();

            decimal balance = 0;

            report.AppendLine("Date\t\tAmount\tBalance\tNote");

            foreach (var item in _allTransactions)
            {
                balance += item.Amount;

                report.AppendLine($"{item.Date.ToShortDateString()}\t{item.Amount}\t{balance}\t{item.Notes}");

            }
            return report.ToString();
        }

        public virtual void PerformMonthEndTransactions()
        {

        }
    }
}
