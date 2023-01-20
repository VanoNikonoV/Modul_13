using Modul_13.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Modul_13.View
{
    /// <summary>
    /// Логика взаимодействия для NewClientWindow.xaml
    /// </summary>
    public partial class NewClientWindow : Window
    {
        public Client NewClient { get; private set; }

        private Client temp;

        public NewClientWindow()
        {
            InitializeComponent();

            temp = new Client();

            NewClientPanel.DataContext = temp;
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        private void AddClient(object sender, RoutedEventArgs e)
        {
            if (temp.Error == string.Empty)
            {
                NewClient = new Client(FirstNameTextBox.Text.Trim(),
                                        MidlleNameTextBox.Text.Trim(),
                                        SecondNameTextBox.Text.Trim(),
                                        TelefonTextBox.Text.Trim(),
                                        SeriesAndPassportNumberTextBox.Text.Trim());
                DialogResult = true;
            }

            else MessageBox.Show(messageBoxText: temp.Error,
                             caption: "Ощибка в данных",
                             MessageBoxButton.OK,
                             icon: MessageBoxImage.Error);

        }

        private bool isFocused = false;
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            isFocused = true;
        }

        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (isFocused)
            {
                isFocused = false;
                (sender as TextBox).SelectAll();
            }
        }
    }
}
