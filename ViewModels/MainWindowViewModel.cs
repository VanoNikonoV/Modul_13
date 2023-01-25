using Modul_13.Cmds;
using Modul_13.Commands;
using Modul_13.Models;
using Modul_13.View;
using Modul_13.ViewModels.Base;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Modul_13.ViewModels
{
    public class MainWindowViewModel:ViewModel
    {
        private ClientsRepository clientsRepository;

        /// <summary>
        /// Позволяет получить и изменить базу данных с клиентами
        /// </summary>
        public ClientsRepository ClientsRepository
        {
            get => clientsRepository;

            set
            {
                //if (clientsRepository == value) return;

                //this.clientsRepository = value;

                Set(ref clientsRepository, value, "ClientsRepository");

                //OnPropertyChanged("ClientsRepository"); //??
            }
        }

        /// <summary>
        /// Выбранный из базы ClientsRepository клиент
        /// определяется на основании выбраного параметра в элементе ListView "DataClients"
        /// принадлежащего MainWindow
        /// </summary>
        public Client CurrentClient 
        {
            get
            {
                ListView Data = Application.Current.MainWindow.FindName("DataClients") as ListView;

                return Data.SelectedItem as Client;
            }
        }

        /// <summary>
        /// Уровень доступа к базе данных для консультанта и менаджера, 
        /// определяется на основании выбраного параметра в элементе ComboBox "AccessLevel_ComboBox"
        /// принадлежащего MainWindow
        /// </summary>
        public int AccessLevel
        {
            get
            {
                ComboBox comboBox = Application.Current.MainWindow.FindName("AccessLevel_ComboBox") as ComboBox;

                Window w = Application.Current.MainWindow;

                int? x = comboBox.SelectedIndex;

                int s = x ?? 0;

                return s;
            }
        }

        public Consultant Consultant { get; set; }

        public Meneger Meneger { get; set; }

        public MainWindowViewModel()
        {
            ClientsRepository = new ClientsRepository("data.csv");

            Consultant = new Consultant();

            Meneger = new Meneger();

            ClientsRepository.CollectionChanged += ClientsRepository_CollectionChanged;

        }

        private void ClientsRepository_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                
                foreach (Client p in e.OldItems)
                {
                    
                }
                
            }

        }

        #region Команды

        private RelayCommand<string> _editTelefonCommand = null;
        public RelayCommand<string> EditTelefonCommand
            => _editTelefonCommand ?? (_editTelefonCommand = new RelayCommand<string>(EditTelefon, CanEditTelefon));


        private RelayCommand<object> editNameCommand = null;
        public RelayCommand<object> EditNameCommand =>
            editNameCommand ?? (editNameCommand = new RelayCommand<object>(EditName, CanEdit));


        private RelayCommand<object> editMiddleNameCommand = null;
        public RelayCommand<object> EditMiddleNameCommand =>
            editMiddleNameCommand ?? (editMiddleNameCommand = new RelayCommand<object>(EditMiddleName, CanEdit));


        private RelayCommand<object> editSecondNameCommand = null;
        public RelayCommand<object> EditSecondNameCommand =>
            editSecondNameCommand ?? (editSecondNameCommand = new RelayCommand<object>(EditSecondName, CanEdit));


        private RelayCommand<object> editSeriesAndPassportNumberCommand = null;
        public RelayCommand<object> EditSeriesAndPassportNumberCommand =>
            editSeriesAndPassportNumberCommand ?? (editSeriesAndPassportNumberCommand
            = new RelayCommand<object>(EditSeriesAndPassportNumber, CanEdit));

        private RelayCommand<int> newClientAddCommand = null;
        public RelayCommand<int> NewClientAddCommand => 
            newClientAddCommand ?? (newClientAddCommand = new RelayCommand<int>(NewClient, CanAddClient));


        private RelayCommand<object> deleteClientCommand = null;
        public RelayCommand<object> DeleteClientCommand => 
            deleteClientCommand ?? (deleteClientCommand = new RelayCommand<object>(DeleteClient, CanEdit));

        private RelayCommand addDepositCommand = null;
        public RelayCommand AddDepositCommand =>
            addDepositCommand ?? (addDepositCommand = new RelayCommand(AddDeposit, CanAddDeposit));

        private bool CanAddDeposit()
        {
            return true;
        }

        private void AddDeposit()
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Редактирование данных о клиенте

        /// <summary>
        /// Опреляет допускается ли редактировать номер телефона клиента
        /// </summary>
        /// <param name="args"></param>
        /// <returns>true - если для данного уровня доступа доступна возможность редактировани
        /// false - если для данного уровня доступа недостукается редактировани</returns>
        private bool CanEditTelefon(string args)
        {
            if (args != null && !String.IsNullOrWhiteSpace(args))
            {
                return true;
            }
            
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

                switch (AccessLevel)
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
                //isDirty = true;

            }
            // else { ShowStatusBarText("Исправте не корректные данные"); }

        }

        private bool CanEdit(object Arg)
        {
            if (Arg != null)
            {
                var array = Arg as object[];

                Client client = (Client)array[0];

                int accessLevel = (int)array[2];

                if (client != null && accessLevel== 1) { return true; }
            }
            return false;
        }

        private bool CanAddClient(int accessLevel)
        {
            if (accessLevel == 1) { return true; }

            return false;
        }

        /// <summary>
        /// Метод редактирования имени клиента
        /// </summary>
        /// <param name = "client" ></ param >
        private void EditName(object Arg)
        {
            var array = Arg as object[];
            Client client = (Client)array[0];
            string newName = (string)array[1];

            if (client != null)
            {
                Client changedClient = Meneger.EditNameClient(client, newName.Trim());

                if (changedClient.IsValid)
                {
                    int index = ClientsRepository.IndexOf(client);

                    ClientsRepository.ReplaceClient(index, changedClient);
                }
                else
                {
                    ShowStatusBarText(changedClient.Error);
                }

                //isDirty = true;
            }
            //else ShowStatusBarText("Выберите клиента");
        }

        /// <summary>
        /// Метод редактирования отчества клиента
        /// </summary>
        /// <param name = "client" ></ param >
        private void EditMiddleName(object Arg)
        {
            var array = Arg as object[];
            Client client = (Client)array[0];
            string middleName = (string)array[1];

            if (client != null)
            {
                Client changedClient = Meneger.EditMiddleNameClient(client, middleName.Trim());

                int index = ClientsRepository.IndexOf(client);

                ClientsRepository.ReplaceClient(index, changedClient);
            }
        }

        private void EditSecondName(object Arg)
        {
            var array = Arg as object[];
            Client client = (Client)array[0];
            string secondName = (string)array[1];

            if (client != null)
            {
                Client changedClient = Meneger.EditSecondNameClient(client, secondName.Trim());

                int index = ClientsRepository.IndexOf(client);

                ClientsRepository.ReplaceClient(index, changedClient);
            }
            
        }

        private void EditSeriesAndPassportNumber(object Arg)
        {
            var array = Arg as object[];
            Client client = (Client)array[0];
            string passport = (string)array[1];

            if (client != null)
            {
                Client changedClient = Meneger.EditSeriesAndPassportNumberClient(client, passport.Trim());

                int index = ClientsRepository.IndexOf(client);

                ClientsRepository.ReplaceClient(index, changedClient);
            }
        }

        private void DeleteClient(object Arg)
        {
            if (Arg != null)
            {
                var array = Arg as object[];

                Client client = (Client)array[0];

               ClientsRepository.Remove(client);
            }
            
        }
        #endregion

        /// <summary>
        /// Метод добавления нового клиенита
        /// </summary>
        private void NewClient(int x)
        {
            NewClientWindow _windowNewClient = new NewClientWindow();

            _windowNewClient.Owner = Application.Current.MainWindow;

            _windowNewClient.ShowDialog();

            if (_windowNewClient.DialogResult == true)
            {
                if (!ClientsRepository.Contains(_windowNewClient.NewClient))
                {
                    ClientsRepository.Add(_windowNewClient.NewClient);
                }

                //isDirty = true;
            }
        }

        private void ShowStatusBarText(string message)
        {
            TextBlock statusBar = Application.Current.MainWindow.FindName("StatusBarText") as TextBlock;
            
            statusBar.Text = message;

            var timer = new System.Timers.Timer();

            timer.Interval = 2000;

            timer.Elapsed += delegate (object sender, System.Timers.ElapsedEventArgs e)
            {
                timer.Stop();
                //удалите текст сообщения о состоянии с помощью диспетчера, поскольку таймер работает в другом потоке
                //Application.Current.MainWindow.Dispatcher.BeginInvoke(new Action(() =>
                //{
                //    statusBar.Text = "";
                //}));

            };
            timer.Start();
        }

    }
}
