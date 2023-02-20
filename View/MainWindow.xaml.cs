using Microsoft.Win32;
using Modul_13.Models;
using Modul_13.View;
using Modul_13.ViewModels;
using Modul_13.ViewModels.Base;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Modul_13
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; set; }

        public  ICollectionView CollectionView { get; private set; }

        public MainWindow()
        {
            ViewModel = ViewModel ?? new MainWindowViewModel(this);

            this.DataContext = ViewModel;

            CollectionView = CollectionViewSource.GetDefaultView(ViewModel.BankRepository);
            
            InitializeComponent(); 
        }

        private void CloseWindows(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Выбор функицонала консультант / менаджер
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AccessLevel_ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            switch (AccessLevel_ComboBox.SelectedIndex)
            {
                case 0: //консультант

                    DataClients.ItemsSource = CollectionView;
                    //DataClients.ItemsSource = CollectionViewSource.GetDefaultView(ViewModel.Consultant.ViewClientsData(ViewModel.BankRepository));
                    // Для консультанта нужно реализовать поиск в базе менаджера
                    // По ID?

                    break;

                case 1: //менждер

                    DataClients.ItemsSource = CollectionView;

                    break;

                default:
                    break;

            }
        }

        /// <summary>
        /// Для анимации закрытия списка изменений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            СhangesClient.Visibility = Visibility.Collapsed;
            ListChanges_Label.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Для анимации открытия списка изменений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
            СhangesClient.Visibility = Visibility.Visible;
            ListChanges_Label.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Для корректной работы метода Sort_Button_Click
        /// </summary>
        private bool isSort = false;

        /// <summary>
        /// Производит сортировку по алфавиту по имени клиента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sort_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!isSort)
            {
                CollectionView.SortDescriptions.Add(new SortDescription("FirstName", ListSortDirection.Ascending));

                DataClients.ItemsSource = CollectionView;

                isSort = true;
            }
            else 
            {
                CollectionView.SortDescriptions.Clear();

                isSort = false;
            }
        }

        private void SaveCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //foreach (var client in ViewModel.ClientsRepository)
            //{
            //    if (client.IsChanged == true) { e.CanExecute = true; break; }

            //    else e.CanExecute = false;
            //}
        }

        private void SaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var saveDlg = new SaveFileDialog { Filter = "Text files|*.csv" };

            if (true == saveDlg.ShowDialog())
            {
                string fileName = saveDlg.FileName;

                using (StreamWriter sw = new StreamWriter(fileName, false, Encoding.Unicode))
                {
                    foreach (var emp in DataClients.ItemsSource)
                    {
                        sw.WriteLine(emp.ToString());
                    }
                }

                foreach (var client in ViewModel.BankRepository)
                {
                    client.Owner.IsChanged = false;
                }
            }

        }

        /// <summary>
        /// Перемещение окна в пространестве рабочего стола
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { this.DragMove(); }
        }

        private bool IsMaximized = false;
        /// <summary>
        /// Сворачивание окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Gride_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2) 
            {
                if (IsMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Height = 800;
                    this.Width = 1250;
                    IsMaximized= false;
                }
                else 
                {
                    this.WindowState = WindowState.Maximized;
                    IsMaximized= true;
                }
            }
        }

    }
}
