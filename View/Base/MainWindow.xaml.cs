using Microsoft.Win32;
using Modul_13.Models;
using Modul_13.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Modul_13
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; set; }

        //ICollectionView collectionView { get; set; }

        private bool isDirty = false;

        public MainWindow()
        {
            ViewModel = ViewModel ?? new MainWindowViewModel();

            this.DataContext = ViewModel;

            //collectionView = CollectionViewSource.GetDefaultView(DataClients.ItemsSource);

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

                    DataClients.ItemsSource = ViewModel.Consultant.ViewClientsData(ViewModel.ClientsRepository);

                    break;

                case 1: //менждер

                    //collectionView.SortDescriptions.Clear();

                    DataClients.ItemsSource = ViewModel.Meneger.ViewClientsData(ViewModel.ClientsRepository);

                    break;

                default:
                    break;

            }
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            СhangesClient.Visibility = Visibility.Collapsed;
            ListChanges_Label.Visibility = Visibility.Visible;
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
            СhangesClient.Visibility = Visibility.Visible;
            ListChanges_Label.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Производит сортировку по алфавиту по имени клиента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sort_Button_Click(object sender, RoutedEventArgs e)
        {
            //collectionView.SortDescriptions.Add(new SortDescription("FirstName", ListSortDirection.Ascending));

            //DataClients.ItemsSource = collectionView;
        }

        private void SaveCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (isDirty)
            {
                e.CanExecute = true;
            }
            else e.CanExecute = false;

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

                foreach (var client in ViewModel.ClientsRepository)
                {
                    client.IsChanged = false;
                }
                isDirty = false;
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
                    this.Width = 1150;
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
