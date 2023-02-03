using Modul_13.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Modul_13.View
{
    /// <summary>
    /// Логика взаимодействия для PanelInfoAndEditClient.xaml
    /// </summary>
    public partial class PanelInfoAndEditClient : Page
    {
        public MainWindow MWindow = Application.Current.MainWindow as MainWindow;

        private PanelEditClientViewModel panelEditClientViewModel;
        public PanelEditClientViewModel PanelEditClientViewModel { get => panelEditClientViewModel;  }

        public PanelInfoAndEditClient()
        {
            InitializeComponent();

            panelEditClientViewModel = new PanelEditClientViewModel(MWindow);
        }
    }
}
