using Modul_13.Cmds;
using Modul_13.Models;
using Modul_13.View;
using System;
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
            newClient = new Client(firstName: _window.FirstNameTextBox.Text,
                                  middleName: _window.MidlleNameTextBox.Text,
                                  secondName: _window.SecondNameTextBox.Text,
                                     telefon: _window.TelefonTextBox.Text,
                     seriesAndPassportNumber: _window.SeriesAndPassportNumberTextBox.Text,
                                   currentId: newClient.ID,
                                    dateTime: DateTime.Now,
                                   isChanged: false);

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
