using Modul_13.Cmds;
using Modul_13.Commands;
using Modul_13.Models;
using Modul_13.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Modul_13.ViewModels
{
    public class PanelEditClientViewModel:ViewModel
    {
        /// <summary>
        /// AccessLevel определяется на основании выбраного параметра в элементе ListView "DataClients"
        /// принадлежащего MainWindow
        /// </summary>
        public MainWindow MWindow { get; } 

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

        //private bankRepository bankRepository;

        ///// <summary>
        ///// Позволяет получить и изменить базу данных с клиентами
        ///// </summary>
        //public bankRepository bankRepository
        //{
        //    get => bankRepository;

        //    private set
        //    {
        //        Set(ref bankRepository, value, "bankRepository");

        //        //if (bankRepository == value) return;
        //        //bankRepository = value;
        //    }
        //}
        private BankRepository bankRepository;
        public BankRepository BankRepository
        {
            get => bankRepository;
            private set
            {
                Set(ref bankRepository, value, "BankRepository");
            }
        }
        public BankAccount CurrentClient { get => this.MWindow.DataClients.SelectedItem as BankAccount; }
        public Consultant Consultant { get; }
        public Meneger Meneger { get; }

        public PanelEditClientViewModel(MainWindow mWindow)
        {
            this.MWindow = mWindow;

            this.Consultant = new Consultant();

            this.Meneger = new Meneger();

            this.BankRepository = MWindow.ViewModel.BankRepository;

            //this.bankRepository = MWindow.ViewModel.bankRepository;
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
            Client changedClient = Meneger.EditNameClient(CurrentClient.Owner, newName.Trim());

            if (changedClient.IsValid)
            {
                int index = bankRepository.IndexOf(CurrentClient);

                bankRepository.ReplaceClient(index, changedClient);
            }
        }
        private void EditTelefon(string telefon)
        {
            string whatChanges = string.Format(CurrentClient.Owner.Telefon + @" на " + telefon.Trim());

            CurrentClient.Owner.Telefon = telefon;

            if (CurrentClient.Owner.IsValid)
            {
                //изменения в коллекции банка, по ID клиента
                BankAccount editClient = bankRepository.First(i => i.Owner.ID == CurrentClient.Owner.ID); // try

                editClient.Owner.Telefon = telefon;

                switch (this.AccessLevel)
                {
                    case 0: //консультант

                        editClient.Owner.InfoChanges.Add(new InformationAboutChanges(DateTime.Now, whatChanges, "замена", nameof(Consultant)));

                        break;

                    case 1: //менждер

                        editClient.Owner.InfoChanges.Add(new InformationAboutChanges(DateTime.Now, whatChanges, "замена", nameof(Meneger)));

                        break;

                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show(messageBoxText: CurrentClient.Owner.Error,
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
                Client changedClient = Meneger.EditMiddleNameClient(CurrentClient.Owner, middleName.Trim());

                int index = bankRepository.IndexOf(CurrentClient);

                bankRepository.ReplaceClient(index, changedClient);
            }

        }

        private void EditSecondName(string secondName)
        {
            if (CurrentClient != null)
            {
                Client changedClient = Meneger.EditSecondNameClient(CurrentClient.Owner, secondName.Trim());

                int index = bankRepository.IndexOf(CurrentClient);

                bankRepository.ReplaceClient(index, changedClient);
            }
        }

        private void EditSeriesAndPassportNumber(string passport)
        {
            if (CurrentClient != null)
            {
                Client changedClient = Meneger.EditSeriesAndPassportNumberClient(CurrentClient.Owner, passport.Trim());

                int index = bankRepository.IndexOf(CurrentClient);

                bankRepository.ReplaceClient(index, changedClient);
            }
        }
    }
}
