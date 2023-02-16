using Modul_13.Models;
using Modul_13.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Modul_13.View
{
    /// <summary>
    /// Логика взаимодействия для PaneWorkingWithDeposit.xaml
    /// </summary>
    public partial class PanelWorkingWithDeposit : Page
    {
        //public MainWindow MWindow = Application.Current.MainWindow as MainWindow;

        private PanelWorkingWithDepositViewModel panelWorkingWithDepositViewModel;

        public PanelWorkingWithDepositViewModel PanelWorkingWithDepositViewModel { get => panelWorkingWithDepositViewModel;  }

        public PanelWorkingWithDeposit()
        {
            InitializeComponent();

            panelWorkingWithDepositViewModel = new PanelWorkingWithDepositViewModel();

            //this.DataContextChanged += PanelWorkingWithDeposit_DataContextChanged;
        }

        /// <summary>
        /// Метод передает данный о текущем клиенте при смене TabItem, выбранном в списке DataClients
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void PanelWorkingWithDeposit_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    BankAccount clienChanged = e.NewValue as BankAccount;

        //    if (clienChanged != null)
        //    {
        //       PanelWorkingWithDepositViewModel.UpdateCurrentClient(clienChanged);
        //    }
        //}

        private void CustomersСhoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CustomersСhoice_Selected(object sender, RoutedEventArgs e)
        {

        }

        //private void Transfer_Button_Clik(object sender, RoutedEventArgs e)
        //{
        //    this.List_BankAccount.SelectedIndex = -1;
        //    this.SumTransfer_TextBox.Text = string.Empty;
        //}
    }
}
