using Modul_13.Models;
using Modul_13.ViewModels;
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
       
        public Client NewClient { get { return ViewModel.NewClient; } }

        public NewClientWindowViewModel ViewModel { get;}

        public NewClientWindow()
        {
            InitializeComponent();

            this.ViewModel = new NewClientWindowViewModel(this);

            this.DataContext = ViewModel;

            this.NewClientPanel.DataContext = ViewModel.NewClient;
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
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
