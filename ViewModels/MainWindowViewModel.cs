﻿using Modul_13.Cmds;
using Modul_13.Commands;
using Modul_13.Models;
using Modul_13.View;
using Modul_13.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Modul_13.ViewModels
{
    public class MainWindowViewModel:ViewModel
    {
        #region Свойства
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

        private ClientsRepository clientsRepository;

        /// <summary>
        /// Позволяет получить и изменить базу данных с клиентами
        /// </summary>
        public ClientsRepository ClientsRepository
        {
            get => clientsRepository;

            private set
            {
                Set(ref clientsRepository, value, "ClientsRepository");
            }
        }

        /// <summary>
        /// Выбранный из базы ClientsRepository клиент
        /// определяется на основании выбраного параметра в элементе ListView "DataClients"
        /// принадлежащего MainWindow
        /// </summary>
        public MainWindow MWindow { get;}  

        public Client CurrentClient { get => this.MWindow.DataClients.SelectedItem as Client; }

        /// <summary>
        /// Информацию о счет для выбранного клиента
        /// </summary>
        public BankAccount CurrentAccount { get => AccountsRepo.FirstOrDefault(i => i.Owner == CurrentClient); }

        /// <summary>
        /// Уровень доступа к базе данных для консультанта и менаджера, 
        /// определяется на основании выбраного параметра в элементе ComboBox "AccessLevel_ComboBox"
        /// принадлежащего MainWindow
        /// </summary>
        public int AccessLevel
        {
            get
            {
                int? index = this.MWindow.AccessLevel_ComboBox.SelectedIndex;

                int s = index ?? 0;

                return s;
            }
        }

        public Consultant Consultant { get; } 

        public Meneger Meneger { get; } //??
        #endregion

        //конструктор
        public MainWindowViewModel(MainWindow mWindow) 
        {
            this.MWindow= mWindow;

            this.ClientsRepository = new ClientsRepository("data.csv");

            this.accountsRepo = new ObservableCollection<BankAccount>();

            this.Consultant = new Consultant();

            this.Meneger = new Meneger();   
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

        private RelayCommand newClientAddCommand = null;
        public RelayCommand NewClientAddCommand => 
            newClientAddCommand ?? (newClientAddCommand = new RelayCommand(AddNewClient, CanAddClient));


        private RelayCommand deleteClientCommand = null;
        public RelayCommand DeleteClientCommand => 
            deleteClientCommand ?? (deleteClientCommand = new RelayCommand(DeleteClient, CanDeleteClient));

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

        #region Методы для редактирование данных о клиенте ....

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

        private bool CanEdit(string args)
        {
            if (AccessLevel == 1 && CurrentClient != null 
                && !String.IsNullOrWhiteSpace(args) 
                && args != null)   
            { return true;}
            
            else { return false; }   
        }

        private bool CanDeleteClient()
        {
            if (AccessLevel == 1 && CurrentClient != null) { return true; }
            return false;
        }

        private bool CanAddClient()
        {
            if (AccessLevel == 1) { return true; }

            return false;
        }

        /// <summary>
        /// Метод редактирования номера телефона
        /// </summary>
        /// <param name="client"></param>
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
            else { ShowStatusBarText(changedClient.Error); }

        }

        /// <summary>
        /// Метод редактирования имени клиента
        /// </summary>
        /// <param name = "client" ></ param >
        private void EditName(string newName)
        {
            if (CurrentClient != null)
            {
                Client changedClient = Meneger.EditNameClient(CurrentClient, newName.Trim());

                if (changedClient.IsValid)
                {
                    int index = clientsRepository.IndexOf(CurrentClient);

                    clientsRepository.ReplaceClient(index, changedClient);
                }
                else
                {
                    ShowStatusBarText(changedClient.Error);
                }
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

        private void DeleteClient()
        {
            if (CurrentClient != null)
            {
               ClientsRepository.Remove(CurrentClient);
            }
            
        }
        /// <summary>
        /// Метод добавления нового клиенита
        /// </summary>
        private void AddNewClient()
        {
            NewClientWindow _windowNewClient = new NewClientWindow();

            _windowNewClient.Owner = this.MWindow;

            _windowNewClient.ShowDialog();

            if (_windowNewClient.DialogResult == true)
            {
                if (!ClientsRepository.Contains(_windowNewClient.NewClient))
                {
                    ClientsRepository.Add(_windowNewClient.NewClient);
                }

                else ShowStatusBarText("Клиент с такими данными уже существует");
            }
        }
        #endregion

        #region Методы для работы со счетами
        /// <summary>
        /// Проверка наличия открытого счета у клиента
        /// </summary>
        /// <returns>
        /// true - если у выбранного клиента отрыт счет
        /// false - если счет не открыт</returns></returns>
        private bool CanCloseDeposit()
        {
            var Client = AccountsRepo.Select(i => i.Owner);

            return Client.Contains(CurrentClient);
        }

        private void CloseDeposit()
        {
            if (this.CurrentAccount == null)
            {
                AccountsRepo.Remove(CurrentAccount);
            }   
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

        private void ShowStatusBarText(string message)
        {
            TextBlock statusBar = Application.Current.MainWindow.FindName("StatusBarText") as TextBlock;
            
            statusBar.Text = message;

            var timer = new System.Timers.Timer();

            timer.Interval = 3000;

            timer.Elapsed += delegate (object sender, System.Timers.ElapsedEventArgs e)
            {
                timer.Stop();
                //удалите текст сообщения о состоянии с помощью диспетчера, поскольку таймер работает в другом потоке
                MWindow.Dispatcher.BeginInvoke(new Action(() =>
                {
                    statusBar.Text = "";
                }));

            };
            timer.Start();
        }

    }
}
