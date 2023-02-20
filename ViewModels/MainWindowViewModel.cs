using Modul_13.Cmds;
using Modul_13.Commands;
using Modul_13.Models;
using Modul_13.View;
using Modul_13.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Modul_13.ViewModels
{
    public class MainWindowViewModel:ViewModel
    {
        #region Свойства
        private BankRepository bankRepository;
        public BankRepository BankRepository 
        {
            get=> bankRepository;
            private set 
            {
                Set(ref bankRepository, value, "BankRepository");
            }
        }

        /// <summary>
        /// Выбранный из базы ClientsRepository клиент
        /// определяется на основании выбраного параметра в элементе ListView "DataClients"
        /// принадлежащего MainWindow
        /// </summary>
        public MainWindow MWindow { get;}

        private BankClient currentClient = null;
        public BankClient CurrentClient { get => currentClient = this.MWindow.DataClients.SelectedItem as BankClient; }

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

            this.BankRepository = new BankRepository();

            this.Consultant = new Consultant();

            this.Meneger = new Meneger();   
        }

        #region Команды
        private RelayCommand newClientAddCommand = null;
        public RelayCommand NewClientAddCommand => 
            newClientAddCommand ?? (newClientAddCommand = new RelayCommand(AddNewClient, CanAddClient));

        private RelayCommand deleteClientCommand = null;
        public RelayCommand DeleteClientCommand => 
            deleteClientCommand ?? (deleteClientCommand = new RelayCommand(DeleteClient, CanDeleteClient));

        #endregion

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
        /// Метод удаления нового клиента
        /// </summary>
        private void DeleteClient()
        {
            if (CurrentClient != null) { BankRepository.Remove(CurrentClient); }
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
                //if (!BankRepository.Contains(_windowNewClient.NewClient))
                //{
                    BankClient newAccount = new BankClient(_windowNewClient.NewClient);
                    
                    BankRepository.Add(newAccount);
                //}

                //else ShowStatusBarText("Клиент с такими данными уже существует");
            }
        }

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
