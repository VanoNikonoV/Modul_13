using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Modul_13.Models
{
    //https://learn.microsoft.com/ru-ru/dotnet/csharp/fundamentals/tutorials/oop
    public class BankAccount : INotifyPropertyChanged
    {
        /// <summary>
        /// Номер счета
        /// </summary>
        public string Number { get; }

        private Client owner;
        /// <summary>
        /// Cведения о владельце счета
        /// </summary>
        public Client Owner 
        {
            get => owner;
            set 
            { if (owner == value) return;
                owner = value;
                OnPropertyChanged(nameof(Owner));
            }
        }
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
        
        /// <summary>
        /// Создает акаунт клиента с без открытия счета
        /// </summary>
        /// <param name="owner">Информация о владельце счета</param>
        public BankAccount(Client owner)
        {
            this.Owner = owner;
        }
        public BankAccount(Client owner, decimal initialBalance) : this(owner, initialBalance, 0) {  }
        /// <summary>
        /// Конструтор BankAccount 
        /// </summary>
        /// <param name="owner">Данные о владельце счета</param>
        /// <param name="initialBalance">Начальный баланс при открытии счета</param>
        /// <param name="minimumBalance">Минимальный баланс при открытии счета</param>
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

        /// <summary>
        /// Пополнение счета 
        /// </summary>
        /// <param name="amount">Сумма перевода</param>
        /// <param name="date">Дата и время операции</param>
        /// <param name="note">Заметка об операции</param>
        public void ReplenishAccount(decimal amount, DateTime date, string note)
        {
            var transfer = new Transaction(amount, date, note);

            _allTransactions.Add(transfer);
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
        public virtual void PerformMonthEndTransactions()
        {

        }
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public bool Equals(BankAccount other)
        {
            if (this.Owner.FirstName == other.Owner.FirstName
                && this.Owner.SecondName == other.Owner.SecondName
                && this.Owner.MiddleName == other.Owner.MiddleName
                && this.Owner.SeriesAndPassportNumber == other.Owner.SeriesAndPassportNumber
                && this.Owner.Telefon == other.Owner.Telefon)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
