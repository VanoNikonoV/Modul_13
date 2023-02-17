using Modul_13.Cmds;
using Modul_13.Commands;
using Modul_13.Models;
using Modul_13.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Modul_13.ViewModels
{
    public class PanelWorkingWithDepositViewModel : ViewModel
    {
        public MainWindow MWindow { get; }

        private BankClient currentClient;
        public BankClient CurrentClient
        {
            get => this.MWindow.DataClients.SelectedItem as BankClient;
            set { Set(ref currentClient, value, "CurrentClient"); }
        }

        private BankRepository bankRepository;
        public BankRepository BankRepository
        {
            get => bankRepository;
            private set
            {
                Set(ref bankRepository, value, "BankRepository");
            }
        }

        //private ObservableCollection<BankClient> onlyDepositRepository;
        //public ObservableCollection<BankClient> OnlyDepositRepository 
        //{

        //    get 
        //    {
        //        ObservableCollection<BankClient> temp = (ObservableCollection<BankClient>)BankRepository.Where(t => t.Deposit != null);
        //        onlyDepositRepository = temp as ObservableCollection<BankClient>;
        //        return onlyDepositRepository
        //    }

        //    set 
        //    {
        //        Set(ref onlyDepositRepository, value, "OnlyDepositRepository"); 
        //    }
        //}

        public PanelWorkingWithDepositViewModel()
        {
            this.MWindow =  Application.Current.MainWindow as MainWindow;

            this.BankRepository = MWindow.ViewModel.BankRepository;
           
        }

        #region Команды для работы с депозитным счетом

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
        /// Корректность исходных данных для выполнения перевода
        /// </summary>
        /// <param name="agrs"></param>
        /// <returns>false - если данные корректны
        ///          true - если нет данных</returns>
        private bool CanMakeTransfer(object agrs)
        {
            var array = agrs as object[];

            BankClient client = array[0] as BankClient;

            if (client.Deposit != null)
            {
                return true;
            }
            else { return false; }
        }
       

        private void Transfer(object agrs)
        {
            var array = agrs as object[];

            BankClient client = array[0] as BankClient;

            decimal sum;

            if (Decimal.TryParse(array[1].ToString(), out sum))
            {
                client.Transfer(CurrentClient, sum);
            }
        }
        #endregion
    }
}
