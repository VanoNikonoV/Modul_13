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

        public PanelWorkingWithDepositViewModel PanelWorkingWithDepositViewModel { get; set; }

        public PanelWorkingWithDeposit()
        {
            InitializeComponent();

            PanelWorkingWithDepositViewModel = new PanelWorkingWithDepositViewModel();

            //this.DataContextChanged += PanelWorkingWithDeposit_DataContextChanged;
        }
    }
}
