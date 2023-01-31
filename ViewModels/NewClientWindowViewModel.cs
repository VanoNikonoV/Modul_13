using Modul_13.Cmds;
using Modul_13.Models;
using Modul_13.View;
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
        private NewClientWindow _window;

        private Client newClient;

        public Client NewClient { get => newClient; }

        public NewClientWindowViewModel(NewClientWindow window)
        {
            _window = window;
            newClient = new Client();
        }

        private RelayCommand addClientCommand = null;
        public RelayCommand AddClientCommand => addClientCommand ?? (new RelayCommand(AddClient));

        private void AddClient()
        {
            newClient = new Client(_window.FirstNameTextBox.Text,
                                   _window.MidlleNameTextBox.Text,
                                   _window.SecondNameTextBox.Text,
                                   _window.TelefonTextBox.Text,
                                   _window.SeriesAndPassportNumberTextBox.Text);

            if (newClient.Error == string.Empty || newClient.IsValid == true)
            {
                _window.DialogResult = true;
            }

            else MessageBox.Show(messageBoxText: NewClient.Error,
                             caption: "Ощибка в данных",
                             MessageBoxButton.OK,
                             icon: MessageBoxImage.Error);

        }

    }
}
