﻿using Modul_13.Cmds;
using Modul_13.Commands;
using Modul_13.Models;
using Modul_13.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Modul_13.ViewModels
{
    public class PanelWorkingWithDepositViewModel:ViewModel
    {
        /// <summary>
        /// AccessLevel определяется на основании выбраного параметра в элементе ListView "DataClients"
        /// принадлежащего MainWindow
        /// </summary>
        public MainWindow MWindow { get; }

        //private ObservableCollection<BankAccount> accountsRepo;

        ///// <summary>
        ///// Коллекция счетов банка
        ///// </summary>
        //public ObservableCollection<BankAccount> AccountsRepo
        //{
        //    get { return accountsRepo; }

        //    private set
        //    {
        //        Set(ref accountsRepo, value, "AccountsRepo");
        //    }
        //}

        private BankAccount currentAccount = null;

        public BankAccount CurrentAccount 
        {
            get => currentAccount = this.MWindow.DataClients.SelectedItem as BankAccount;

            private set => currentAccount = value;
        }

        private BankRepository bankAccounts = null;

        public BankRepository BankRepository 
        { 
            get =>  this.MWindow.ViewModel.BankRepository; 
            set
            {
                Set(ref bankAccounts, value, "BankRepository");
            }
        }

        public PanelWorkingWithDepositViewModel(MainWindow window)
        {
            //accountsRepo= new ObservableCollection<BankAccount>();

            this.MWindow = window;
        }

        #region Команды для работы с депозитным счетом

        private RelayCommand addDepositCommand = null;
        /// <summary>
        /// Команда добавление ДЕПОЗИТНОГО счета для выбранного клиента 
        /// </summary>
        public RelayCommand AddDepositCommand =>
            addDepositCommand ?? (addDepositCommand = new RelayCommand(AddDeposit, CanAddDeposit));

        private RelayCommand closeDepositCommand = null;
        /// <summary>
        /// Команда закрытия ДЕПОЗИТНОГО счета для выбранного клиента
        /// </summary>
        public RelayCommand CloseDepositCommand =>
            closeDepositCommand ?? (closeDepositCommand = new RelayCommand(CloseDeposit, CanCloseDeposit));

        private RelayCommand<object> makeTransfer = null;

        public RelayCommand<object> MakeTransfer => makeTransfer ?? (makeTransfer = new RelayCommand<object>(Transfer, CanMakeTransfer));

        //private RelayCommand addNoDepositCommand = null;
        ///// <summary>
        ///// Команда добавление Недпозитного счета для выбранного клиента
        ///// </summary>
        //public RelayCommand AddNoDepositCommand =>
        //    addDepositCommand ?? (addNoDepositCommand = new RelayCommand(AddNoDeposit, CanAddDeposit));
        #endregion

        #region Методы для работы со счетами
        /// <summary>
        /// Проверка наличия открытого счета у клиента
        /// </summary>
        /// <returns>
        /// true - если у выбранного клиента открыт счет
        /// false - если счет не открыт</returns></returns>
        private bool CanCloseDeposit()
        {
            return CurrentAccount.Number != null ? true: false;

            //if(CurrentAccount.Number != null)

            //return true; else return false;
        } 
        /// <summary>
        /// Выполняет поиск клиента и в случаи совпадения удаляет счет
        /// </summary>
        private void CloseDeposit()
        {
           var Client = BankRepository.First(i => i == CurrentAccount);

           BankRepository.Remove(Client);  
            
           this.CurrentAccount = null;
        }
        /// <summary>
        /// Корректность исходных данных для выполнения перевода
        /// </summary>
        /// <param name="agrs"></param>
        /// <returns>false - если данные корректны
        ///          true - если нет данных</returns>
        private bool CanMakeTransfer(object agrs)
        {
            var array = agrs as object[];

            BankAccount client = array[0] as BankAccount;

            decimal sum;

            if (client != null && Decimal.TryParse(array[1].ToString(), out sum))
            {
                return true;
            }
            else { return false; }
        }
        private void AddNoDeposit()
        {
            //InterestNoEarningAccount account = new InterestEarningAccount(CurrentClient, 0);

            //AccountsRepo.Add(account);
        }

        /// <summary>
        /// Проверка наличия открытого счета у клиента
        /// </summary>
        /// <returns>false - если у выбранного клиента отрыт счет
        ///          true - если счет не открыт</returns>
        private bool CanAddDeposit()
        {
            return CurrentAccount.Number != null ? false : true;
        }
        /// <summary>
        /// Добавление счета для выбранного клиента
        /// </summary>
        private void AddDeposit()
        {
            InterestEarningAccount account = new InterestEarningAccount(CurrentAccount.Owner, 10);

            int index = BankRepository.IndexOf(CurrentAccount);

            BankRepository.Add(account);

            //BankRepository.Insert(index, account);

            //BankRepository.ReplaceClient(index, (BankAccount)account);

            //this.CurrentAccount = account;
        }

        private void Transfer(object agrs)
        {
            var array = agrs as object[];

            BankAccount client = array[0] as BankAccount;

            decimal sum;

            if (Decimal.TryParse(array[1].ToString(), out sum))
            {
                client.ReplenishAccount(sum, DateTime.Now, $"Перевод от клиента с ID: {CurrentAccount.Owner.ID}");

                CurrentAccount.MakeWithdrawal(sum, DateTime.Now, $"Списание в пользу клиента с ID:{client.Owner.ID}");
            }   
        }

        /// <summary>
        /// Вызывается при изменении выбора в списке DataClients, тем самым обновляет выбранный элемент в коллекции AccountsRepo
        /// </summary>
        /// <param name="clienChanged">Выбранный клиент</param>
        public void UpdateCurrentClient(BankAccount clienChanged)
        {
            this.CurrentAccount = BankRepository.FirstOrDefault(accont => accont == clienChanged);
        }


        //collectionView.MoveCurrentTo(workspace);
        #endregion
    }
}
