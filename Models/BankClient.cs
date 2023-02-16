using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul_13.Models
{
    public class BankClient : INotifyPropertyChanged
    {
        /// <summary>
        /// Cведения о владельце счета
        /// </summary>
        public Client Owner { get; set; }

        /// <summary>
        /// Депозитный счет
        /// </summary>
        public DepositAccount Deposit { get; set; }

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
        public BankClient(Client owner, DepositAccount deposit = null, NoDepositAccount noDeposit = null)
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
