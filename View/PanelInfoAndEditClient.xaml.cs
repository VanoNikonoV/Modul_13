using Modul_13.Cmds;
using Modul_13.Commands;
using Modul_13.Models;
using Modul_13.ViewModels;
using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace Modul_13.View
{
    /// <summary>
    /// Логика взаимодействия для PanelInfoAndEditClient.xaml
    /// </summary>
    public partial class PanelInfoAndEditClient : Page
    {
        /// <summary>
        /// AccessLevel определяется на основании выбраного параметра в элементе ListView "DataClients"
        /// принадлежащего MainWindow
        /// </summary>
        public MainWindow MWindow { get => (MainWindow)Application.Current.MainWindow; }

        /// <summary>
        /// Уровень доступа к базе данных для консультанта и менаджера, 
        /// определяется на основании выбраного параметра в элементе ComboBox "AccessLevel_ComboBox"
        /// принадлежащего MainWindow
        /// </summary>
        public int AccessLevel
        {
            get
            {
                int? index = MWindow.AccessLevel_ComboBox.SelectedIndex;

                int s = index ?? 0;

                return s;
            }
        }

        private ClientsRepository clientsRepository;

        /// <summary>
        /// Позволяет получить и изменить базу данных с клиентами
        /// </summary>
        public ClientsRepository ClientsRepository
        {
            get => clientsRepository;

            private set
            {
                if (clientsRepository == value) return;
                clientsRepository = value;
            }
        }

        public Client CurrentClient { get => this.DataContext as Client; }
        public Consultant Consultant { get; }
        public Meneger Meneger { get; }

        public PanelInfoAndEditClient()
        {
            InitializeComponent();

            this.Consultant = new Consultant();

            this.Meneger = new Meneger();

            this.ClientsRepository = MWindow.ViewModel.ClientsRepository;

        }

        #region Команды

        private RelayCommand<string> _editTelefonCommand = null;
        public RelayCommand<string> EditTelefonCommand
            => _editTelefonCommand ?? (_editTelefonCommand = new RelayCommand<string>(EditTelefon, CanEditTelefon));


        private RelayCommand<string> editNameCommand = null;
        public RelayCommand<string> EditNameCommand =>
            editNameCommand ?? (editNameCommand = new RelayCommand<string>(EditName, CanEdit));


        private RelayCommand<string> editMiddleNameCommand = null;
        public RelayCommand<string> EditMiddleNameCommand =>
            editMiddleNameCommand ?? (editMiddleNameCommand = new RelayCommand<string>(EditMiddleName, CanEdit));


        private RelayCommand<string> editSecondNameCommand = null;
        public RelayCommand<string> EditSecondNameCommand =>
            editSecondNameCommand ?? (editSecondNameCommand = new RelayCommand<string>(EditSecondName, CanEdit));


        private RelayCommand<string> editSeriesAndPassportNumberCommand = null;
        public RelayCommand<string> EditSeriesAndPassportNumberCommand =>
            editSeriesAndPassportNumberCommand ?? (editSeriesAndPassportNumberCommand
            = new RelayCommand<string>(EditSeriesAndPassportNumber, CanEdit));

        //Команды для работы с депозитным счетом

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

        private RelayCommand addNoDepositCommand = null;
        /// <summary>
        /// Команда добавление Недпозитного счета для выбранного клиента
        /// </summary>
        public RelayCommand AddNoDepositCommand =>
            addDepositCommand ?? (addNoDepositCommand = new RelayCommand(AddNoDeposit, CanAddDeposit));

        #endregion

        /// <summary>
        /// Опреляет допускается ли редактировать номер телефона клиента
        /// </summary>
        /// <param name="args"></param>
        /// <returns>true - если для данного уровня доступа доступна возможность редактировани
        /// false - если для данного уровня доступа недостукается редактировани</returns>
        private bool CanEditTelefon(string telefon)
        {
            if (telefon != null && !String.IsNullOrWhiteSpace(telefon))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Опреляет допускается ли редактировать данные клиента
        /// </summary>
        /// <param name="args"></param>
        /// <returns>true - если для данного уровня доступа доступна возможность редактировани
        /// false - если для данного уровня доступа недостукается редактировани</returns>
        private bool CanEdit(string args)
        { 
            if (AccessLevel == 1 && CurrentClient != null
                && !String.IsNullOrWhiteSpace(args)
                && args != null)
            { return true; }

            else { return false; }
        }
        /// <summary>
        /// Метод редактирования имени клиента
        /// </summary>
        /// <param name = "client" ></ param >
        private void EditName(string newName)
        {  
            Client changedClient = Meneger.EditNameClient(CurrentClient, newName.Trim());

            if (changedClient.IsValid)
            {
                int index = clientsRepository.IndexOf(CurrentClient);

                clientsRepository.ReplaceClient(index, changedClient);
            }
        }
        private void EditTelefon(string telefon)
        {
            string whatChanges = string.Format(CurrentClient.Telefon + @" на " + telefon.Trim());

            //изменения в коллекции клиентов
            Client changedClient = Consultant.EditeTelefonClient(telefon, CurrentClient);

            if (changedClient.IsValid)
            {
                //изменения в коллекции банка, по ID клиента
                Client editClient = ClientsRepository.First(i => i.ID == CurrentClient.ID); // try

                editClient.Telefon = telefon.Trim();

                switch (this.AccessLevel)
                {
                    case 0: //консультант

                        editClient.InfoChanges.Add(new InformationAboutChanges(DateTime.Now, whatChanges, "замена", nameof(Consultant)));

                        break;

                    case 1: //менждер

                        editClient.InfoChanges.Add(new InformationAboutChanges(DateTime.Now, whatChanges, "замена", nameof(Meneger)));

                        break;

                    default:
                        break;
                }
            }
            else 
            {  
                MessageBox.Show(messageBoxText: changedClient.Error,
                                      caption: "Ощибка в данных",
                                     MessageBoxButton.OK,
                                    icon: MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Метод редактирования отчества клиента
        /// </summary>
        /// <param name = "client" ></ param >
        private void EditMiddleName(string middleName)
        {
            if (CurrentClient != null)
            {
                Client changedClient = Meneger.EditMiddleNameClient(CurrentClient, middleName.Trim());

                int index = ClientsRepository.IndexOf(CurrentClient);

                ClientsRepository.ReplaceClient(index, changedClient);
            }

        }

        private void EditSecondName(string secondName)
        {
            if (CurrentClient != null)
            {
                Client changedClient = Meneger.EditSecondNameClient(CurrentClient, secondName.Trim());

                int index = ClientsRepository.IndexOf(CurrentClient);

                ClientsRepository.ReplaceClient(index, changedClient);
            }
        }

        private void EditSeriesAndPassportNumber(string passport)
        {
            if (CurrentClient != null)
            {
                Client changedClient = Meneger.EditSeriesAndPassportNumberClient(CurrentClient, passport.Trim());

                int index = ClientsRepository.IndexOf(CurrentClient);

                ClientsRepository.ReplaceClient(index, changedClient);
            }
        }

        #region Методы для работы со счетами
        /// <summary>
        /// Проверка наличия открытого счета у клиента
        /// </summary>
        /// <returns>
        /// true - если у выбранного клиента отрыт счет
        /// false - если счет не открыт</returns></returns>
        private bool CanCloseDeposit()
        {
            //var Client = AccountsRepo.Select(i => i.Owner);

            //return Client.Contains(CurrentClient);
            return true;
        }

        private void CloseDeposit()
        {
            //if (this.CurrentAccount == null)
            //{
            //    AccountsRepo.Remove(CurrentAccount);
            //}
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
            //var Client = AccountsRepo.Select(i => i.Owner);

            //return !Client.Contains(CurrentClient);
            return true;
        }
        /// <summary>
        /// Добавление счета для выбранного клиента
        /// </summary>
        private void AddDeposit()
        {
            InterestEarningAccount account = new InterestEarningAccount(CurrentClient, 10);

            //AccountsRepo.Add(account);

            // this.MWindow.Name_TextBox.Text = this.CurrentAccount.Owner.FirstName;
        }
        #endregion

    }
}
