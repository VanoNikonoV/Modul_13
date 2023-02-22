using Modul_13.Commands;
using Modul_13.Models;
using Modul_13.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Modul_13.ViewModels
{
    public class PanelWorkingWithDepositViewModel : ViewModel
    {
        /// <summary>
        /// Ссылка на главное окно
        /// </summary>
        public MainWindow MWindow { get; }
        /// <summary>
        /// Отправитель платежа
        /// </summary>
        private BankClient<Client> sender;
        /// <summary>
        /// Отправитель платежа
        /// </summary>
        public BankClient<Client> Sender
        {
            get => this.MWindow.DataClients.SelectedItem as BankClient<Client>;
            set { Set(ref sender, value); }
        }
        /// <summary>
        /// Получатель платежа
        /// </summary>
        private BankClient<Client> recipient;
        /// <summary>
        /// Получатель платежа
        /// </summary>
        public BankClient<Client> Recipient 
        { 
            get => recipient;
            set { Set(ref recipient, value); }  
        }
        /// <summary>
        /// Список клиентов банка
        /// </summary>
        private BankRepository bankRepository;
        /// <summary>
        /// Список клиентов банка
        /// </summary>
        public BankRepository BankRepository
        {
            get => bankRepository;
            private set
            {
                Set(ref bankRepository, value, "BankRepository");
            }
        }
        /// <summary>
        /// Список клиентов с открытым депозитным счетом счетом
        /// </summary>
        private IEnumerable<BankClient<Client>> onlyDepositRepository;
        /// <summary>
        /// Список клиентов с открытым депозитным счетом счетом
        /// </summary>
        public IEnumerable<BankClient<Client>> OnlyDepositRepository 
        { 
            get => onlyDepositRepository;
            set
            {
                Set(ref onlyDepositRepository, value, "OnlyDepositRepository");
            }
        }

        #region UI

        public TextBox SumTransfer { get; set; }

        #endregion

        public PanelWorkingWithDepositViewModel()
        {
            this.MWindow =  Application.Current.MainWindow as MainWindow;

            this.BankRepository = MWindow.ViewModel.BankRepository;
        }

        #region Команды для работы с депозитным счетом

        private RelayCommand<string> makeTransfer = null;

        public RelayCommand<string> MakeTransfer => makeTransfer ?? (makeTransfer = new RelayCommand<string>(TransferExecuted, CanMakeTransfer));

        //private RelayCommand addNoDepositCommand = null;
        ///// <summary>
        ///// Команда добавление Недпозитного счета для выбранного клиента
        ///// </summary>
        //public RelayCommand AddNoDepositCommand =>
        //    addDepositCommand ?? (addNoDepositCommand = new RelayCommand(AddNoDeposit, CanAddDeposit));
        #endregion

        #region Методы для работы со счетами
     
        /// <summary>
        /// Корректность исходных данных для выполнения перевода
        /// </summary>
        /// <param name="agrs"></param>
        /// <returns>false - если данные корректны
        ///          true - если нет данных</returns>
        private bool CanMakeTransfer(string sum)
        {
            if (sum.Length>0 && Recipient != null)
            {
                return true;

            }
            return false;  
        }

       /// <summary>
       /// Перевод денежных средств между счетами
       /// </summary>
       /// <param name="sum">Сумма перевода</param>
        private void TransferExecuted(string sum) // либо передать в метод сам TextBox, чтобы можно было скинуть в ноль свойство текст
        {
            decimal amount;

            if (Decimal.TryParse(sum, out amount))
            {
                this.Sender.Transfer(this.Recipient, amount);

                SumTransfer.Text = "";
            }
            else { MessageBox.Show("Нужно ввсети число"); }
        }
        /// <summary>
        /// Пополнить счет
        /// </summary>
        /// <param name="sum">Сумма пополнения</param>
        /// 
        /// Используя ковариантный интерфейс, реализуйте методы пополнения счёта по соответствующему типу.
        private void TopUpAccount(string sum)
        {
            decimal amount;

            if (Decimal.TryParse(sum, out amount))
            {
                this.Sender.Deposit.MakeDeposit(amount, DateTime.Now, $"Пополнение счета на {sum}");

                //SumTransfer.Text = "";
            }
            
        }

        #endregion
    }
}
