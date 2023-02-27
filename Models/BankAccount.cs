using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Modul_13.Models
{
    //https://learn.microsoft.com/ru-ru/dotnet/csharp/fundamentals/tutorials/oop
    //https://metanit.com/sharp/tutorial/3.29.php
    public class BankAccount : INotifyPropertyChanged
    {
        /// <summary>
        /// Номер счета
        /// </summary>
        public string Number { get; }

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

        /// <summary>
        /// Номер счета
        /// </summary>
        private static int accountNumberSeed = 0;

        private readonly decimal _minimumBalance;
        
        public BankAccount(decimal initialBalance) : this(initialBalance, 0) {  }

        /// <summary>
        /// Конструтор BankAccount 
        /// </summary>
        /// <param name="initialBalance">Начальный баланс при открытии счета</param>
        /// <param name="minimumBalance">Минимальный баланс при открытии счета</param>
        public BankAccount(decimal initialBalance, decimal minimumBalance)
        {
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
        /// Пополнение (открытие нового) счета, начальный баланс должен быть положительным
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
       
        /// <summary>
        /// Списание средст со счета, любой вывод не должен создавать отрицательный баланс
        /// </summary>
        /// <param name="amount">Сумма списания</param>
        /// <param name="date">Дата и время операции</param>
        /// <param name="note">Заметка об операции</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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
        
        /// <summary>
        /// Получение текстовых данных о транзакциях
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Операция с денежными средствами в конце каждого месяца
        /// </summary>
        public virtual void PerformMonthEndTransactions() {  }

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        #endregion
    }
}
