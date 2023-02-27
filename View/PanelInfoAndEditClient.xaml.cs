using Modul_13.Models;
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
        public PanelEditClientViewModel PanelEditClientViewModel { get; set; }

        public PanelInfoAndEditClient()
        {
            InitializeComponent();

            PanelEditClientViewModel = new PanelEditClientViewModel();    
        }
    }
}
