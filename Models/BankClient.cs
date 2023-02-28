using System;
using System.ComponentModel;

namespace Modul_13.Models
{
    // List<T> Accounts; Сделать у клиента список с возмодными счетами
    public class BankClient<T> : INotifyPropertyChanged where T : Client 
        //BankClient<T> : INotifyPropertyChanged where T : BankAccount
    {
        /// <summary>
        /// Cведения о владельце счета
        /// </summary>
        public T Owner { get; set; }

        /// <summary>
        /// Депозитный счет
        /// </summary>
        public DepositAccount Deposit { get; set; }
        //public T Deposit { get; set; }

        /// <summary>
        /// Недепозитный счет
        /// </summary>
        public NoDepositAccount NoDeposit { get; set; }

        /// <summary>
        /// Конструктор клиента банка, с возможностью завести два счета
        /// </summary>
        /// <param name="bankClient">Базованя информация о клиенте</param>
        /// <param name="deposit">Депозитный счет</param>
        /// <param name="noDeposit">Не депозитный счет</param>
        public BankClient(T owner, DepositAccount deposit = null, NoDepositAccount noDeposit = null)
        {
            this.Owner = owner;
            this.Deposit = deposit;
            this.NoDeposit = noDeposit;
        }

        /// <summary>
        /// Открытие нового депозитного счета 
        /// </summary>
        /// <param name="initialBalance">Начальный баланс при открытии счета</param>
        /// <param name="minimumBalance">Минимальный баланс при открытии счета</param>
        public void AddDeposit(decimal initialBalance, decimal minimumBalance)
        {
            this.Deposit = new DepositAccount(initialBalance, minimumBalance);
            OnPropertyChanged(nameof(Deposit));
        }

        /// <summary>
        /// Перевод денежных средств на счет получателя
        /// </summary>
        /// <param name="recipient">Получатель платежа</param>
        /// <param name="amount">Сумма перевода</param>
        public void Transfer(BankClient<T> recipient, decimal amount)
        {
            if(recipient != this)
            {
                this.Deposit.MakeWithdrawal(amount, DateTime.Now, $"Списание {amount} в пользу клиента с:{recipient.Owner.FirstName}");

                recipient.Deposit.MakeDeposit(amount, DateTime.Now, $"Перевод {amount} от клиента с : {this.Owner.FirstName}");
            }
        }

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}
