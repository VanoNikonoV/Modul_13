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
        public MainWindow MWindow = Application.Current.MainWindow as MainWindow;

        private PanelWorkingWithDepositViewModel panelWorkingWithDepositViewModel;

        public PanelWorkingWithDepositViewModel PanelWorkingWithDepositViewModel { get => panelWorkingWithDepositViewModel;  }

        public BankRepository BanksRepository { get; set; }

        public PanelWorkingWithDeposit()
        {
            InitializeComponent();

            BanksRepository = MWindow.ViewModel.BankRepository;

            panelWorkingWithDepositViewModel = new PanelWorkingWithDepositViewModel(MWindow);

            this.DataContextChanged += PanelWorkingWithDeposit_DataContextChanged;
        }

        /// <summary>
        /// Метод передает обновленные данный о текущем клиенте выбранном в DataClients
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelWorkingWithDeposit_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BankAccount clienChanged = e.NewValue as BankAccount;

            PanelWorkingWithDepositViewModel.UpdateCurrentClient(clienChanged);
        }

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
