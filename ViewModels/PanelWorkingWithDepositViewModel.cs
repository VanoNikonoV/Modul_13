using Modul_13.Cmds;
using Modul_13.Models;
using Modul_13.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;

namespace Modul_13.ViewModels
{
    public class PanelWorkingWithDepositViewModel:ViewModel
    {
        /// <summary>
        /// AccessLevel определяется на основании выбраного параметра в элементе ListView "DataClients"
        /// принадлежащего MainWindow
        /// </summary>
        public MainWindow MWindow { get; }

        private ObservableCollection<BankAccount> accountsRepo;

        /// <summary>
        /// Коллекция счетов банка
        /// </summary>
        public ObservableCollection<BankAccount> AccountsRepo
        {
            get { return accountsRepo; }

            private set
            {
                Set(ref accountsRepo, value, "AccountsRepo");
            }
        }

        public Client CurrentClient { get => this.MWindow.DataClients.SelectedItem as Client; }

        public PanelWorkingWithDepositViewModel(MainWindow window)
        {
            accountsRepo= new ObservableCollection<BankAccount>();

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
            var Client = AccountsRepo.Select(i => i.Owner);

            return Client.Contains(CurrentClient);
        } 
        /// <summary>
        /// Выполняет поиск клиента и в случаи совпадения удаляет счет
        /// </summary>
        private void CloseDeposit()
        {
           var Client = AccountsRepo.First(i => i.Owner == CurrentClient);

           AccountsRepo.Remove(Client);   
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
            var Client = AccountsRepo.Select(i => i.Owner);

            return !Client.Contains(CurrentClient); 
        }
        /// <summary>
        /// Добавление счета для выбранного клиента
        /// </summary>
        private void AddDeposit()
        {
            InterestEarningAccount account = new InterestEarningAccount(CurrentClient, 10);

            AccountsRepo.Add(account);
        }
        #endregion
    }
}
