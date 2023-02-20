using Modul_13.Models;
using Modul_13.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Modul_13.View
{
    /// <summary>
    /// Логика взаимодействия для PaneWorkingWithDeposit.xaml
    /// </summary>
    public partial class PanelWorkingWithDeposit : Page
    {
        public PanelWorkingWithDepositViewModel PanelWorkingWithDepositViewModel { get; set; }

        public PanelWorkingWithDeposit()
        {
            InitializeComponent();

            PanelWorkingWithDepositViewModel = new PanelWorkingWithDepositViewModel();

            PanelWorkingWithDepositViewModel.SumTransfer = this.SumTransfer_TextBox;
        }

        /// <summary>
        /// Обновляет спискок клентов с отркрытым счетом
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FixingSender_TextBox(object sender, RoutedEventArgs e)
        {
           IEnumerable<BankClient> banks= from client in PanelWorkingWithDepositViewModel.BankRepository
                                          where client.Deposit != null 
                                          where client != DataContext as BankClient
                                          select client;


            PanelWorkingWithDepositViewModel.OnlyDepositRepository = banks;

            this.List_OnlyDepositRepository.SelectedItem = null;
        }

        /// <summary>
        /// Выбор получателя платежа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectedRecipient(object sender, SelectionChangedEventArgs e)
        {
            PanelWorkingWithDepositViewModel.Recipient = this.List_OnlyDepositRepository.SelectedItem as BankClient;
        }
    }
}
