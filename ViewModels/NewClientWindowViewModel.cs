using Modul_13.Cmds;
using Modul_13.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Modul_13.ViewModels
{
    public class NewClientWindowViewModel
    {
        public NewClientWindowViewModel() { TempClient = new Client(); }

        /// <summary>
        /// Временный клиент для заполнения полей и корректной работы DataErrorInfo
        /// </summary>
        public Client TempClient { get; }

        private Client newClient;
        public Client NewClient { get => newClient; }

        private bool isErrorDataClient = false;
        public bool IsErrorDataClient { get => isErrorDataClient; }

        private RelayCommand addClientCommand = null;
        public RelayCommand AddClientCommand => addClientCommand ?? (new RelayCommand(AddClient));

        private void AddClient()
        {
             newClient = new Client(TempClient.FirstName,
                                        TempClient.MiddleName,
                                        TempClient.SecondName,
                                        TempClient.Telefon,
                                        TempClient.SeriesAndPassportNumber);

            if (newClient.Error == string.Empty || newClient.Error == null)
            {
                isErrorDataClient = true;
            }

            //else MessageBox.Show(messageBoxText: NewClient.Error,
            //                 caption: "Ощибка в данных",
            //                 MessageBoxButton.OK,
            //                 icon: MessageBoxImage.Error);

        }

    }
}
